using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace Talabat.APIs.Extentions
{
    public static class IdentityServicesExtention
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services)
        {
            Services.AddScoped<ITokenService, TokenService>();
            Services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>(); // Use your DbContext
            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer();
            return Services;
        } 
    }
}
