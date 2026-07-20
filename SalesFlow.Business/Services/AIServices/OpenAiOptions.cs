using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.AIServices
{
    public sealed class OpenAiOptions
    {
        public const string SectionName = "OpenAI";

        public string ApiKey { get; set; } = string.Empty;

        public string Model { get; set; } = "gpt-5.5";
    }
}
