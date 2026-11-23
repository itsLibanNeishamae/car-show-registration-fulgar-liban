using Api.Filters;
using Api;
using CreateRegistrationHandler = Application.Feature.Registrations.CreateRegistration.Handler;
using CreateRegistrationCommand = Application.Feature.Registrations.CreateRegistration.Command;
using CreateRegistrationRequest = Application.Feature.Registrations.CreateRegistration.Request;
using GetRegistrationByIdHandler = Application.Feature.Registrations.GetRegistrationById.Handler;
using GetRegistrationByIdQuery = Application.Feature.Registrations.GetRegistrationById.Query;
using GetRegistrationsHandler = Application.Feature.Registrations.GetRegistrations.Handler;
using GetRegistrationsQuery = Application.Feature.Registrations.GetRegistrations.Query;

namespace Api.Endpoints.Registrations;

public static class RegistrationEndpoints
{
    public static IEndpointRouteBuilder MapRegistrationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var registrations = endpoints.MapGroup("/api/registrations");

        registrations.MapPost("", async (
                CreateRegistrationRequest request,
                CreateRegistrationHandler handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new CreateRegistrationCommand(request), cancellationToken);
                return result.ToHttpResult(response =>
                    Results.CreatedAtRoute("GetRegistrationById", new { id = response.Id }, response));
            })
            .AddEndpointFilter<ValidationFilter<CreateRegistrationRequest>>()
            .WithName("CreateRegistration");

        registrations.MapGet("", async (
                [AsParameters] GetRegistrationsQuery query,
                GetRegistrationsHandler handler,
                CancellationToken cancellationToken) =>
                (await handler.Handle(query, cancellationToken)).ToHttpResult())
            .AddEndpointFilter<ValidationFilter<GetRegistrationsQuery>>();

        registrations.MapGet("/summary", async (
                Application.Feature.Registrations.GetRegistrationSummary.Handler handler,
                CancellationToken cancellationToken) =>
                (await handler.Handle(
                    new Application.Feature.Registrations.GetRegistrationSummary.Query(),
                    cancellationToken)).ToHttpResult());

        registrations.MapGet("/{id:guid}", async (
                Guid id,
                GetRegistrationByIdHandler handler,
                CancellationToken cancellationToken) =>
                (await handler.Handle(new GetRegistrationByIdQuery { Id = id }, cancellationToken)).ToHttpResult())
            .WithName("GetRegistrationById");

        return endpoints;
    }
}
