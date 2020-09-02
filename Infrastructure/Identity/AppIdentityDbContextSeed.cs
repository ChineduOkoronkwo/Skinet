using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any()) 
            {
                var user = new AppUser
                {
                    DisplayName = "Chinedum",
                    Email = "alfred.okoronkwo@gmail.com",
                    UserName = "alfred.okoronkwo@gmail.com",
                    Address = new Address 
                    {
                        FirstName = "Chinedum",
                        LastName = "Okoronkwo",
                        Street = "10 The street",
                        City = "Kitchener",
                        State = "Ontario",
                        Zipcode = "N2G2L2"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}