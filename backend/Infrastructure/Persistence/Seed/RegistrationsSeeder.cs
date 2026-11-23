using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Persistence.Seed;

public static class RegistrationsSeeder
{
    private sealed record SeedRegistration(
        string Name,
        string Phone,
        string Email,
        string Address,
        string Car,
        string Plate,
        RegistrationType Type,
        RegistrationStatus Status,
        int PaymentPercent);

    private static readonly SeedRegistration[] SeedData =
    [
        new("Juan Dela Cruz", "09171234001", "juan.delacruz@example.com", "123 Rizal St., Naga City", "Toyota Supra MK4", "ABC 1234", RegistrationType.Competitor, RegistrationStatus.Confirmed, 40),
        new("Maria Santos", "09171234002", "maria.santos@example.com", "Mabini Ave., Lipa City", "Honda Civic Type R", "NCR 2187", RegistrationType.Exhibitor, RegistrationStatus.Pending, 0),
        new("Miguel Reyes", "09171234003", "miguel.reyes@example.com", "Lahug, Cebu City", "Nissan Skyline GT-R R34", "CEB 9042", RegistrationType.VIP, RegistrationStatus.CheckedIn, 100),
        new("Angela Garcia", "09171234004", "angela.garcia@example.com", "San Fernando, Pampanga", "Mitsubishi Lancer Evolution IX", "PMP 3318", RegistrationType.Competitor, RegistrationStatus.Confirmed, 100),
        new("Carlo Mendoza", "09171234005", "carlo.mendoza@example.com", "Legazpi City, Albay", "Mazda RX-7 FD", "ALB 7620", RegistrationType.Exhibitor, RegistrationStatus.Cancelled, 0),
        new("Patricia Flores", "09171234006", "patricia.flores@example.com", "Iloilo City, Iloilo", "Subaru Impreza WRX STI", "ILO 4851", RegistrationType.VIP, RegistrationStatus.Pending, 50),
        new("Rafael Torres", "09171234007", "rafael.torres@example.com", "Davao City, Davao del Sur", "Toyota 86", "DAV 6204", RegistrationType.Competitor, RegistrationStatus.CheckedIn, 100),
        new("Camille Navarro", "09171234008", "camille.navarro@example.com", "Antipolo City, Rizal", "Honda S2000", "RZL 5179", RegistrationType.Exhibitor, RegistrationStatus.Confirmed, 0),
        new("Andres Bautista", "09171234009", "andres.bautista@example.com", "Baguio City, Benguet", "Ford Mustang GT", "BGT 8246", RegistrationType.VIP, RegistrationStatus.Confirmed, 100),
        new("Sofia Castillo", "09171234010", "sofia.castillo@example.com", "Cagayan de Oro City", "Volkswagen Golf GTI", "CDO 1935", RegistrationType.Exhibitor, RegistrationStatus.Pending, 50),
        new("Paolo Villanueva", "09171234011", "paolo.villanueva@example.com", "Quezon City, Metro Manila", "Toyota Corolla AE86", "QZN 4082", RegistrationType.Competitor, RegistrationStatus.Cancelled, 0),
        new("Isabella Ramos", "09171234012", "isabella.ramos@example.com", "Tagaytay City, Cavite", "Porsche 911 Carrera", "CVT 6713", RegistrationType.VIP, RegistrationStatus.CheckedIn, 100),
        new("Gabriel Lim", "09171234013", "gabriel.lim@example.com", "Binondo, Manila", "BMW M3 E46", "MNL 2258", RegistrationType.Exhibitor, RegistrationStatus.Confirmed, 50),
        new("Beatriz Aquino", "09171234014", "beatriz.aquino@example.com", "San Juan City, Metro Manila", "Mercedes-Benz 190E", "SJH 7641", RegistrationType.Competitor, RegistrationStatus.Pending, 0),
        new("Enzo Fernandez", "09171234015", "enzo.fernandez@example.com", "Bacolod City, Negros Occidental", "Nissan Silvia S15", "BCL 3109", RegistrationType.VIP, RegistrationStatus.Cancelled, 0),
        new("Lourdes Mercado", "09171234016", "lourdes.mercado@example.com", "Lucena City, Quezon", "Honda Integra Type R", "QZN 5827", RegistrationType.Exhibitor, RegistrationStatus.CheckedIn, 100),
        new("Nico Cabrera", "09171234017", "nico.cabrera@example.com", "Santa Rosa City, Laguna", "Mitsubishi Lancer Evo VI", "LAG 9472", RegistrationType.Competitor, RegistrationStatus.Confirmed, 50),
        new("Cheska Dominguez", "09171234018", "cheska.dominguez@example.com", "Puerto Princesa City, Palawan", "Mini Cooper S", "PLW 1356", RegistrationType.Exhibitor, RegistrationStatus.Pending, 0),
        new("Dante Soriano", "09171234019", "dante.soriano@example.com", "Tacloban City, Leyte", "Chevrolet Camaro SS", "LEY 8063", RegistrationType.VIP, RegistrationStatus.Confirmed, 100),
        new("Bianca Valdez", "09171234020", "bianca.valdez@example.com", "Makati City, Metro Manila", "Audi RS3", "MKT 4530", RegistrationType.Competitor, RegistrationStatus.CheckedIn, 100),
        new("Felix Pascual", "09171234021", "felix.pascual@example.com", "Tuguegarao City, Cagayan", "Toyota Land Cruiser FJ40", "CAG 7291", RegistrationType.Exhibitor, RegistrationStatus.Cancelled, 0),
        new("Jasmine Co", "09171234022", "jasmine.co@example.com", "Dumaguete City, Negros Oriental", "Mazda MX-5 Miata", "DGT 2648", RegistrationType.VIP, RegistrationStatus.Pending, 50),
        new("Noel Estrada", "09171234023", "noel.estrada@example.com", "General Santos City, South Cotabato", "Honda Civic EG6", "GSC 8914", RegistrationType.Competitor, RegistrationStatus.Confirmed, 100),
        new("Teresa Manalo", "09171234024", "teresa.manalo@example.com", "Marikina City, Metro Manila", "Toyota GR Yaris", "MRK 3765", RegistrationType.Exhibitor, RegistrationStatus.Pending, 0),
        new("Victor Salazar", "09171234025", "victor.salazar@example.com", "Cebu City, Cebu", "Nissan 370Z", "CEB 5408", RegistrationType.VIP, RegistrationStatus.CheckedIn, 50)
    ];

    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(
            IsolationLevel.Serializable, cancellationToken);

