using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.LeadRepositories;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.LeadServices
{
    public class LeadBusinessRules
    {
        private readonly ILeadRepository _leadRepository;

        public LeadBusinessRules(ILeadRepository leadRepository)
        {
            _leadRepository = leadRepository;
        }

        public async Task EnsureEmailIsUniqueAsync(string email)
        {
            bool exists = await _leadRepository.AnyAsync(x => x.Email == email);

            if (exists)
                throw new BusinessException("A lead with this email already exists.");
        }

        public async Task EnsureEmailIsUniqueForUpdateAsync(int leadId, string email)
        {
            bool exists = await _leadRepository.AnyAsync(x => x.Email == email && x.Id != leadId);

            if (exists)
                throw new BusinessException("A lead with this email already exists.");
        }

        public async Task<Lead> GetLeadByIdAsync(int id, bool tracking = false)
        {
            Lead? lead = await _leadRepository.GetByIdAsync(id, tracking);

            if (lead is null)
                throw new NotFoundException("Lead not found.");

            return lead;
        }
    }
}
