using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.CustomerDtos
{
    public class GenerateFollowUpEmailDto
    {
        public string Tone { get; set; } = "Professional";

        public string Purpose { get; set; } = "Follow Up";
    }
}
