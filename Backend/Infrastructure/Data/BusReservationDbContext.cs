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

        // BusSchedule
        modelBuilder.Entity<BusSchedule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).HasPrecision(18, 2);

            // JourneyDate 
            entity.Property(e => e.JourneyDate)
                .HasColumnType("timestamp without time zone");

            entity.HasOne(e => e.Bus)
                .WithMany(b => b.BusSchedules)
                .HasForeignKey(e => e.BusId);

            entity.HasOne(e => e.Route)
                .WithMany(r => r.BusSchedules)
                .HasForeignKey(e => e.RouteId);
        });

        // BookingDate
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.BookingReference).IsRequired().HasMaxLength(50);

            entity.Property(e => e.BookingDate)
                .HasColumnType("timestamp without time zone");

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
