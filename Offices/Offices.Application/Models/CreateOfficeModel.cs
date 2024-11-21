﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.Application.Models
{
    public record CreateOfficeModel(string Address, string PhotoURL, string RegistryPhoneNumber, bool IsActive);
}