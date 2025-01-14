using System.ComponentModel.DataAnnotations;

namespace ClickSlotWebAPI.Validation
{
    public class AtLeastNowDateTimeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime dateTime)
            {

                return dateTime > DateTime.Now;
            }

            return false;
        }
    }
}