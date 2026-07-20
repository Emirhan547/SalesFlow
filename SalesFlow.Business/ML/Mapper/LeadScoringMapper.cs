using SalesFlow.Business.ML.Models;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.ML.Mapper
{
    public static class LeadScoringMapper
    {
        public static LeadScoringInput Map(Lead lead)
        {
            return new LeadScoringInput
            {
                Source = (float)lead.Source,
                Status = (float)lead.Status,

                HasEmail = !string.IsNullOrWhiteSpace(lead.Email),

                HasPhone = !string.IsNullOrWhiteSpace(lead.PhoneNumber),

                HasWebsite = !string.IsNullOrWhiteSpace(lead.Website),

                HasCompany = !string.IsNullOrWhiteSpace(lead.CompanyName),

                HasAddress = !string.IsNullOrWhiteSpace(lead.Address),

                IsAssigned = lead.AssignedUserId.HasValue,

                DescriptionLength = lead.Description?.Length ?? 0
            };
        }
    }
}
