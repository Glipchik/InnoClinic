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
    public record RegisterAccountModel(
        string Email,
        string PhoneNumber,
        string Password
    );

    public record LoginAccountModel(
        string Email,
        string Password
    );
}
