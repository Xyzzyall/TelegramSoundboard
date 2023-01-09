namespace TelegramSoundboardBot.Soundboard;

public class SoundboardConfigs
{
    public string SoundsStorageFolder { get; init; } = Path.Combine(Directory.GetCurrentDirectory(), ".sounds");
    public string SoundsTempFolder { get; init; } = Path.Combine(Directory.GetCurrentDirectory(), ".temp");
}