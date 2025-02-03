using System.ComponentModel.DataAnnotations;

namespace Authorization.Presentation.Pages.Account.SendCode
{
    public class InputModel
    {
        [Required]
        public string? Email { get; set; }
    }
}
