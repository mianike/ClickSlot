using System.ComponentModel.DataAnnotations;
using ClickSlotWebAPI.Validation;

namespace ClickSlotWebAPI.Models.Request
{
    public class ScheduleRequest
    {
        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [AtLeastTomorrowDate(ErrorMessage = "The date must be no earlier than tomorrow")]
        public DateOnly Date { get; set; }
    }
}