using Authorization.Application.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Application.Models
{
    public record CreateAccountModel(
        string Email,
        string PhoneNumber,
        bool IsEmailVerified,
        RolesModel Role,
        Guid CreatedBy,
        Guid UpdatedBy,
        DateTime CreatedAt,
        DateTime UpdatedAt);

    public record AccountModel(
        Guid Id,
        string Email,
        string PhoneNumber,
        bool IsEmailVerified,
        RolesModel Role,
        Guid CreatedBy,
        Guid UpdatedBy,
        DateTime CreatedAt,
        DateTime UpdatedAt);

    public record UpdateAccountModel(
        Guid Id,
        string Email,
        string PhoneNumber,
        bool IsEmailVerified,
        RolesModel Role,
        Guid CreatedBy,
        Guid UpdatedBy,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
