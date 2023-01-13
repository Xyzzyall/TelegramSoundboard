using Microsoft.EntityFrameworkCore;
using Serilog;
using TelegramSoundboardBot.Soundboard.Database;
using TelegramSoundboardBot.Soundboard.Database.Models;

namespace TelegramSoundboardBot.Soundboard;

public class SoundsService : ISoundsService
{
    private readonly SoundboardContext _soundboardContext;

    public SoundsService(SoundboardContext soundboardContext, ILogger logger)
    {
        _soundboardContext = soundboardContext;
        logger.ForContext<SoundsService>();
    }

    public async Task CreateSound(string name, string filePath, CancellationToken ct, int duration = -1)
    {
        var sound = new Sound {Name = name, Duration = duration, OggFilePath = filePath};
        _soundboardContext.Sounds.Add(sound);
        await _soundboardContext.SaveChangesAsync(ct);
    }

    public async Task SaveTelegramFileId(Sound sound, string fileId)
    {
        sound.TelegramFileId = fileId;
        sound.CachedByTelegramOn = DateTime.Now;
        await _soundboardContext.SaveChangesAsync();
    }

    public async Task<bool> SoundNameExistAsync(string name, CancellationToken ct)
    {
        return await _soundboardContext.Sounds.AnyAsync(s => s.Name == name, ct);
    }

    public async Task<Sound?> TryGetSoundByNameAsync(string name, CancellationToken ct)
    {
        return await _soundboardContext.Sounds.Where(s => s.Name == name)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<Sound>> GetSoundsWithPartialNameAsync(string name, CancellationToken ct, int take = -1,
        int skip = -1)
    {
        var query = _soundboardContext.Sounds.Where(s => s.Name.StartsWith(name));
        if (take > 0) query = query.Take(take);

        if (skip > 0) query = query.Skip(skip);
        return await query.ToListAsync(ct);
    }
}

public interface ISoundsService
{
    Task CreateSound(string name, string filePath, CancellationToken ct, int duration = -1);
    Task SaveTelegramFileId(Sound sound, string fileId);
    Task<bool> SoundNameExistAsync(string name, CancellationToken ct);
    Task<Sound?> TryGetSoundByNameAsync(string name, CancellationToken ct);
    Task<List<Sound>> GetSoundsWithPartialNameAsync(string name, CancellationToken ct, int take = -1, int skip = -1);
}