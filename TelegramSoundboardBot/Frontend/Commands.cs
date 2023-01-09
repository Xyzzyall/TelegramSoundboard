namespace TelegramSoundboardBot.Frontend;

public static class Commands
{
    public enum Codes
    {
        Start,
        GetSound
    }

    public static readonly IReadOnlyDictionary<Codes, string> CommandNames = new Dictionary<Codes, string>
    {
        [Codes.Start] = "/start",
        [Codes.GetSound] = "/sound"
    };
}