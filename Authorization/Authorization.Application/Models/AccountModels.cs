using Authorization.Application.Models.Enums;
using IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Application.Models
{
    public record CredentialsModel(
        string Email,
        string Password
    );

    public record CreateAccountModel(
        string Email,
        string PhoneNumber,
        string Password
    );

    public record ExternalProviderFindModel(
        string Provider,
        Guid Id
    );

    public record AutoProvisionModel(
        string Provider,
        Guid Id,
        List<Claim> Claims
    );

    public record AccountModel(
        string Guid,
        string Email,
        bool IsEmailVerified,
        string PhoneNumber,
        string Password,
        RoleModel Role,
        string ProviderName,
        string ProviderSubjectId,
        bool IsActive,
        ICollection<Claim> Claims
    );
}
