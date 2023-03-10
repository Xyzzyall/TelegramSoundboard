using System.Security.Authentication;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramSoundboardBot.Frontend.Localization;
using TelegramSoundboardBot.Frontend.Requests;

namespace TelegramSoundboardBot.Frontend;

public sealed class TelegramBot : IHostedService
{
    private readonly TelegramBotClient _botClient;

    private readonly CancellationTokenSource _botCts = new();
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TelegramBot(ILogger logger, IServiceScopeFactory serviceScopeFactory, IConfiguration config)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger.ForContext<TelegramBot>();
        var telegramToken = config["Telegram:BotToken"] ??
                            throw new InvalidCredentialException("Telegram bot token not specified");
        _botClient = new TelegramBotClient(telegramToken);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            },
            _botCts.Token
        );

        var botInfo = await _botClient.GetMeAsync(cancellationToken);
        _logger.Information("Started bot {@User}", botInfo);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _botCts.Cancel();
        return Task.CompletedTask;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        _logger.Debug("Got update {@Update}", update);
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();

        var telegramContext = new TelegramContext
        {
            Client = botClient,
            Update = update
        };
        var locale = serviceScope.ServiceProvider.GetRequiredService<ILocalizationService>();
        locale.SetLocale(telegramContext.GetCulture());

        var mediatorCmd = RouteTelegramMessage(telegramContext);
        try
        {
            await mediator.Send(mediatorCmd, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error occured while executing request {@Request}", mediatorCmd);
#if DEBUG
            await telegramContext.SendTextMessageAsync(
                string.Format(Resources.TelegramBot.DEBUG_ErrorOccured, e.Message), cancellationToken);
#endif
        }
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    //todo better create some kind of service for routing, maybe do something with annotations?  
    private static BaseTelegramRequest RouteTelegramMessage(TelegramContext context)
    {
        return context.Update switch
        {
            {InlineQuery: not null} => new InlineQueryRequest {Context = context},
            {Message: not null} => context.Update.Message switch
            {
                {Audio: not null} => new AddSoundRequest {Context = context},
                {Text: not null} => ParseSimpleTextCommand(context),
                _ => new IncorrectTelegramRequest {Context = context}
            },
            _ => new IncorrectTelegramRequest {Context = context}
        };
    }

    private static BaseTelegramRequest ParseSimpleTextCommand(TelegramContext context)
    {
        var text = context.Update.Message!.Text!;
        if (text.StartsWith(Commands.CommandNames[Commands.Codes.Start]))
            return new InitialMessageRequest {Context = context};
        if (text.StartsWith(Commands.CommandNames[Commands.Codes.GetSound]))
            return new GetSoundRequest {Context = context};

        //fallback to get sound
        return new GetSoundRequest {Context = context, IsPlainText = true};
    }
}