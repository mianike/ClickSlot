﻿namespace ClickSlotModel.DTOs
{
    public class ScheduleDTO
    {
        public int Id { get; set; }
        public int MasterId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateOnly Date { get; set; }

    }
}