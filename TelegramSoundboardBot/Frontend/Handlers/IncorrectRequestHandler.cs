using MediatR;
using TelegramSoundboardBot.Frontend.Requests;

namespace TelegramSoundboardBot.Frontend.Handlers;

public class IncorrectRequestHandler : IRequestHandler<IncorrectTelegramRequest>
{
    public Task<Unit> Handle(IncorrectTelegramRequest request, CancellationToken cancellationToken)
    {
        return Unit.Task;
    }
}