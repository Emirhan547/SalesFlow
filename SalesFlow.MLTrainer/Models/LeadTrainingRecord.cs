using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.MLTrainer.Models
{
    public class LeadTrainingRecord
    {
        public int Source { get; set; }

        public int Status { get; set; }

        public bool HasEmail { get; set; }

        public bool HasPhone { get; set; }

        public bool HasWebsite { get; set; }

        public bool HasCompany { get; set; }

        public bool HasAddress { get; set; }

        public bool IsAssigned { get; set; }

        public int DescriptionLength { get; set; }

        public bool Converted { get; set; }
    }
}
