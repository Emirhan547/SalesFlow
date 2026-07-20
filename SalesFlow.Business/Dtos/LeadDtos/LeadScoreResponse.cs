using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.LeadDtos
{
    public class LeadScoreResponse
    {
        public double Score { get; set; }

        public string Category { get; set; } = string.Empty;

        public string Recommendation { get; set; } = string.Empty;
    }
}
