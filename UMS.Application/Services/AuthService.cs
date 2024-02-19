using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Dtos;
using UMS.Core.Interfaces;
using UMS.Core.Models;

namespace UMS.Application.Services
{
	public class AuthService : IAuthService
	{
		private readonly IConfiguration _config;

		public AuthService(IConfiguration config)
		{
			_config = config;
		}

		public ResultDto<string> GenerateJwtToken(UserDto user)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var key = Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"]);
				_ = int.TryParse(Encoding.ASCII.GetBytes(_config["Jwt:TokenValidityInMinutes"]), out int tokenExpiresIn);

				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new Claim[]
					{
						new Claim(ClaimTypes.Name, user.Username),
						new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
						new Claim(ClaimTypes.Role, user.Role.ToString()),

					}),
					Expires = DateTime.UtcNow.AddMinutes(tokenExpiresIn),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
				};

				var token = tokenHandler.CreateToken(tokenDescriptor);
				return new ResultDto<string>()
				{
					Data = tokenHandler.WriteToken(token),
					Message = "Successful Operation",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch (Exception ex)
			{
				return new ResultDto<string>()
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}

		}

		public ResultDto<JwtSecurityToken> GenerateJwtToken(List<Claim> authClaims)
		{
			try
			{
				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
				_ = int.TryParse(Encoding.ASCII.GetBytes(_config["Jwt:TokenValidityInMinutes"]), out int tokenExpiresIn);

				var token = new JwtSecurityToken(
					issuer: _config["JWT:ValidIssuer"],
					audience: _config["JWT:ValidAudience"],
					expires: DateTime.Now.AddMinutes(tokenExpiresIn),
					claims: authClaims,
					signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
					);

				return new ResultDto<JwtSecurityToken>()
				{
					Data = token,
					Message = "Successful Operation",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch (Exception ex)
			{
				return new ResultDto<JwtSecurityToken>()
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}
	}
}
