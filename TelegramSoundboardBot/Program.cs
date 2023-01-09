using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TelegramSoundboardBot.Frontend;
using TelegramSoundboardBot.Frontend.Localization;
using TelegramSoundboardBot.Frontend.Pipelines;
using TelegramSoundboardBot.Soundboard;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddEnvironmentVariables();
        if (context.HostingEnvironment.IsDevelopment()) builder.AddUserSecrets(typeof(TelegramBot).Assembly);
    })
    .ConfigureServices((context, services) =>
    {
        Soundboard.ConfigureSoundboard(context, services);
        services.AddSoundboard();

        services.AddCustomLocalization();

        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddPipelines();

        services.AddHostedService<TelegramBot>();
    })
    .UseSerilog((hostingContext, services, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .MinimumLevel.Debug()
        .Enrich.FromLogContext()
        .WriteTo.Console())
    .Build();

await host.RunAsync();