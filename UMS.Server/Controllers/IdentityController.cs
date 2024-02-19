using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using UMS.Core.Dtos;
using UMS.Core.Interfaces;
using UMS.Core.Utils;

namespace UMS.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class IdentityController : ControllerBase
	{
		private readonly IUserIdentityService _userIdentityService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public IdentityController(
			IUserIdentityService userIdentityService,
			IHttpContextAccessor httpContextAccessor)
		{
			_userIdentityService = userIdentityService;
			_httpContextAccessor = httpContextAccessor;
		}

		[HttpPost("RegisterViewer")]
		[AllowAnonymous]
		public async Task<IActionResult> RegisterUser(RegisterIdentityDto input)
		{
			try
			{
				var response = await _userIdentityService.RegisterUser(input);
				if(response.StatusCode != HttpStatusCode.OK)
				{
					return StatusCode(500, response.Message);
				}
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		
		[HttpPost("RegisterAdmin")]
		[AllowAnonymous]
		public async Task<IActionResult> RegisterAdmin(RegisterIdentityDto input)
		{
			try
			{
				var response = await _userIdentityService.RegisterAdmin(input);
				if(response.StatusCode != HttpStatusCode.OK)
				{
					return StatusCode(500, response.Message);
				}
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("Login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login(LoginDto input)
		{
			try
			{
				var response = await _userIdentityService.Authenticate(input.Username, input.Password);
				if (response.StatusCode != HttpStatusCode.OK || response.Data == null)
				{
					return Unauthorized();
				}
				return Ok(response.Data);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("IsAdmin")]
		[Authorize]
		public ResultDto<bool> IsAdmin()
		{
			try
			{
				var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
				if (role == null)
				{
					throw new Exception("Can't get role.");
				}

				return new ResultDto<bool>()
				{
					Data = role == "0",
					Message = "Succcess",
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

		[HttpPost("RefreshToken")]
		[AllowAnonymous]
		public async Task<IActionResult> RefreshToken(TokenDto token)
		{
			try
			{
				var response = await _userIdentityService.RefreshToken(token);
				if(response.StatusCode != HttpStatusCode.OK || response.Data == null)
				{
					throw new Exception(response.Message); 
				}

				return Ok(new TokenDto()
				{
					AccessToken = response.Data.AccessToken,
					RefreshToken = response.Data.RefreshToken,
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		
		[HttpPost("Revoke")]
		[AllowAnonymous]
		public async Task<IActionResult> Revoke()
		{
			try
			{
				var username = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Name)?.Value;
				if (username == null)
				{
					throw new Exception("Can't get username.");
				}

				var response = await _userIdentityService.Revoke(username);
				if(response.StatusCode != HttpStatusCode.OK || response.Data == false)
				{
					throw new Exception(response.Message); 
				}

				return Ok(new { response.Data });
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		
		[HttpPost("RevokeAll")]
		[Authorize(Roles = UserRoles.Admin)]
		public async Task<IActionResult> RevokeAll()
		{
			try
			{
				var response = await _userIdentityService.RevokeAll();
				if(response.StatusCode != HttpStatusCode.OK || response.Data == false)
				{
					throw new Exception(response.Message); 
				}

				return Ok(new { response.Data });
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
