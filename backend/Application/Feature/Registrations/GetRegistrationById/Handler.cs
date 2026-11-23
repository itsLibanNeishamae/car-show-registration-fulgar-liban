using Application.Abstractions;
using Application.Common;
using Application.Feature.Registrations;

namespace Application.Feature.Registrations.GetRegistrationById;

public sealed class Handler(IRegistrationStore store)
{
    public async Task<Result<RegistrationResponse>> Handle(Query query, CancellationToken cancellationToken)
    {
        var registration = await store.GetByIdAsync(query.Id, cancellationToken);
        return registration is null
            ? Result<RegistrationResponse>.NotFound("Registration.NotFound", "Registration was not found.")
            : Result<RegistrationResponse>.Success(RegistrationResponse.FromEntity(registration));
    }
}
