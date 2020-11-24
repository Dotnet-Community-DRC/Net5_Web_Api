using System.ComponentModel.DataAnnotations;

namespace NetCoreWeb.Shared.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string ConfirmPassword { get; set; }
    }
}