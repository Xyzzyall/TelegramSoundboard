using MediatR;
using Serilog;
using Telegram.Bot;
using TelegramSoundboardBot.Frontend.Localization;
using TelegramSoundboardBot.Frontend.Requests;
using TelegramSoundboardBot.Soundboard;

namespace TelegramSoundboardBot.Frontend.Handlers;

internal class AddSoundHandler : IRequestHandler<AddSoundRequest>
{
    private readonly ILocalizationService _localization;
    private readonly ISoundLoader _soundLoader;
    private readonly ISoundsService _soundsService;
    private readonly IMediator _mediator;

    public AddSoundHandler(ISoundLoader soundLoader, ISoundsService soundsService,
        ILocalizationService localization, IMediator mediator)
    {
        _soundLoader = soundLoader;
        _soundsService = soundsService;
        _localization = localization;
        _mediator = mediator;
    }

    //todo it looks bad. consider not saving audio files to disk, just passing URL to ffmpeg 
    public async Task<Unit> Handle(AddSoundRequest request, CancellationToken cancellationToken)
    {
        var audioId = request.Context.Update.Message!.Audio!.FileId;
        var audioName = request.Context.Update.Message!.Audio!.FileName!;
        string tempPath;
        await using (var stream = _soundLoader.CreateTempFile(audioName, out tempPath))
        {
            await request.Context.Client.GetInfoAndDownloadFileAsync(
                audioId,
                stream,
                cancellationToken
            );
        }
        
        var newOgg = await _soundLoader.ConvertSoundToOggAsync(tempPath, cancellationToken);

        File.Delete(tempPath);

        var soundName = request.Context.Update.Message!.Caption!;
        await _soundsService.CreateSound(soundName, newOgg, cancellationToken,
            request.Context.Update.Message.Audio.Duration);

        await request.Context.SendTextMessageAsync(_localization.Localize("SoundSuccessfullyAdded", soundName),
            cancellationToken);
        
        await _mediator.Send(new GetSoundRequest
        {
            Context = request.Context,
            NameOverride = soundName
        }, cancellationToken);
        
        return default;
    }
}