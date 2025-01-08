using ClickSlotDAL.Entities.Base;

namespace ClickSlotDAL.Entities
{
    public class Master : AppUser
    {
        public ICollection<Offering> Services { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}