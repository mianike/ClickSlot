using System.ComponentModel.DataAnnotations;

namespace ClickSlotWebAPI.Models.Request
{
    public class ReviewRequest
    {
        [Required]
        public int MasterId { get; set; }

        [Required]
        [Range(1,5)]
        public int Rating { get; set; }

        [Required]
        [Length(5,200)]
        public string Comment { get; set; }
    }
}
