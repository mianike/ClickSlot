using ClickSlotModel.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClickSlotWebAPI.Models.Request
{
    public class UpdateBookingRequest
    {
        [Required]
        public int MasterId { get; set; }
        
        [Required]
        public int OfferingId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

        [Required]
        public BookingStatus Status { get; set; }
    }
}
