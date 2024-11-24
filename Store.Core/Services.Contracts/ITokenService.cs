﻿using Store.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services.Contracts
{
	public interface ITokenService
	{
		public string GenerateToken(AppUser user);
	}
}
