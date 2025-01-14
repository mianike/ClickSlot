using ClickSlotModel.Enums;
using ClickSlotWebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace ClickSlotWebAPI.Models.Request
{
    public class NewBookingRequest
    {
        [Required]
        public int MasterId { get; set; }
        
        [Required]
        public int OfferingId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [AtLeastNowDateTime]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [AtLeastNowDateTime]
        public DateTime EndTime { get; set; }
    }
}
