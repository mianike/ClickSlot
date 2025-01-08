using ClickSlotDAL.Entities;
using ClickSlotModel.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace ClickSlotDAL.Contexts
{
    public partial class ClickSlotDbContext : IdentityDbContext
    {
        public ClickSlotDbContext(DbContextOptions<ClickSlotDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Client)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Master)
                .WithMany(m => m.Bookings)
                .HasForeignKey(b => b.MasterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Service)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ServiceId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Client)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Master)
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MasterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Master)
                .WithMany(m => m.Schedules)
                .HasForeignKey(s => s.MasterId);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.IdentityUser)
                .WithMany()
                .HasForeignKey(c => c.IdentityUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Master>()
                .HasOne(m => m.IdentityUser)
                .WithMany()
                .HasForeignKey(m => m.IdentityUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Offering>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<Booking>()
                .Property(b => b.Status)
                .HasDefaultValue(BookingStatus.Pending);

            modelBuilder.Entity<Schedule>()
                .Property(s => s.Date)
                .HasConversion<DateOnlyConverter>();

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}