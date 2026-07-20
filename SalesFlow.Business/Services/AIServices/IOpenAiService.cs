using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.AIServices
{
    public interface IOpenAiService
    {
        Task<string> GenerateAsync(
         string systemPrompt,
         string userPrompt,
         CancellationToken cancellationToken = default);
    }
}
