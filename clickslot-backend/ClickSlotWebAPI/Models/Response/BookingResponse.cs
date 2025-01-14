using ClickSlotModel.Enums;

namespace ClickSlotWebAPI.Models.Response
{
    public class BookingResponse
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int MasterId { get; set; }
        public int OfferingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public BookingStatus Status { get; set; }
    }
}
