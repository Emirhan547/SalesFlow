using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly CustomerBusinessRules _customerBusinessRules;
        public MeetingBusinessRules(IMeetingRepository meetingRepository, ICustomerRepository customerRepository, UserManager<AppUser> userManager, CustomerBusinessRules customerBusinessRules)
        {
            _meetingRepository = meetingRepository;
            _customerRepository = customerRepository;
            _userManager = userManager;
            _customerBusinessRules = customerBusinessRules;
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
            await _customerBusinessRules.EnsureCustomerExistsAsync(customerId);
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

            return await _meetingRepository
                .GetAll()
                .AnyAsync(x =>
                    (!excludedMeetingId.HasValue ||
                     x.Id != excludedMeetingId.Value) &&
                    x.AssignedUserId == assignedUserId &&
                    x.Status == MeetingStatus.Scheduled &&
                    startDate < x.EndDate &&
                    endDate > x.StartDate);
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
    }
}
