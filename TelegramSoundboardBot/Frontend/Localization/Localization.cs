using Microsoft.Extensions.DependencyInjection;

namespace TelegramSoundboardBot.Frontend.Localization;

public static class Localization
{
    public static IServiceCollection AddCustomLocalization(this IServiceCollection services)
    {
        services.AddScoped<ILocalizationService, LocalizationService>();
        return services;
    }
}