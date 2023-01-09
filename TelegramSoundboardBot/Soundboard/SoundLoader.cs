using Microsoft.Extensions.Options;

namespace TelegramSoundboardBot.Soundboard;

public interface ISoundLoader
{
    Stream GetSoundStream(string fileLocation);
    Task<string> ConvertSoundToOggAsync(string soundFile, CancellationToken ct);
    FileStream CreateTempFile(string fileName, out string newFilePath);
}

public class SoundLoader : ISoundLoader
{
    private readonly IFFmpegConverter _ffmpegConverter;
    private readonly SoundboardConfigs _soundboardConfigs;

    public SoundLoader(IOptions<SoundboardConfigs> soundboardConfigs, IFFmpegConverter ffmpegConverter)
    {
        _ffmpegConverter = ffmpegConverter;
        _soundboardConfigs = soundboardConfigs.Value;
        Directory.CreateDirectory(_soundboardConfigs.SoundsTempFolder);
        Directory.CreateDirectory(_soundboardConfigs.SoundsStorageFolder);
    }

    public Stream GetSoundStream(string fileLocation)
    {
        return File.OpenRead(fileLocation);
    }

    public async Task<string> ConvertSoundToOggAsync(string soundFile, CancellationToken ct)
    {
        var newSound = Path.Combine(_soundboardConfigs.SoundsStorageFolder, $"{Guid.NewGuid()}.ogg");
        await _ffmpegConverter.ConvertToOggOpus(soundFile, newSound, ct);
        return newSound;
    }

    public FileStream CreateTempFile(string fileName, out string newFilePath)
    {
        newFilePath = Path.Combine(_soundboardConfigs.SoundsTempFolder,
            $"{Guid.NewGuid()}.{Path.GetExtension(fileName)}");
        return File.OpenWrite(newFilePath);
    }
}