using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.DTOs;
using Store.APIs.Errors;
using Store.APIs.Extensions;
using Store.Core.Entities.Identity;
using Store.Core.Services.Contracts;
using System.Security.Claims;

namespace Store.APIs.Controllers
{
	public class AccountController : BaseApiController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IMapper _mapper;
		private readonly ITokenService _tokenService;
		public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, IMapper mapper)
		{
			_userManager = userManager;
			_tokenService = tokenService;
			_signInManager = signInManager;
			_mapper = mapper;
		}

		[HttpPost("Register")]
		public async Task<ActionResult<TokenDto>> Register(RegisterDto register)
		{
			if (await CheckEmailValidation(register.Email))

			{
				return BadRequest(new ApiResponse(400, "This Email Is Already Exists"));
			}

			var User = new AppUser()
			{
				UserName = register.Username,
				DisplayName = register.Username,
				Email = register.Email,
			};

			var result = await _userManager.CreateAsync(User, register.Password);

			if (!result.Succeeded) { return BadRequest(new ApiResponse(400)); }

			var UserToken = _tokenService.GenerateToken(User);

			return Ok(new TokenDto() { Token = UserToken, Expiration = DateTime.Now.AddHours(1) });
		}

		[HttpPost("Login")]
		public async Task<ActionResult<TokenDto>> Login(LoginDto login)
		{
			var isEmail = login.Username.Contains('@');

			var user = isEmail ? await _userManager.FindByEmailAsync(login.Username) : await _userManager.FindByNameAsync(login.Username);

			if (user == null) return Unauthorized(new ApiResponse(401, "No Account With This UserName or Email"));

			var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, isPersistent: false, lockoutOnFailure: false);

			if (!result.Succeeded) return Unauthorized(new ApiResponse(401, "Wrong Username/Email or Password"));

			var UserToken = _tokenService.GenerateToken(user);

			return Ok(new TokenDto() { Token = UserToken, Expiration = DateTime.Now.AddHours(1) });
		}

		[Authorize]
		[HttpGet("GetCurrentUser")]
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);

			var user = await _userManager.FindByEmailAsync(email);

			var ReturnedUser = new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = _tokenService.GenerateToken(user)
			};

			return Ok(ReturnedUser);
		}


		[Authorize]
		[HttpGet("Address")]
		public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
		{
			var user = await _userManager.FindUserWithAddressAsync(User);

			var MappedAddress = _mapper.Map<AddressDto>(user.Address);

			return (MappedAddress);
		}

		[Authorize]
		[HttpPut("Address")]
		public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto addressDto)
		{
			var user = await _userManager.FindUserWithAddressAsync(User);

			var Address = _mapper.Map<Address>(addressDto);

			Address.Id = user.Address.Id;

			user.Address = Address;

			var result = await _userManager.UpdateAsync(user);

			if (!result.Succeeded) return BadRequest(new ApiResponse(400));

			return Ok(addressDto);
		}

		[HttpGet("CheckEmail")]
		public async Task<bool> CheckEmailValidation(string email)
		{
			return await _userManager.FindByEmailAsync(email) is not null;
		}
	}
}

