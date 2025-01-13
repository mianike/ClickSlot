using System.ComponentModel.DataAnnotations;

namespace ClickSlotWebAPI.Validation
{
    public class TomorrowDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateOnly date)
            {
                var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
                return date >= currentDate.AddDays(1);
            }

            return false;
        }
    }
}
