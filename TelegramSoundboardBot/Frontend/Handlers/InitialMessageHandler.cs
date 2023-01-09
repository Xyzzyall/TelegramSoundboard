using MediatR;
using TelegramSoundboardBot.Frontend.Localization;
using TelegramSoundboardBot.Frontend.Requests;

namespace TelegramSoundboardBot.Frontend.Handlers;

public class InitialMessageHandler : IRequestHandler<InitialMessageRequest>
{
    private readonly ILocalizationService _localization;

    public InitialMessageHandler(ILocalizationService localization)
    {
        _localization = localization;
    }

    public async Task<Unit> Handle(InitialMessageRequest request, CancellationToken cancellationToken)
    {
        await request.Context.SendTextMessageAsync(_localization.Localize("BotGreeting_0"), cancellationToken);
        return default;
    }
}