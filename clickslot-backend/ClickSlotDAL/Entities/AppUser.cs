using ClickSlotDAL.Entities.Base;
using ClickSlotModel.Enums;
using Microsoft.AspNetCore.Identity;

namespace ClickSlotDAL.Entities
{
    public class AppUser : EntityBase<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public AppUserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }

        public ICollection<Booking> ClientBookings { get; set; }
        public ICollection<Booking> MasterBookings { get; set; }

        public ICollection<Review> ClientReviews { get; set; }
        public ICollection<Review> MasterReviews { get; set; }

        public ICollection<Offering> Offerings { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}