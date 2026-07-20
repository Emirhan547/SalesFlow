using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.LeadRepositories;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.LeadServices
{
    public class LeadBusinessRules
    {
        private readonly ILeadRepository _leadRepository;
        private readonly AuthBusinessRules _authorizationBusinessRules;

        public LeadBusinessRules(ILeadRepository leadRepository, AuthBusinessRules authorizationBusinessRules)
        {
            _leadRepository = leadRepository;
            _authorizationBusinessRules = authorizationBusinessRules;
        }

        public async Task EnsureEmailIsUniqueAsync(string email)
        {
            var exists = await _leadRepository.AnyAsync(x => x.Email == email);

            if (exists)
                throw new BusinessException("A lead with this email already exists.");
        }

        public async Task EnsureEmailIsUniqueForUpdateAsync(int leadId, string email)
        {
            var exists = await _leadRepository.AnyAsync(x => x.Email == email && x.Id != leadId);

            if (exists)
                throw new BusinessException("A lead with this email already exists.");
        }

        public async Task<Lead> GetLeadByIdAsync(int id, bool tracking = false)
        {
            var lead = await _leadRepository.GetByIdAsync(id, tracking);

            if (lead is null)
                throw new NotFoundException("Lead not found.");

            return lead;
        }
        public void EnsureLeadCanBeConverted(Lead lead)
        {
            if (lead.Status == LeadStatus.Converted)
                throw new BusinessException("Lead has already been converted.");

            if (lead.Status != LeadStatus.Qualified)
                throw new BusinessException("Only qualified leads can be converted.");
        }
        public void EnsureUserCanModify(Lead lead, string? forbiddenMessage = null)
        {
            _authorizationBusinessRules
               .EnsureCurrentUserCanAccess(lead.AssignedUserId, forbiddenMessage);
        }
    }
}
