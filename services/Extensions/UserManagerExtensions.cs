using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace services.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindUserWithAddressByClaimsPrincipalAsync
            (this UserManager<AppUser> userMgr, ClaimsPrincipal user) 
        {
            var email = GetUserEmail(user);
            return await userMgr.Users.Include(x => x.Address)
                .SingleOrDefaultAsync(x => x.Email == email);           
        }

        public static string GetUserEmail(ClaimsPrincipal user)
        {
            return user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        }
    }
}