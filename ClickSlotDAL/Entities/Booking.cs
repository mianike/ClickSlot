using ClickSlotDAL.Entities.Base;
using ClickSlotModel.Enums;

namespace ClickSlotDAL.Entities
{
    public class Booking : EntityBase<int>
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int MasterId { get; set; }
        public Master Master { get; set; }
        public int ServiceId { get; set; }
        public Offering Service { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public BookingStatus Status { get; set; }
    }
}