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
        RoleModel Role);

    public record CreateAccountAuthorizationServerModel(
        string Email,
        string PhoneNumber,
        bool IsEmailVerified,
        RoleModel Role);

    public record AccountModel(
        Guid Id,
        string Email,
        string PhoneNumber,
        bool IsEmailVerified,
        RoleModel Role,
        DateTime CreatedAt,
        Guid CreatedBy,
        DateTime UpdatedAt,
        Guid UpdatedBy);
}
