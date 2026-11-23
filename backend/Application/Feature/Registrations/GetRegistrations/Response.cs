using Application.Feature.Registrations;

namespace Application.Feature.Registrations.GetRegistrations;

public sealed record Response(
    IReadOnlyList<RegistrationResponse> Items,
    int Page,
    int PageSize,
    long TotalCount,
    int TotalPages);
