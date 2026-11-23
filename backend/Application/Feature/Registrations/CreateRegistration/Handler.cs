using Application.Abstractions;
using Application.Common;
using Application.Feature.Registrations;
using Domain.Entities;

namespace Application.Feature.Registrations.CreateRegistration;

public sealed class Handler(IRegistrationStore store)
{
    public async Task<Result<RegistrationResponse>> Handle(Command command, CancellationToken cancellationToken)
    {
        var result = await store.CreateAsync(command.Registration, cancellationToken);
        if (!result.IsSuccess)
            return Result<RegistrationResponse>.Failure(result.Error!);

        return Result<RegistrationResponse>.Success(RegistrationResponse.FromEntity(result.Value!));
    }
}
