using MediatR;
using Serilog;
using TelegramSoundboardBot.Frontend.Localization;
using TelegramSoundboardBot.Frontend.Requests;
using TelegramSoundboardBot.Soundboard;

namespace TelegramSoundboardBot.Frontend.Handlers;

public class GetSoundHandler : IRequestHandler<GetSoundRequest>
{
    private readonly ILocalizationService _localizationService;
    private readonly ILogger _logger;
    private readonly ISoundLoader _soundLoader;
    private readonly ISoundsService _soundsService;

    public GetSoundHandler(ILogger logger, ISoundsService soundsService, ILocalizationService localizationService,
        ISoundLoader soundLoader)
    {
        _soundsService = soundsService;
        _localizationService = localizationService;
        _soundLoader = soundLoader;
        _logger = logger.ForContext<GetSoundHandler>();
    }

    //todo how it will perform when there are multiple requests for one sound?
    public async Task<Unit> Handle(GetSoundRequest request, CancellationToken cancellationToken)
    {
        var text = request.Context.Update.Message!.Text!;
        var name = request.NameOverride ?? (request.IsPlainText ? text : text[Commands.CommandNames[Commands.Codes.GetSound].Length..]);
        var sound = await _soundsService.TryGetSoundByNameAsync(name, cancellationToken);

        if (sound is null)
        {
            await request.Context.SendTextMessageAsync(
                _localizationService.Localize("SoundIsNotFound", name),
                cancellationToken
            );
            return default;
        }

        if (sound.TelegramFileId is null)
        {
            await using var soundStream = _soundLoader.GetSoundStream(sound.OggFilePath);
            var fileId = await request.Context.SendVoiceMessageAsync(soundStream, sound.Duration, cancellationToken);
            await _soundsService.SaveTelegramFileId(sound, fileId);
            return default;
        }

        await request.Context.SendCachedVoiceMessageAsync(sound.TelegramFileId, sound.Duration, cancellationToken);
        return default;
    }
}