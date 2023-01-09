using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types.InlineQueryResults;
using TelegramSoundboardBot.Frontend.Requests;
using TelegramSoundboardBot.Soundboard;

namespace TelegramSoundboardBot.Frontend.Handlers;

public class InlineQueryHandler : IRequestHandler<InlineQueryRequest>
{
    private readonly ISoundsService _sounds;

    public InlineQueryHandler(ISoundsService sounds)
    {
        _sounds = sounds;
    }

    //todo not working
    public async Task<Unit> Handle(InlineQueryRequest request, CancellationToken cancellationToken)
    {
        var inlineQueryId = request.Context.Update.InlineQuery!.Id;
        var queryStr = request.Context.Update.InlineQuery!.Query;
        if (queryStr.Length <= 3) return default;

        var sounds = await _sounds.GetSoundsWithPartialNameAsync(queryStr, cancellationToken, 50);

        await request.Context.Client.AnswerInlineQueryAsync(
            inlineQueryId,
            sounds.Select(s => new InlineQueryResultVoice(s.Name, "dummy", s.Name)
            {
                InputMessageContent = new InputTextMessageContent(s.Name)
            }),
            cancellationToken: cancellationToken
        );
        return default;
    }
}