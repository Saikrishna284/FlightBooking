using FlightBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightBooking.Data;

public partial class FlightDbContext : DbContext
{
    public FlightDbContext()
    {
    }

    public FlightDbContext(DbContextOptions<FlightDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<CheckIn> CheckIns { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<Passenger> Passengers { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=INBLRVM26590142;Database=Skr_FlightBooking;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__73951ACDCA0042DC");

            entity.HasIndex(e => e.PaymentId, "UQ_Bookings_payments").IsUnique();

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.BookingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Class)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Economy");
            entity.Property(e => e.Fare).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.FlightId).HasColumnName("FlightID");
            entity.Property(e => e.NoOfTickets).HasDefaultValue(1);
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Flight).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.FlightId)
                .HasConstraintName("FK_Bookings_Flights");

            entity.HasOne(d => d.Payment).WithOne(p => p.Booking)
                .HasForeignKey<Booking>(d => d.PaymentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Bookings_Payments");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Bookings__UserID__5EBF139D");
        });

        modelBuilder.Entity<CheckIn>(entity =>
        {
            entity.HasKey(e => e.CheckInId).HasName("PK__CheckIns__E64976A4ED965332");

            entity.HasIndex(e => e.PassengerId, "UQ_Checkins_Passengers").IsUnique();

            entity.Property(e => e.CheckInId).HasColumnName("CheckInID");
            entity.Property(e => e.CheckInDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PassengerId).HasColumnName("PassengerID");

            entity.HasOne(d => d.Passenger).WithOne(p => p.CheckIn)
                .HasForeignKey<CheckIn>(d => d.PassengerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Checkins_Passengers");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.FlightId).HasName("PK__Flights__8A9E148E01F47685");

            entity.Property(e => e.FlightId).HasColumnName("FlightID");
            entity.Property(e => e.AirlineName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ArrivalDateTime).HasColumnType("datetime");
            entity.Property(e => e.AvailableSeats).HasDefaultValue(100);
            entity.Property(e => e.DepartureDateTime).HasColumnType("datetime");
            entity.Property(e => e.DestinationCity)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.FlightNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SourceCity)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.HasKey(e => e.PassengerId).HasName("PK__Passenge__88915FB02CE71AA5");

            entity.Property(e => e.Age).HasColumnName("AGE");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Booking).WithMany(p => p.Passengers)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Passenger__Booki__60A75C0F");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A58A97EA092");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.AmountPaid).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Paid");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC61DEA056");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534A87B54E3").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("User")
                .HasColumnName("ROLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
