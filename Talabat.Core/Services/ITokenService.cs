﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Services
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser User, UserManager<AppUser> userManager);

    }
}
