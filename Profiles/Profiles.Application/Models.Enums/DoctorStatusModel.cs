using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Models.Enums
{
    public enum DoctorStatusModel
    {
        None = 0,
        AtWork,
        OnVacation,
        SickDay,
        SickLeave,
        LeaveWithoutPay,
        Inactive
    }
}
