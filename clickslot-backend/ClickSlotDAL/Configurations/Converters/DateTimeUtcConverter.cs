using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace ClickSlotDAL.Configurations.Converters
{
    public class DateTimeUtcConverter : ValueConverter<DateTime, DateTime>
    {
        public DateTimeUtcConverter() : base(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
        {
        }
    }
}