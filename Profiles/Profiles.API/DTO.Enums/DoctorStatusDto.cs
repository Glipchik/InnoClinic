using System;

namespace Profiles.API.DTO.Enums;

public enum DoctorStatusDto
{
    None = 0,
    AtWork,
    OnVacation,
    SickDay,
    SickLeave,
    LeaveWithoutPay,
    Inactive
}