﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.Data.Enums
{
    public enum DoctorStatus
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
