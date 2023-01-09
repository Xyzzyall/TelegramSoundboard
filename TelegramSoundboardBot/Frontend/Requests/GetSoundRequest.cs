namespace TelegramSoundboardBot.Frontend.Requests;

public class GetSoundRequest : BaseTelegramRequest
{
    public bool IsPlainText { get; init; } = false;
}