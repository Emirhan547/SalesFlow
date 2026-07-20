using SalesFlow.Business.Dtos.LeadDtos;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.ML.Services
{
    public interface ILeadScoringService
    {
        Task<LeadScoreResponse> PredictAsync(Lead lead);
    }
}
