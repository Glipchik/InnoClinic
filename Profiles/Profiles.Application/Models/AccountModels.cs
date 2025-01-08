using Profiles.Application.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Models
{
    public record CreateAccountModel(
        string Email,
        string PhoneNumber,
        string PhotoFileName,
        bool IsEmailVerified,
        string Password);

    public record CreateAccountAuthorizationServerModel(
        string Email,
        string PhoneNumber,
        bool IsEmailVerified,
        string Password,
        RoleModel Role);

    public record AccountModel(
        Guid Id,
        string Email,
        string PhoneNumber,
        bool IsEmailVerified,
        DateTime CreatedAt,
        Guid CreatedBy,
        DateTime UpdatedAt,
        Guid UpdatedBy);
}
