using System.ComponentModel.DataAnnotations;

namespace HealthCalendar.ViewModels
{
    // ViewModel for user login forms (used by both Patient and Worker)
    public class LoginViewModel
    {
        // The user's email address (required, must be a valid email)
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        // The user's password (required, input type is password)
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
