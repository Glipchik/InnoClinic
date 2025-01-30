﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.Application.Models
{
    public record DoctorModel(Guid Id, string FirstName, string LastName, string? MiddleName, Guid OfficeId, string Status);
}
