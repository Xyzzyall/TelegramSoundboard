namespace TelegramSoundboardBot.Frontend.Requests;

public class IncorrectTelegramRequest : BaseTelegramRequest
{
    public string? Message { get; init; }
}