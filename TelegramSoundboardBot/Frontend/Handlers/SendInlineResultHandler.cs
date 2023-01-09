using MediatR;
using TelegramSoundboardBot.Frontend.Requests;
using TelegramSoundboardBot.Soundboard;

namespace TelegramSoundboardBot.Frontend.Handlers;

public class SendInlineResultHandler : IRequestHandler<SendInlineResultRequest>
{
    private readonly ISoundsService _sounds;

    public async Task<Unit> Handle(SendInlineResultRequest request, CancellationToken cancellationToken)
    {
        var resultId = request.Context.Update.ChosenInlineResult!.ResultId;
        var messageId = request.Context.Update.ChosenInlineResult!.InlineMessageId!;

        return default;
    }
}