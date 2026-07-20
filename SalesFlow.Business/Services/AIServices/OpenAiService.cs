using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace SalesFlow.Business.Services.AIServices;

public sealed class OpenAiService : IOpenAiService
{
    private readonly ChatClient _client;

    public OpenAiService(IOptions<OpenAiOptions> options)
    {
        var opt = options.Value;

        _client = new ChatClient(
            model: opt.Model,
            apiKey: opt.ApiKey);
    }

    public async Task<string> GenerateAsync(
        string systemPrompt,
        string userPrompt,
        CancellationToken cancellationToken = default)
    {
        ChatCompletion completion =
            await _client.CompleteChatAsync(
            [
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(userPrompt)
            ],
            cancellationToken: cancellationToken);

        return completion.Content[0].Text;
    }
}