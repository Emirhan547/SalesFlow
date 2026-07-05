using FluentValidation;
using Mapster;
using SalesFlow.Business.Dtos.NoteDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.NoteRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
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

        public NoteService(INoteRepository noteRepository, IUnitOfWork unitOfWork,NoteBusinessRules businessRules, IValidator<CreateNoteDto> createValidator, IValidator<UpdateNoteDto> updateValidator)
        {
            _noteRepository = noteRepository;
            _unitOfWork = unitOfWork;
            _businessRules = businessRules;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<Result> CreateAsync(CreateNoteDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _businessRules.EnsureCustomerExistsAsync(dto.CustomerId);
            await _businessRules.EnsureCreatedByExistsAsync(dto.CreatedById);
            var note = dto.Adapt<Note>();

            await _noteRepository.AddAsync(note);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success("Note created successfully.");
        }

        public async Task<Result> UpdateAsync(UpdateNoteDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);
            var note = await _businessRules.GetNoteByIdAsync(dto.Id, true);
            await _businessRules.EnsureCustomerExistsAsync(dto.CustomerId);
            await _businessRules.EnsureCreatedByExistsAsync(dto.CreatedById);
            dto.Adapt(note);
            _noteRepository.Update(note);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Note updated successfully.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var note = await _businessRules.GetNoteByIdAsync(id, true);
            _noteRepository.Delete(note);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Note deleted successfully.");
        }

        public async Task<Result<PagedResult<ResultNoteDto>>> GetAllAsync(PaginationRequest request)
        {
            var notes = await _noteRepository.GetAll().ProjectToType<ResultNoteDto>() .ToPagedResultAsync(request);
            return Result<PagedResult<ResultNoteDto>>.Success(notes);
        }

        public async Task<Result<GetByIdNoteDto>> GetByIdAsync(int id)
        {
            var note = await _businessRules.GetNoteByIdAsync(id);
            var dto = note.Adapt<GetByIdNoteDto>();
            return Result<GetByIdNoteDto>.Success(dto);
        }
    }
}
