using Application.Common;
using Application.Feature.Registrations.CreateRegistration;
using Application.Feature.Registrations.GetRegistrations;
using Domain.Entities;

namespace Application.Abstractions;

public interface IRegistrationStore
{
    Task<Result<Registration>> CreateAsync(Request request, CancellationToken cancellationToken);
    Task<Application.Feature.Registrations.GetRegistrations.Response> GetRegistrationsAsync(
        Query query, CancellationToken cancellationToken);
    Task<Registration?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Application.Feature.Registrations.GetRegistrationSummary.Response> GetSummaryAsync(
        CancellationToken cancellationToken);
}
