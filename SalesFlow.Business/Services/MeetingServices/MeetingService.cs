using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SalesFlow.Business.Dtos.MeetingDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.NotificationServices;
using SalesFlow.Business.Services.RealtimeServices;
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
        private readonly IRealtimeService _realtimeService;
        private readonly INotificationService _notificationService;
        public MeetingService(IMeetingRepository meetingRepository, IUnitOfWork unitOfWork, MeetingBusinessRules businessRules, IValidator<CreateMeetingDto> createValidator, IValidator<UpdateMeetingDto> updateValidator, IActivityLogService activityLogService, ICurrentUserService currentUserService, IRealtimeService realtimeService, INotificationService notificationService)
        {
            _meetingRepository = meetingRepository;
            _unitOfWork = unitOfWork;
            _businessRules = businessRules;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _activityLogService = activityLogService;
            _currentUserService = currentUserService;
            _realtimeService = realtimeService;
            _notificationService = notificationService;
        }

        public async Task<Result> CreateAsync(CreateMeetingDto dto)
        {
            await ValidateAndThrowAsync(_createValidator, dto);

            await _businessRules.EnsureCustomerExistsAsync(
                dto.CustomerId);

            await _businessRules.EnsureAssignedUserExistsAsync(
                dto.AssignedUserId);

            await _businessRules.EnsureNoMeetingConflictAsync(
                dto.AssignedUserId,
                dto.StartDate,
                dto.EndDate);

            var meeting = dto.Adapt<Meeting>();

            meeting.Status = MeetingStatus.Scheduled;

            await _meetingRepository.AddAsync(meeting);

            await _unitOfWork.SaveChangesAsync();

            await _activityLogService.AddAsync(
                ActivityAction.Create,
                nameof(Meeting),
                meeting.Id,
                $"Meeting '{meeting.Title}' created.",
                _currentUserService.UserId);

            if (meeting.AssignedUserId.HasValue)
            {
                await _notificationService.AddAsync(
                    meeting.AssignedUserId.Value,
                    "New Meeting Scheduled",
                    $"Meeting '{meeting.Title}' has been scheduled for you.",
                    NotificationType.Reminder,
                    nameof(Meeting),
                    meeting.Id);
            }

            await _realtimeService.DashboardUpdatedAsync();

            return Result.Success(
                "Meeting created successfully.");
        }

        public async Task<Result> UpdateAsync(UpdateMeetingDto dto)
        {
            await ValidateAndThrowAsync(_updateValidator, dto);

            var meeting =
                await _businessRules.GetMeetingByIdAsync(
                    dto.Id,
                    true);
            _businessRules.EnsureUserCanModify(meeting);
            _businessRules.EnsureMeetingIsEditable(
                meeting);

            await _businessRules.EnsureCustomerExistsAsync(
                dto.CustomerId);

            await _businessRules.EnsureAssignedUserExistsAsync(
                dto.AssignedUserId);

            await _businessRules.EnsureNoMeetingConflictForUpdateAsync(
                dto.Id,
                dto.AssignedUserId,
                dto.StartDate,
                dto.EndDate);

            if (meeting.Status != dto.Status)
            {
                _businessRules.EnsureStatusTransition(
                    meeting.Status,
                    dto.Status);
            }

            int? previousAssignedUserId =
                meeting.AssignedUserId;

            dto.Adapt(meeting);

            _meetingRepository.Update(meeting);

            await _activityLogService.AddAsync(
                ActivityAction.Update,
                nameof(Meeting),
                meeting.Id,
                $"Meeting '{meeting.Title}' updated.",
                _currentUserService.UserId);

            await _unitOfWork.SaveChangesAsync();

            if (
                meeting.AssignedUserId.HasValue &&
                meeting.AssignedUserId != previousAssignedUserId
            )
            {
                await _notificationService.AddAsync(
                    meeting.AssignedUserId.Value,
                    "Meeting Assigned",
                    $"Meeting '{meeting.Title}' has been assigned to you.",
                    NotificationType.Reminder,
                    nameof(Meeting),
                    meeting.Id);
            }

            await _realtimeService.DashboardUpdatedAsync();

            return Result.Success(
                "Meeting updated successfully.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var meeting = await _businessRules.GetMeetingByIdAsync(id, true);
            _businessRules.EnsureUserCanModify(meeting);
            _meetingRepository.Delete(meeting);
            await _activityLogService.AddAsync(ActivityAction.Delete, nameof(Meeting),meeting.Id,$"Meeting '{meeting.Title}' deleted.", _currentUserService.UserId);
            await _unitOfWork.SaveChangesAsync();
            await _realtimeService.DashboardUpdatedAsync();
            return Result.Success("Meeting deleted successfully.");
        }

        public async Task<Result<PagedResult<ResultMeetingDto>>> GetAllAsync(PaginationRequest request)
        {
            IQueryable<Meeting> query = _meetingRepository.GetAll();

            if (!_currentUserService.IsInRole("Admin") &&
                !_currentUserService.IsInRole("SalesManager"))
            {
                query = query.Where(x =>
                    x.AssignedUserId == _currentUserService.UserId);
            }

            PagedResult<ResultMeetingDto> meetings = await query
                .ProjectToType<ResultMeetingDto>()
                .ToPagedResultAsync(request);

            return Result<PagedResult<ResultMeetingDto>>
                .Success(meetings);
        }

        public async Task<Result<GetByIdMeetingDto>> GetByIdAsync(int id)
        {
            Meeting? meeting = await _meetingRepository
                .GetAll()
                .Include(x => x.Customer)
                .Include(x => x.AssignedUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (meeting is null)
            {
                return Result<GetByIdMeetingDto>.Failure(
                    "Meeting not found.");
            }

            GetByIdMeetingDto dto = new()
            {
                Id = meeting.Id,
                Title = meeting.Title,
                Description = meeting.Description,
                StartDate = meeting.StartDate,
                EndDate = meeting.EndDate,
                Type = meeting.Type,
                Status = meeting.Status,
                Location = meeting.Location,

                CustomerId = meeting.CustomerId,

                CustomerName =
                    !string.IsNullOrWhiteSpace(
                        meeting.Customer.CompanyName)
                        ? meeting.Customer.CompanyName
                        : $"{meeting.Customer.ContactFirstName} {meeting.Customer.ContactLastName}",

                AssignedUserId =
                    meeting.AssignedUserId,

                AssignedUserName =
                    meeting.AssignedUser is null
                        ? null
                        : $"{meeting.AssignedUser.FirstName} {meeting.AssignedUser.LastName}"
            };

            return Result<GetByIdMeetingDto>.Success(dto);
        }
        public async Task<Result<bool>> CheckAvailabilityAsync(
    int assignedUserId,
    DateTime startDate,
    DateTime endDate,
    int? meetingId = null)
        {
            await _businessRules.EnsureAssignedUserExistsAsync(
                assignedUserId);
            _businessRules.EnsureCurrentUserCanAccess(assignedUserId);
            if (endDate <= startDate)
            {
                return Result<bool>.Failure(
                    "End date must be later than start date.");
            }

            var hasConflict =
                await _businessRules.HasMeetingConflictAsync(
                    assignedUserId,
                    startDate,
                    endDate,
                    meetingId);

            return Result<bool>.Success(
                !hasConflict);
        }
        private static async Task ValidateAndThrowAsync<TDto>(IValidator<TDto> validator, TDto dto)
        {
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}
