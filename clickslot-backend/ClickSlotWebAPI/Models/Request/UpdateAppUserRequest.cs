using System.ComponentModel.DataAnnotations;

namespace ClickSlotWebAPI.Models.Request
{
    public class UpdateAppUserRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }
    }
}