using FluentValidation;
using Mapster;
using SalesFlow.Business.Dtos.MeetingDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.MeetingRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.MeetingServices
{
    public class MeetingService : IMeetingService
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly MeetingBusinessRules _businessRules;
        private readonly IValidator<CreateMeetingDto> _createValidator;
        private readonly IValidator<UpdateMeetingDto> _updateValidator;
        private readonly IActivityLogService _activityLogService;
        private readonly ICurrentUserService _currentUserService;
        public MeetingService(IMeetingRepository meetingRepository, IUnitOfWork unitOfWork, MeetingBusinessRules businessRules, IValidator<CreateMeetingDto> createValidator, IValidator<UpdateMeetingDto> updateValidator, IActivityLogService activityLogService, ICurrentUserService currentUserService)
        {
            _meetingRepository = meetingRepository;
            _unitOfWork = unitOfWork;
            _businessRules = businessRules;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _activityLogService = activityLogService;
            _currentUserService = currentUserService;
        }

        public async Task<Result> CreateAsync(CreateMeetingDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _businessRules.EnsureCustomerExistsAsync(dto.CustomerId);
            await _businessRules.EnsureAssignedUserExistsAsync(dto.AssignedUserId);
            await _businessRules.EnsureNoMeetingConflictAsync( dto.AssignedUserId, dto.StartDate,dto.EndDate);

            var meeting = dto.Adapt<Meeting>();
            meeting.Status = MeetingStatus.Scheduled;
            await _meetingRepository.AddAsync(meeting);
            await _activityLogService.AddAsync(ActivityAction.Create,nameof(Meeting),meeting.Id,$"Meeting '{meeting.Title}' created.",_currentUserService.UserId);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Meeting created successfully.");
        }

        public async Task<Result> UpdateAsync(UpdateMeetingDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var meeting = await _businessRules.GetMeetingByIdAsync(dto.Id, true);
            await _businessRules.EnsureCustomerExistsAsync(dto.CustomerId);

            await _businessRules.EnsureAssignedUserExistsAsync(dto.AssignedUserId);
            await _businessRules.EnsureNoMeetingConflictForUpdateAsync( dto.Id, dto.AssignedUserId, dto.StartDate, dto.EndDate);
            _businessRules.EnsureStatusChanged( meeting.Status, dto.Status);

            _businessRules.EnsureStatusTransition(  meeting.Status, dto.Status);
            dto.Adapt(meeting);
            _meetingRepository.Update(meeting);
            await _activityLogService.AddAsync( ActivityAction.Update,nameof(Meeting),meeting.Id,$"Meeting '{meeting.Title}' updated.",_currentUserService.UserId);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success("Meeting updated successfully.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var meeting = await _businessRules.GetMeetingByIdAsync(id, true);

            _meetingRepository.Delete(meeting);
            await _activityLogService.AddAsync(ActivityAction.Delete, nameof(Meeting),meeting.Id,$"Meeting '{meeting.Title}' deleted.", _currentUserService.UserId);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success("Meeting deleted successfully.");
        }

        public async Task<Result<PagedResult<ResultMeetingDto>>> GetAllAsync(PaginationRequest request)
        {
            var meetings = await _meetingRepository.GetAll().ProjectToType<ResultMeetingDto>().ToPagedResultAsync(request);
            return Result<PagedResult<ResultMeetingDto>>.Success(meetings);
        }

        public async Task<Result<GetByIdMeetingDto>> GetByIdAsync(int id)
        {
            var meeting = await _businessRules.GetMeetingByIdAsync(id);
            var dto = meeting.Adapt<GetByIdMeetingDto>();
            return Result<GetByIdMeetingDto>.Success(dto);
        }
    }
}
