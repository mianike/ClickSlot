using ClickSlotDAL.Entities;
using ClickSlotModel.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ClickSlotDAL.Configurations.Converters;

namespace ClickSlotDAL.Contexts
{
    public partial class ClickSlotDbContext : IdentityDbContext
    {
        public ClickSlotDbContext(DbContextOptions<ClickSlotDbContext> options) : base(options)
        {
        }

        //add convert to snake case
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Client)
                .WithMany(c => c.ClientBookings)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Master)
                .WithMany(m => m.MasterBookings)
                .HasForeignKey(b => b.MasterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Offering)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.OfferingId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Client)
                .WithMany(c => c.ClientReviews)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Master)
                .WithMany(m => m.MasterReviews)
                .HasForeignKey(r => r.MasterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Master)
                .WithMany(m => m.Schedules)
                .HasForeignKey(s => s.MasterId);

            modelBuilder.Entity<AppUser>()
                .HasOne(c => c.IdentityUser)
                .WithMany()
                .HasForeignKey(c => c.IdentityUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Offering>()
                .HasOne(o => o.Master)
                .WithMany(m => m.Offerings)
                .HasForeignKey(o => o.MasterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .Property(b => b.Status)
                .HasDefaultValue(BookingStatus.Pending);

            modelBuilder.Entity<Schedule>()
                .Property(s => s.Date)
                .HasConversion(new DateOnlyConverter());

            modelBuilder.Entity<Booking>()
                .Property(s => s.StartTime)
                .HasConversion(new DateTimeUtcConverter());

            modelBuilder.Entity<Booking>()
                .Property(s => s.EndTime)
                .HasConversion(new DateTimeUtcConverter());

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}