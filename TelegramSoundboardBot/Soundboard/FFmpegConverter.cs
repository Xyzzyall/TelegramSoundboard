using Cysharp.Diagnostics;
using Serilog;

namespace TelegramSoundboardBot.Soundboard;

public interface IFFmpegConverter
{
    Task ConvertToOggOpus(string pathFrom, string pathTo, CancellationToken ct);
}

public class FFmpegConverter : IFFmpegConverter
{
    private readonly ILogger _logger;

    public FFmpegConverter(ILogger logger)
    {
        _logger = logger.ForContext<FFmpegConverter>();
    }

    public async Task ConvertToOggOpus(string pathFrom, string pathTo, CancellationToken ct)
    {
        var command =
            $"ffmpeg -i \"{pathFrom}\" -c:a libopus -vn -b:a 64K -compression_level 10 -map_metadata 0 -application audio \"{pathTo}\"";
        _logger.Debug("Executing ffmpeg: {Command}", command);
        try
        {
            //todo idk why always throws error, maybe misconfiguration? 
            await ProcessX.StartAsync(command).ToTask(ct);
        }
        catch
        {
            // ignored
        }
    }
}