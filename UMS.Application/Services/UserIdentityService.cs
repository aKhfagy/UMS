using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UMS.Core.Interfaces;
using UMS.Core.Dtos;
using Azure;
using Microsoft.AspNetCore.Http;
using UMS.Core.Utils;
using System.Net;
using UMS.Core.Models;
using System.Security.Cryptography;

namespace UMS.Application.Services
{
	public class UserIdentityService : IUserIdentityService
	{
		private readonly UserManager<AuthenticationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IAuthService _authService;
		private readonly IConfiguration _configuration;

		public UserIdentityService(
			UserManager<AuthenticationUser> userManager, 
			RoleManager<IdentityRole> roleManager, 
			IAuthService authService,
			IConfiguration configuration)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_authService = authService;
			_configuration = configuration;
		}

		public async Task<ResultDto<bool>> RegisterUser(RegisterIdentityDto input)
		{
			try
			{
				var existingUserUserName = await _userManager.FindByNameAsync(input.Username);
				if(existingUserUserName != null)
				{
					throw new Exception("Username Already exists");
				}

				AuthenticationUser user = new()
				{
					Email = input.Email,
					SecurityStamp = Guid.NewGuid().ToString(),
					UserName = input.Username
				};
				var result = await _userManager.CreateAsync(user, input.Password);
				if (!result.Succeeded)
				{
					throw new Exception("Failed to create user");
				}

				if (!await _roleManager.RoleExistsAsync(UserRoles.Viewer))
					await _roleManager.CreateAsync(new IdentityRole(UserRoles.Viewer));

				if (await _roleManager.RoleExistsAsync(UserRoles.Viewer))
					await _userManager.AddToRoleAsync(user, UserRoles.Viewer);

				return new ResultDto<bool>()
				{
					Data = true,
					Message = "Success",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch (Exception ex)
			{
				return new ResultDto<bool>()
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}

		public async Task<ResultDto<bool>> RegisterAdmin(RegisterIdentityDto input)
		{
			try
			{
				var existingUserUserName = await _userManager.FindByNameAsync(input.Username);
				if (existingUserUserName != null)
				{
					throw new Exception("Username Already exists");
				}

				AuthenticationUser user = new()
				{
					Email = input.Email,
					SecurityStamp = Guid.NewGuid().ToString(),
					UserName = input.Username
				};
				var result = await _userManager.CreateAsync(user, input.Password);
				if (!result.Succeeded)
				{
					throw new Exception("Failed to create user");
				}

				if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
					await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

				if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
					await _userManager.AddToRoleAsync(user, UserRoles.Admin);

				return new ResultDto<bool>()
				{
					Data = true,
					Message = "Success",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch (Exception ex)
			{
				return new ResultDto<bool>()
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}

		public async Task<ResultDto<TokenDto>> Authenticate(string username, string password)
		{
			try
			{
				var user = await _userManager.FindByNameAsync(username);
				bool flag = user != null && await _userManager.CheckPasswordAsync(user, password);
				if(!flag)
				{
					throw new Exception("Username or password is incorrect.");
				}

				var userRoles = await _userManager.GetRolesAsync(user);

				var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.UserName ?? ""),
					new Claim(ClaimTypes.Email, user.Email ?? ""),
					new Claim(ClaimTypes.NameIdentifier, user.Id),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				};

				foreach (var userRole in userRoles)
				{
					authClaims.Add(new Claim(ClaimTypes.Role, userRole));
				}

				var token = _authService.GenerateJwtToken(authClaims);
				var refreshToken = GenerateRefreshToken();

				_ = int.TryParse(Encoding.ASCII.GetBytes(_configuration["Jwt:RefreshTokenValidityInDays"]), out int refreshTokenValidityInDays);

				user.RefreshToken = refreshToken;
				user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

				await _userManager.UpdateAsync(user);


				if (token.StatusCode != HttpStatusCode.OK || token.Data == null)
				{
					throw new Exception("Failed to generate token.");
				}

				return new ResultDto<TokenDto>()
				{
					Data =new TokenDto() { 
						AccessToken = new JwtSecurityTokenHandler().WriteToken(token.Data),
						RefreshToken = refreshToken,
					},
					Message = "Success",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch(Exception ex) {
				return new ResultDto<TokenDto>()
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError
				};
			}
		}

		private static string GenerateRefreshToken()
		{
			var randomNumber = new byte[64];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}

		private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"])),
				ValidateLifetime = false
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
			if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
				throw new SecurityTokenException("Invalid token");

			return principal;

		}

		public async Task<ResultDto<TokenDto>> RefreshToken(TokenDto token)
		{
			try
			{
				var principal = GetPrincipalFromExpiredToken(token.AccessToken);

				if(principal == null)
				{
					throw new Exception("Invalid token");
				}

				string username = principal.Identity?.Name ?? "";

				var user = await _userManager.FindByNameAsync(username);
				if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
				{
					throw new Exception("Invalid token");
				}

				var newAccessToken = _authService.GenerateJwtToken(principal.Claims.ToList());
				var newRefreshToken = GenerateRefreshToken();

				if(newAccessToken.StatusCode != HttpStatusCode.OK || newAccessToken.Data == null)
				{
					throw new Exception(newAccessToken.Message);
				}

				user.RefreshToken = newRefreshToken;
				await _userManager.UpdateAsync(user);

				return new ResultDto<TokenDto>()
				{
					Data = new TokenDto() { 
						AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken.Data),
						RefreshToken = newRefreshToken,
					},
					Message = "Success",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch (Exception ex)
			{
				return new ResultDto<TokenDto>()
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}

		public async Task<ResultDto<bool>> Revoke(string username)
		{
			try
			{
				var user = await _userManager.FindByNameAsync(username);
				if (user == null) 
					throw new Exception("Invalid username");

				user.RefreshToken = null;
				await _userManager.UpdateAsync(user);
				return new ResultDto<bool>
				{
					Data = true,
					Message = "Success",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch(Exception ex)
			{
				return new ResultDto<bool>
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}


		}

		public async Task<ResultDto<bool>> RevokeAll()
		{
			try
			{
				var users = _userManager.Users.ToList();
				foreach (var user in users)
				{
					user.RefreshToken = null;
					await _userManager.UpdateAsync(user);
				}

				return new ResultDto<bool>
				{
					Data = true,
					Message = "Success",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch (Exception ex)
			{
				return new ResultDto<bool>
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}
	}
}
