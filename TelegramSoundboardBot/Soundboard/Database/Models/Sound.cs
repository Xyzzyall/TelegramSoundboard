namespace TelegramSoundboardBot.Soundboard.Database.Models;

public class Sound
{
    public ulong Id { get; set; }
    public string Name { get; set; } = default!;
    public string OggFilePath { get; set; } = default!; //todo consider using relative path 
    public int Duration { get; set; } = -1;

    public string? TelegramFileId { get; set; }
    public DateTime? CachedByTelegramOn { get; set; }
    
}