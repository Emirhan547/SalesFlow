using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.ML.Models
{
    public class LeadScoringInput
    {
        [LoadColumn(0)]
        public float Source { get; set; }

        [LoadColumn(1)]
        public float Status { get; set; }

        [LoadColumn(2)]
        public bool HasEmail { get; set; }

        [LoadColumn(3)]
        public bool HasPhone { get; set; }

        [LoadColumn(4)]
        public bool HasWebsite { get; set; }

        [LoadColumn(5)]
        public bool HasCompany { get; set; }

        [LoadColumn(6)]
        public bool HasAddress { get; set; }

        [LoadColumn(7)]
        public bool IsAssigned { get; set; }

        [LoadColumn(8)]
        public float DescriptionLength { get; set; }

        [LoadColumn(9)]
        [ColumnName("Label")]
        public bool Converted { get; set; }
    }
}
