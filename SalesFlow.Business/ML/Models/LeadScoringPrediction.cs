using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.ML.Models
{
    public class LeadScoringPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool WillConvert { get; set; }

        public float Probability { get; set; }

        public float Score { get; set; }
    }
}
