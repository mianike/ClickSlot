namespace ClickSlotWebAPI.Models.Response
{
    public class ScheduleResponse
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateOnly Date { get; set; }
    }
}