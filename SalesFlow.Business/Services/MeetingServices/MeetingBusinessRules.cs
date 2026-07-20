using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.MeetingRepositories;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;

namespace SalesFlow.Business.Services.MeetingServices
{
    public class MeetingBusinessRules
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly CustomerBusinessRules _customerBusinessRules;
        private readonly AuthBusinessRules _authorizationBusinessRules;

        public MeetingBusinessRules(
      IMeetingRepository meetingRepository,
      UserManager<AppUser> userManager,
      CustomerBusinessRules customerBusinessRules,
      AuthBusinessRules authorizationBusinessRules)
        {
            _meetingRepository = meetingRepository;
            _userManager = userManager;
            _customerBusinessRules = customerBusinessRules;
            _authorizationBusinessRules = authorizationBusinessRules;
        }

        public async Task<Meeting> GetMeetingByIdAsync(int id, bool tracking = false)
        {
            var meeting = await _meetingRepository.GetByIdAsync(id, tracking);

            if (meeting is null)
                throw new NotFoundException("Meeting not found.");

            return meeting;
        }

        public async Task EnsureCustomerExistsAsync(int customerId)
        {
            await _customerBusinessRules.GetCustomerByIdAsync(customerId);
        }

        public async Task EnsureAssignedUserExistsAsync(int? userId)
        {
            if (!userId.HasValue)
                return;

            var user = await _userManager.FindByIdAsync(userId.Value.ToString());

            if (user is null)
                throw new BusinessException("Assigned user not found.");
        }

        public async Task<bool> HasMeetingConflictAsync(
    int? assignedUserId,
    DateTime startDate,
    DateTime endDate,
    int? excludedMeetingId = null)
        {
            if (!assignedUserId.HasValue)
                return false;

            var query = _meetingRepository
                  .GetAll()
               .Where(x =>
                    (!excludedMeetingId.HasValue ||
                     x.Id != excludedMeetingId.Value) &&
                    x.AssignedUserId == assignedUserId &&
                    x.Status == MeetingStatus.Scheduled &&
                    startDate < x.EndDate &&
                    endDate > x.StartDate);
            if (query.Provider is IAsyncQueryProvider)
                return await query.AnyAsync();

            return query.Any();
        }

        public async Task EnsureNoMeetingConflictAsync(
            int? assignedUserId,
            DateTime startDate,
            DateTime endDate)
        {
            var hasConflict =
                await HasMeetingConflictAsync(
                    assignedUserId,
                    startDate,
                    endDate);

            if (hasConflict)
                throw new BusinessException(
                    "The assigned user already has another meeting during this time.");
        }

        public async Task EnsureNoMeetingConflictForUpdateAsync(
            int meetingId,
            int? assignedUserId,
            DateTime startDate,
            DateTime endDate)
        {
            var hasConflict =
                await HasMeetingConflictAsync(
                    assignedUserId,
                    startDate,
                    endDate,
                    meetingId);

            if (hasConflict)
                throw new BusinessException(
                    "The assigned user already has another meeting during this time.");
        }


        public void EnsureMeetingIsEditable(Meeting meeting)
        {
            if (meeting.Status is MeetingStatus.Completed
                or MeetingStatus.Cancelled)
            {
                throw new BusinessException(
                    "Completed or cancelled meetings cannot be updated.");
            }
        }
        public void EnsureStatusTransition(MeetingStatus current, MeetingStatus next)
        {
            var valid = current switch
            {
                MeetingStatus.Scheduled =>
                    next is MeetingStatus.Completed or MeetingStatus.Cancelled,

                MeetingStatus.Completed => false,

                MeetingStatus.Cancelled => false,

                _ => false
            };

            if (!valid)
                throw new BusinessException("Invalid meeting status transition.");
        }
        public void EnsureUserCanModify(Meeting meeting)
        {
            _authorizationBusinessRules
                .EnsureCurrentUserCanAccess(meeting.AssignedUserId, "You are not authorized to access this record.");
        }
        public void EnsureCurrentUserCanAccess(int? userId)
        {
            _authorizationBusinessRules
              .EnsureCurrentUserCanAccess(userId, "You are not authorized to access this record.");
        }
    }
}
