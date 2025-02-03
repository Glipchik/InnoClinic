using System.ComponentModel.DataAnnotations;

namespace Authorization.Presentation.Pages.Account.ResetPassword
{
    public class InputModel
    {
        [Required]
        public string? EmailToken { get; set; }

        [Required]
        public string? NewPassword { get; set; }
    }
}
