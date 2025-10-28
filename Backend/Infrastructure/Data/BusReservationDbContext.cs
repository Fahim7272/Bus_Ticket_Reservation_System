using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class BusReservationDbContext : DbContext
{
    public BusReservationDbContext(DbContextOptions<BusReservationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Bus> Buses { get; set; }
    public DbSet<Route> Routes { get; set; }
    public DbSet<BusSchedule> BusSchedules { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Bus
        modelBuilder.Entity<Bus>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.BusName).IsRequired().HasMaxLength(100);
        });

        // Route
        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FromCity).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ToCity).IsRequired().HasMaxLength(100);
        });

        // BusSchedule
        modelBuilder.Entity<BusSchedule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).HasPrecision(18, 2);

            entity.HasOne(e => e.Bus)
                .WithMany(b => b.BusSchedules)
                .HasForeignKey(e => e.BusId);

            entity.HasOne(e => e.Route)
                .WithMany(r => r.BusSchedules)
                .HasForeignKey(e => e.RouteId);
        });

        // Seat
        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SeatNumber).IsRequired().HasMaxLength(10);

            entity.HasOne(e => e.Bus)
                .WithMany(b => b.Seats)
                .HasForeignKey(e => e.BusId);
        });

        // Passenger
        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.MobileNumber).IsRequired().HasMaxLength(20);
        });

        // Ticket
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.BookingReference).IsRequired().HasMaxLength(50);

            entity.HasOne(e => e.BusSchedule)
                .WithMany(bs => bs.Tickets)
                .HasForeignKey(e => e.BusScheduleId);

            entity.HasOne(e => e.Seat)
                .WithMany(s => s.Tickets)
                .HasForeignKey(e => e.SeatId);

            entity.HasOne(e => e.Passenger)
                .WithMany(p => p.Tickets)
                .HasForeignKey(e => e.PassengerId);
        });
    }
}
