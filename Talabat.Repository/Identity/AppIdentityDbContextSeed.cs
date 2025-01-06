using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public async static Task SeedidentityAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any()){
                var User = new AppUser
                {
                    DisplayName = "Amr Shamy",
                    Email = "amrshamy91@gmail.com",
                    UserName = "amrshamy91",
                    PhoneNumber = "01234567891"

                };
                await userManager.CreateAsync(User,"Pa$$w0rd");
            }
            
            
        }
    }
}
