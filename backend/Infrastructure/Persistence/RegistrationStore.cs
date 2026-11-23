using Application.Abstractions;
using Application.Common;
using Application.Feature.Registrations;
using Application.Feature.Registrations.CreateRegistration;
using Application.Feature.Registrations.GetRegistrations;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;

namespace Infrastructure.Persistence;

public sealed class RegistrationStore(AppDbContext db) : IRegistrationStore
{
    public async Task<Result<Registration>> CreateAsync(Request request, CancellationToken cancellationToken)
    {
        var plateNumber = request.PlateNumber.Trim().ToUpperInvariant();
        for (var attempt = 0; attempt < 5; attempt++)
        {
            try
            {
                await using var transaction = await db.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable, cancellationToken);

                if (await db.Registrations.AnyAsync(
                        registration => registration.PlateNumber == plateNumber, cancellationToken))
                    return Result<Registration>.Conflict(
                        "Registration.Conflict", $"Plate number {plateNumber} is already registered.");

                var sequence = await db.RegistrationSequences.SingleAsync(row => row.Id == 1, cancellationToken);
                sequence.LastNumber++;
                var registration = Registration.Create(
                    $"CS27-{sequence.LastNumber:D4}",
                    request.ParticipantName,
                    request.ContactNumber,
                    request.Email,
                    request.Address,
                    request.CarMakeModel,
                    plateNumber,
                    request.RegistrationType);
                if (request.AmountPaid > 0m)
                    registration.RecordPayment(request.AmountPaid);

                db.Registrations.Add(registration);
                await db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return Result<Registration>.Success(registration);
            }
            catch (SqliteException exception) when (exception.SqliteErrorCode is 5 or 6 && attempt < 4)
            {
                db.ChangeTracker.Clear();
                await Task.Delay(TimeSpan.FromMilliseconds(20 * (attempt + 1)), cancellationToken);
            }
            catch (DbUpdateException)
            {
                db.ChangeTracker.Clear();
                if (await db.Registrations.AnyAsync(
                        registration => registration.PlateNumber == plateNumber, cancellationToken))
                    return Result<Registration>.Conflict(
                        "Registration.Conflict", $"Plate number {plateNumber} is already registered.");
                throw;
            }
        }

        return Result<Registration>.Conflict(
            "Registration.Conflict", "Registration could not be created due to concurrent requests. Please retry.");
    }

    public async Task<Application.Feature.Registrations.GetRegistrations.Response> GetRegistrationsAsync(
        Query query,
        CancellationToken cancellationToken)
    {
        var page = int.Parse(query.Page!, CultureInfo.InvariantCulture);
        var pageSize = int.Parse(query.PageSize!, CultureInfo.InvariantCulture);
        var registrations = db.Registrations.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(query.ParticipantName))
        {
            var search = query.ParticipantName.Trim().ToLower();
            registrations = registrations.Where(registration =>
                registration.ParticipantName.ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(query.Type))
        {
            Enum.TryParse<RegistrationType>(query.Type, true, out var registrationType);
            registrations = registrations.Where(registration => registration.RegistrationType == registrationType);
        }

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            Enum.TryParse<RegistrationStatus>(query.Status, true, out var registrationStatus);
            registrations = registrations.Where(registration => registration.Status == registrationStatus);
        }

        var totalCount = await registrations.LongCountAsync(cancellationToken);
        var items = await registrations
            .OrderByDescending(registration => registration.RegistrationDate)
            .ThenByDescending(registration => registration.Id)
            .Skip((int)Math.Min(((long)page - 1) * pageSize, int.MaxValue))
            .Take(pageSize)
            .Select(registration => new RegistrationResponse(
                registration.Id,
                registration.RegistrationNumber,
                DateTime.SpecifyKind(registration.RegistrationDate, DateTimeKind.Utc),
                registration.ParticipantName,
                registration.ContactNumber,
                registration.Email,
                registration.Address,
                registration.CarMakeModel,
                registration.PlateNumber,
                registration.RegistrationType,
                registration.Status,
                registration.RegistrationFee,
                registration.AmountPaid,
                registration.AmountPaid == 0
                    ? PaymentStatus.Unpaid
                    : registration.AmountPaid == registration.RegistrationFee
                        ? PaymentStatus.Paid
                        : PaymentStatus.Partial))
            .ToListAsync(cancellationToken);

        return new Application.Feature.Registrations.GetRegistrations.Response(
            items, page, pageSize, totalCount,
            (int)Math.Ceiling(totalCount / (double)pageSize));
    }

    public Task<Registration?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Registrations.AsNoTracking().SingleOrDefaultAsync(item => item.Id == id, cancellationToken);

    public async Task<Application.Feature.Registrations.GetRegistrationSummary.Response> GetSummaryAsync(
        CancellationToken cancellationToken)
    {
        var counts = await db.Registrations
            .GroupBy(_ => 1)
            .Select(group => new
            {
                TotalRegistrations = group.LongCount(),
                Confirmed = group.LongCount(registration => registration.Status == RegistrationStatus.Confirmed),
                CheckedIn = group.LongCount(registration => registration.Status == RegistrationStatus.CheckedIn),
                Pending = group.LongCount(registration => registration.Status == RegistrationStatus.Pending),
                Cancelled = group.LongCount(registration => registration.Status == RegistrationStatus.Cancelled),
                TotalFees = group.Where(registration => registration.Status != RegistrationStatus.Cancelled)
                    .Sum(registration => (decimal?)registration.RegistrationFee) ?? 0m,
                TotalPayments = group.Where(registration => registration.Status != RegistrationStatus.Cancelled)
                    .Sum(registration => (decimal?)registration.AmountPaid) ?? 0m
            })
            .SingleOrDefaultAsync(cancellationToken);

        return counts is null
            ? new Application.Feature.Registrations.GetRegistrationSummary.Response(0, 0, 0, 0, 0, 0m, 0m, 0m)
            : new Application.Feature.Registrations.GetRegistrationSummary.Response(
                counts.TotalRegistrations,
                counts.Confirmed,
                counts.CheckedIn,
                counts.Pending,
                counts.Cancelled,
                counts.TotalFees,
                counts.TotalPayments,
                counts.TotalFees - counts.TotalPayments);
    }
}
