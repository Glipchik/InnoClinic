// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace Authorization.Presentation.Pages.Create
{
    public class InputModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public string? MiddleName { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; } = null;

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        public IFormFile? ProfilePicture { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }
        public string? ReturnUrl { get; set; }

        public string? Button { get; set; }
    }
}