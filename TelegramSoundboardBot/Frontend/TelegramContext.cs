using System.Globalization;
using JetBrains.Annotations;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace TelegramSoundboardBot.Frontend;

//todo maybe use DI? 
public class TelegramContext
{
    public ITelegramBotClient Client { get; init; } = default!;
    public Update Update { get; init; } = default!;


    [LocalizationRequired(true)]
    public async Task SendTextMessageAsync(string text, CancellationToken ct)
    {
        if (Update.Message is null) return;
        await Client.SendTextMessageAsync(Update.Message.Chat.Id, text, cancellationToken: ct);
    }

    public async Task SendCachedVoiceMessageAsync(string fileId, int duration, CancellationToken ct)
    {
        if (Update.Message is null) throw new NullReferenceException(nameof(Update.Message));
        await Client.SendVoiceAsync(Update.Message.Chat.Id, new InputOnlineFile(fileId), cancellationToken: ct,
            duration: duration);
    }
    
    public async Task<string> SendVoiceMessageAsync(Stream soundStream, int duration, CancellationToken ct)
    {
        if (Update.Message is null) throw new NullReferenceException(nameof(Update.Message));
        var message = await Client.SendVoiceAsync(Update.Message.Chat.Id, soundStream!, cancellationToken: ct, duration: duration);
        return message.Voice!.FileId;
    }

    public CultureInfo GetCulture()
    {
        return Update switch
        {
            {Message.From.LanguageCode: not null} => CultureInfo.GetCultureInfo(Update.Message.From.LanguageCode),
            {InlineQuery.From.LanguageCode: not null} => CultureInfo.GetCultureInfo(
                Update.InlineQuery.From.LanguageCode),
            _ => CultureInfo.InvariantCulture
        };
    }
}