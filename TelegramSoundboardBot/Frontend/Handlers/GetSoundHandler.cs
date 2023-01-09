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

    public async Task<Unit> Handle(GetSoundRequest request, CancellationToken cancellationToken)
    {
        var text = request.Context.Update.Message!.Text!;
        var name = request.IsPlainText ? text : text[Commands.CommandNames[Commands.Codes.GetSound].Length..];
        var sound = await _soundsService.TryGetSoundByNameAsync(name, cancellationToken);

        if (sound is null)
        {
            await request.Context.SendTextMessageAsync(
                _localizationService.Localize("SoundIsNotFound", name),
                cancellationToken
            );
            return default;
        }

        //todo how it will perform when there are multiple requests for one sound?
        //todo consider using telegram caching with saving fileid (issues with tg bot client lib) 
        await using var soundStream = _soundLoader.GetSoundStream(sound.OggFilePath);

        await request.Context.SendVoiceMessageAsync(soundStream, sound.Duration, cancellationToken);

        return default;
    }
}