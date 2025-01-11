using ClickSlotModel.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClickSlotWebAPI.Models.Request
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public AppUserRole Role { get; set; }
    }
}