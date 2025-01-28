using Profiles.Application.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Models
{
    public class CreateAccountModel
    {
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PhotoFileName { get; set; }
        public bool IsEmailVerified { get; set; }
        public RoleModel Role { get; set; }
        public Guid AuthorId { get; set; }
    }

    public class CreateAccountFromAuthServerModel
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PhotoFileName { get; set; }
        public bool IsEmailVerified { get; set; }
        public Guid AuthorId { get; set; }
    }

    public class CreateAccountAuthorizationServerModel
    {
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public int RoleId { get; set; }
    }

    public class AuthorizationAccountModel
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PhotoFileName { get; set; }
        public bool IsEmailVerified { get; set; }
        public RoleModel Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
    }

    public class AccountModel
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PhotoFileName { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
