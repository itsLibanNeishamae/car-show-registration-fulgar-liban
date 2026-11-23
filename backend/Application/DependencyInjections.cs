using Application.Feature.Registrations.CreateRegistration;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjections
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<Application.Feature.Registrations.CreateRegistration.Handler>();
        services.AddScoped<Application.Feature.Registrations.GetRegistrations.Handler>();
        services.AddScoped<Application.Feature.Registrations.GetRegistrationById.Handler>();
        services.AddScoped<Application.Feature.Registrations.GetRegistrationSummary.Handler>();
        services.AddValidatorsFromAssemblyContaining<Validator>();
        return services;
    }
}