        var sequence = await db.RegistrationSequences.SingleAsync(row => row.Id == 1, cancellationToken);
        var existing = await db.Registrations
            .Select(registration => new { registration.RegistrationNumber, registration.PlateNumber })
            .ToListAsync(cancellationToken);
        var existingMaximum = existing
            .Where(item => item.RegistrationNumber.StartsWith("CS27-", StringComparison.Ordinal))
            .Select(item => long.TryParse(item.RegistrationNumber.AsSpan(5), out var parsed) ? parsed : 0)
            .DefaultIfEmpty(0)
            .Max();
        sequence.LastNumber = Math.Max(sequence.LastNumber, existingMaximum);

        var existingPlates = existing
            .Select(item => item.PlateNumber)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var item in SeedData.Where(item => !existingPlates.Contains(item.Plate)))
        {
            sequence.LastNumber++;
            var registration = Registration.Create(
                $"CS27-{sequence.LastNumber:D4}",
                item.Name,
                item.Phone,
                item.Email,
                item.Address,
                item.Car,
                item.Plate,
                item.Type);

            if (item.Status is RegistrationStatus.Confirmed or RegistrationStatus.CheckedIn)
                registration.Confirm();
            if (item.Status == RegistrationStatus.CheckedIn)
                registration.CheckIn();
            if (item.Status == RegistrationStatus.Cancelled)
                registration.Cancel();
            if (item.PaymentPercent > 0)
                registration.RecordPayment(registration.RegistrationFee * item.PaymentPercent / 100m);
            db.Registrations.Add(registration);
        }

        await db.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
}
