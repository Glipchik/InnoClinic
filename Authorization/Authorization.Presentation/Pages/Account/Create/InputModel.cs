// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace Authorization.Presentation.Pages.Create
{
    public class InputModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateTime? DateOfBirth { get; set; } = null;
        public string? Email { get; set; }
        public string? Password { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ReturnUrl { get; set; }
        public string? Button { get; set; }
    }
}