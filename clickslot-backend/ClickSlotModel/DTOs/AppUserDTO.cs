using ClickSlotModel.Enums;

namespace ClickSlotModel.DTOs
{
    public class AppUserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public AppUserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public string IdentityUserId { get; set; }
        public ICollection<OfferingDTO> Offerings { get; set; }
        public IEnumerable<ScheduleDTO> Schedules { get; set; }
    }
}