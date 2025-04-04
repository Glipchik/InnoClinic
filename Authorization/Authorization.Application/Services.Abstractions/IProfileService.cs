﻿using Authorization.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Application.Services.Abstractions
{
    public interface IProfileService
    {
        Task CreatePatientProfile(CreatePatientModel createPatientModel, CreateAccountForProfilesApiModel createAccountForProfilesApiModel, CancellationToken cancellationToken);
    }
}