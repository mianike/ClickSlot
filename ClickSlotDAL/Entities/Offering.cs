using ClickSlotDAL.Entities.Base;

namespace ClickSlotDAL.Entities
{
    public class Offering : EntityBase<int>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public TimeSpan Duration { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}