using ClickSlotDAL.Entities.Base;

namespace ClickSlotDAL.Entities
{
    public class Schedule : EntityBase<int>
    {
        public int MasterId { get; set; }
        public Master Master { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateOnly Date { get; set; }
    }
}