using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Store.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Identity
{
	public static class AppIdentityDbContextSeeding	
	{
		public static async Task SeedUserAsync(UserManager<AppUser> userManager, ILogger logger)
		{
			if (!userManager.Users.Any())
			{
				logger.LogInformation("No users found. Starting seeding process...");

				var user = new AppUser
				{
					DisplayName = "Shefo",
					UserName = "SherifAshraf",
					Email = "SherifAshrf6060@gmail.com"
				};

				var result = await userManager.CreateAsync(user, "@CmPunk98");

				if (result.Succeeded)
				{
					logger.LogInformation("User created successfully.");
				}
				else
				{
					logger.LogError("User creation failed.");

					foreach (var error in result.Errors)
					{
						logger.LogError("Error: {Error}", error.Description);
					}
				}
			}
			else
			{
				logger.LogInformation("Users already exist in the database.");
			}
		}
	}
}
