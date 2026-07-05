using Microsoft.AspNetCore.Identity;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.MeetingRepositories;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums.SalesFlow.Entity.Enums;
using Microsoft.EntityFrameworkCore;

namespace SalesFlow.Business.Services.MeetingServices
{
    public class MeetingBusinessRules
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<AppUser> _userManager;

        public MeetingBusinessRules(IMeetingRepository meetingRepository,ICustomerRepository customerRepository,UserManager<AppUser> userManager)
        {
            _meetingRepository = meetingRepository;
            _customerRepository = customerRepository;
            _userManager = userManager;
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
            var exists = await _customerRepository.AnyAsync(x => x.Id == customerId);

            if (!exists)
                throw new BusinessException("Customer not found.");
        }

        public async Task EnsureAssignedUserExistsAsync(int? userId)
        {
            if (!userId.HasValue)
                return;

            var user = await _userManager.FindByIdAsync(userId.Value.ToString());

            if (user is null)
                throw new BusinessException("Assigned user not found.");
        }

        public async Task EnsureNoMeetingConflictAsync( int? assignedUserId, DateTime startDate, DateTime endDate)
        {
            if (!assignedUserId.HasValue)
                return;

            var exists = await _meetingRepository
                .GetAll()
                .AnyAsync(x =>
                    x.AssignedUserId == assignedUserId &&
                    x.Status == MeetingStatus.Scheduled &&
                    startDate < x.EndDate &&
                    endDate > x.StartDate);

            if (exists)
                throw new BusinessException("The assigned user already has another meeting during this time.");
        }

        public async Task EnsureNoMeetingConflictForUpdateAsync(int meetingId,int? assignedUserId, DateTime startDate,DateTime endDate)
        {
            if (!assignedUserId.HasValue)
                return;

            var exists = await _meetingRepository.GetAll().AnyAsync(x =>
                    x.Id != meetingId &&
                    x.AssignedUserId == assignedUserId &&
                    x.Status == MeetingStatus.Scheduled &&
                    startDate < x.EndDate &&
                    endDate > x.StartDate);

            if (exists)
                throw new BusinessException("The assigned user already has another meeting during this time.");
        }    

        public void EnsureStatusChanged(MeetingStatus current, MeetingStatus next)
        {
            if (current == next)
                throw new BusinessException("Meeting is already in this status.");
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
