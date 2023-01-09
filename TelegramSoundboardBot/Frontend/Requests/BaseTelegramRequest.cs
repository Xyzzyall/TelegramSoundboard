using MediatR;

namespace TelegramSoundboardBot.Frontend.Requests;

public abstract class BaseTelegramRequest : IRequest
{
    public TelegramContext Context { get; init; } = default!;
}