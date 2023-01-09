using FluentValidation;
using MediatR;
using TelegramSoundboardBot.Frontend.Requests;

namespace TelegramSoundboardBot.Frontend.Pipelines;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();
        var context = new ValidationContext<TRequest>(request);
        var validationResults =
            await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Count == 0) return await next();
        if (request is BaseTelegramRequest tgRequest)
            await tgRequest.Context.SendTextMessageAsync(
                string.Join("\n", failures.Select(f => f.ErrorMessage)), cancellationToken
            );
        return default;
    }
}