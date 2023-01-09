using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TelegramSoundboardBot.Frontend.Pipelines;

public static class Behaviours
{
    public static IServiceCollection AddPipelines(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(Behaviours).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        return services;
    }
}