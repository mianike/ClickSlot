using ClickSlotDAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace ClickSlotDAL.Entities.Base
{
    public abstract class AppUser : EntityBase<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
}