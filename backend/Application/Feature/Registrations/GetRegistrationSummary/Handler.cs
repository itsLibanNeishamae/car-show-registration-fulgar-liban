using Application.Abstractions;
using Application.Common;

namespace Application.Feature.Registrations.GetRegistrationSummary;

public sealed class Handler(IRegistrationStore store)
{
    public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken) =>
        Result<Response>.Success(await store.GetSummaryAsync(cancellationToken));
}
