using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Core.Entities.Identity;
using Store.Core.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;

		public TokenService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GenerateToken(AppUser user)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

			var Creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


			var UserClaims = new List<Claim>()
			{
				new Claim(JwtRegisteredClaimNames.Sub,user.UserName),

				new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),

				new Claim(JwtRegisteredClaimNames.Email,user.Email),

				new Claim("uid", user.Id)
			};

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: UserClaims,
				expires: DateTime.Now.AddHours(1),
				signingCredentials: Creds
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
