using System.ComponentModel.DataAnnotations;

namespace ClickSlotWebAPI.Models.Request
{
    public class OfferingRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0.0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(typeof(TimeSpan), "00:01:00", "24:00:00")]
        public TimeSpan Duration { get; set; }
    }
}