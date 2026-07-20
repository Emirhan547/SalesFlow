using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SalesFlow.Business.Dtos.NoteDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.RealtimeServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.NoteRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.NoteServices
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly NoteBusinessRules _businessRules;
        private readonly IValidator<CreateNoteDto> _createValidator;
        private readonly IValidator<UpdateNoteDto> _updateValidator;
        private readonly IActivityLogService _activityLogService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRealtimeService _realtimeService;
        public NoteService(INoteRepository noteRepository, IUnitOfWork unitOfWork, NoteBusinessRules businessRules, IValidator<CreateNoteDto> createValidator, IValidator<UpdateNoteDto> updateValidator, IActivityLogService activityLogService, ICurrentUserService currentUserService, IRealtimeService realtimeService)
        {
            _noteRepository = noteRepository;
            _unitOfWork = unitOfWork;
            _businessRules = businessRules;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _activityLogService = activityLogService;
            _currentUserService = currentUserService;
            _realtimeService = realtimeService;
        }

        public async Task<Result> CreateAsync(CreateNoteDto dto)
        {
            await ValidateAndThrowAsync(_createValidator, dto);
            await _businessRules.EnsureCustomerExistsAsync(dto.CustomerId);
            var note = dto.Adapt<Note>();

            await _noteRepository.AddAsync(note);
            await _activityLogService.AddAsync(ActivityAction.Create,nameof(Note), note.Id,"Note created.", _currentUserService.UserId);
            await _unitOfWork.SaveChangesAsync();
            await _realtimeService.DashboardUpdatedAsync();
            return Result.Success("Note created successfully.");
        }

        public async Task<Result> UpdateAsync(UpdateNoteDto dto)
        {
            await ValidateAndThrowAsync(_updateValidator, dto);
            var note = await _businessRules.GetNoteByIdAsync(dto.Id, true);
            _businessRules.EnsureUserCanModify(note);
            await _businessRules.EnsureCustomerExistsAsync(dto.CustomerId);
            dto.Adapt(note);
            _noteRepository.Update(note);
            await _activityLogService.AddAsync(ActivityAction.Update, nameof(Note), note.Id, "Note updated.", _currentUserService.UserId);
            await _unitOfWork.SaveChangesAsync();
            await _realtimeService.DashboardUpdatedAsync();
            return Result.Success("Note updated successfully.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var note = await _businessRules.GetNoteByIdAsync(id, true);
            _businessRules.EnsureUserCanModify(note);
            _noteRepository.Delete(note);
            await _activityLogService.AddAsync(ActivityAction.Delete, nameof(Note), note.Id, "Note deleted.", _currentUserService.UserId);
            await _unitOfWork.SaveChangesAsync();
            await _realtimeService.DashboardUpdatedAsync();
            return Result.Success("Note deleted successfully.");
        }

        public async Task<Result<PagedResult<ResultNoteDto>>> GetAllAsync(PaginationRequest request)
        {
            IQueryable<Note> query = _noteRepository.GetAll();

            if (!_currentUserService.IsInRole("Admin") &&
                !_currentUserService.IsInRole("SalesManager"))
            {
                query = query.Where(x =>
                    x.CreatedById == _currentUserService.UserId);
            }
            var notes = await _noteRepository
     .GetAll()
     .Select(x => new ResultNoteDto
     {
         Id = x.Id,
         Content = x.Content,

         CustomerId = x.CustomerId,

         CustomerName =
             !string.IsNullOrWhiteSpace(x.Customer.CompanyName)
                 ? x.Customer.CompanyName
                 : x.Customer.ContactFirstName + " " + x.Customer.ContactLastName,

         CreatedById = x.CreatedById,

         CreatedByName =
             x.CreatedBy == null
                 ? null
                 : x.CreatedBy.FirstName + " " + x.CreatedBy.LastName
     })
     .ToPagedResultAsync(request);
            return Result<PagedResult<ResultNoteDto>>.Success(notes);
        }

        public async Task<Result<GetByIdNoteDto>> GetByIdAsync(int id)
        {
            var note = await _noteRepository
      .GetAll()
      .Include(x => x.Customer)
      .Include(x => x.CreatedBy)
      .FirstOrDefaultAsync(x => x.Id == id);

            if (note is null)
                throw new NotFoundException("Note not found.");

            _businessRules.EnsureUserCanModify(note);

            GetByIdNoteDto dto = new()
            {
                Id = note.Id,
                Content = note.Content,

                CustomerId = note.CustomerId,

                CustomerName =
                    !string.IsNullOrWhiteSpace(note.Customer.CompanyName)
                        ? note.Customer.CompanyName
                        : $"{note.Customer.ContactFirstName} {note.Customer.ContactLastName}",

                CreatedById = note.CreatedById,

                CreatedByName =
                    note.CreatedBy == null
                        ? null
                        : $"{note.CreatedBy.FirstName} {note.CreatedBy.LastName}"
            };
            return Result<GetByIdNoteDto>.Success(dto);
        }
        private static async Task ValidateAndThrowAsync<TDto>(IValidator<TDto> validator, TDto dto)
        {
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}
