using ClickSlotDAL.Entities.Base;

namespace ClickSlotDAL.Entities
{
    public class Schedule : EntityBase<int>
    {
        public int MasterId { get; set; }
        public AppUser Master { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateOnly Date { get; set; }
    }
}