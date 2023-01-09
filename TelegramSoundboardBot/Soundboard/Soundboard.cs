using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TelegramSoundboardBot.Soundboard.Database;

namespace TelegramSoundboardBot.Soundboard;

public static class Soundboard
{
    public static IServiceCollection AddSoundboard(this IServiceCollection services)
    {
        services.AddSingleton<IFFmpegConverter, FFmpegConverter>();
        services.AddSingleton<ISoundLoader, SoundLoader>();
        services.AddScoped<ISoundsService, SoundsService>();

        return services;
    }

    public static void ConfigureSoundboard(HostBuilderContext context, IServiceCollection services)
    {
        services.Configure<SoundboardConfigs>(context.Configuration.GetSection(nameof(SoundboardConfigs)));

        //todo only in development
        services.AddDbContext<SoundboardContext>(options =>
        {
            options.UseSqlite(context.Configuration.GetConnectionString("SoundboardDbContext"));
        });
    }
}