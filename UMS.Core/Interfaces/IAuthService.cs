using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Dtos;
using UMS.Core.Models;

namespace UMS.Core.Interfaces
{
	public interface IAuthService
	{
		public ResultDto<string> GenerateJwtToken(UserDto user);
		public ResultDto<JwtSecurityToken> GenerateJwtToken(List<Claim> authClaims);
	}
}
