using ClickSlotModel.Enums;

namespace ClickSlotModel.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public AppUserDTO Client { get; set; }
        public int MasterId { get; set; }
        public int OfferingId { get; set; }
        public OfferingDTO Offering { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
