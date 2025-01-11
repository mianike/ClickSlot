using ClickSlotDAL.Entities.Base;
using ClickSlotModel.Enums;

namespace ClickSlotDAL.Entities
{
    public class Booking : EntityBase<int>
    {
        public int ClientId { get; set; }
        public AppUser Client { get; set; }

        public int MasterId { get; set; }
        public AppUser Master { get; set; }

        public int OfferingId { get; set; }
        public Offering Offering { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public BookingStatus Status { get; set; }
    }
}