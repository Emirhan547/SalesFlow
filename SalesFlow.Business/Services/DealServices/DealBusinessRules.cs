using Microsoft.AspNetCore.Identity;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.DealRepositories;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;

namespace SalesFlow.Business.Services.DealServices
{
    public class DealBusinessRules
    {
        private readonly IDealRepository _dealRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly CustomerBusinessRules _customerBusinessRules;
        public DealBusinessRules(IDealRepository dealRepository, ICustomerRepository customerRepository, UserManager<AppUser> userManager, CustomerBusinessRules customerBusinessRules)
        {
            _dealRepository = dealRepository;
            _customerRepository = customerRepository;
            _userManager = userManager;
            _customerBusinessRules = customerBusinessRules;
        }

        public async Task<Deal> GetDealByIdAsync(int id, bool tracking = false)
        {
           var deal = await _dealRepository.GetByIdAsync(id, tracking);

            if (deal is null)
                throw new NotFoundException("Deal not found.");

            return deal;
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

        public async Task EnsureActiveDealTitleIsUniqueAsync(string title, int customerId)
        {
            var exists = await _dealRepository.AnyAsync(x =>
                x.CustomerId == customerId &&
                x.Title == title &&
                x.Stage != DealStage.Won &&
                x.Stage != DealStage.Lost);

            if (exists)
                throw new BusinessException("An active deal with the same title already exists for this customer.");
        }

        public async Task EnsureActiveDealTitleIsUniqueForUpdateAsync(int dealId, string title, int customerId)
        {
            var exists = await _dealRepository.AnyAsync(x =>
                x.Id != dealId &&
                x.CustomerId == customerId &&
                x.Title == title &&
                x.Stage != DealStage.Won &&
                x.Stage != DealStage.Lost);

            if (exists)
                throw new BusinessException("An active deal with the same title already exists for this customer.");
        }

        public void EnsureDealIsEditable(Deal deal)
        {
            if (deal.Stage is DealStage.Won or DealStage.Lost)
                throw new BusinessException("Completed deals cannot be updated.");
        }

        public void EnsureDealIsDeletable(Deal deal)
        {
            if (deal.Stage == DealStage.Won)
                throw new BusinessException("Won deals cannot be deleted.");
        }

       

        public void EnsureStageTransition(DealStage current, DealStage next)
        {
            var valid = current switch
            {
                DealStage.New =>
                    next is DealStage.Qualified or DealStage.Lost,

                DealStage.Qualified =>
                    next is DealStage.ProposalSent or DealStage.Lost,

                DealStage.ProposalSent =>
                    next is DealStage.Negotiation or DealStage.Lost,

                DealStage.Negotiation =>
                    next is DealStage.Won or DealStage.Lost,

                DealStage.Won => false,

                DealStage.Lost => false,

                _ => false
            };

            if (!valid)
                throw new BusinessException("Invalid deal stage transition.");
        }
    }
}