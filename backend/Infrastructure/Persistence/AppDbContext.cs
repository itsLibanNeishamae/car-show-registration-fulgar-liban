using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Registration> Registrations => Set<Registration>();
    public DbSet<RegistrationSequence> RegistrationSequences => Set<RegistrationSequence>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RegistrationsConfigurations());
        modelBuilder.Entity<RegistrationSequence>(entity =>
        {
            entity.ToTable("RegistrationSequences");
            entity.HasKey(sequence => sequence.Id);
            entity.Property(sequence => sequence.LastNumber).IsRequired();
            entity.HasData(new RegistrationSequence { Id = 1, LastNumber = 0 });
        });
        base.OnModelCreating(modelBuilder);
    }
}
