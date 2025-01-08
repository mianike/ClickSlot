namespace ClickSlotModel.DTOs
{
    public class MasterDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public string IdentityUserId { get; set; }
        public ICollection<OfferingDTO> Services { get; set; }
        public ICollection<ScheduleDTO> Schedules { get; set; }
    }
}
