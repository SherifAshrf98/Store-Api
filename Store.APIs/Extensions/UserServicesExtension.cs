using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities.Identity;
using System.Security.Claims;

namespace Store.APIs.Extensions
{
	public static class UserServicesExtension
	{

		public static async Task<AppUser> FindUserWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User)
		{
			var Email = User.FindFirstValue(ClaimTypes.Email);

			var user = await userManager.Users.Include(u => u.Address).SingleOrDefaultAsync(u => u.Email == Email);

			return user;
		}



	}
}
