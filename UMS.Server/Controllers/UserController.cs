using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using UMS.Application.Services;
using UMS.Core.Dtos;
using UMS.Core.Interfaces;

namespace UMS.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserController(
            ILogger<UserController> logger,
			IUserService userService,
			IAuthService authService,
			IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
			_userService = userService;
			_authService = authService;
			_httpContextAccessor = httpContextAccessor;
        }

		[HttpPost("login")]
		[AllowAnonymous]
		public IActionResult Login(LoginDto model)
		{
			var authResult = _userService.Authenticate(model.Username, model.Password);
			if(authResult.StatusCode == HttpStatusCode.OK && authResult.Data != null)
			{
				return Ok(new { authResult.Data });
			}
			
			return Unauthorized();
		}
		[HttpGet("IsAdmin")]
		[Authorize]
		public ResultDto<bool> IsAdmin()
		{
			try
			{
				var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
				if(role == null)
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
			catch(Exception ex)
			{
				return new ResultDto<bool>()
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}

		}

		[HttpPost("Register")]
		[AllowAnonymous]
		public IActionResult Register(LoginDto model)
		{
			var authResult = _userService.Register(model.Username, model.Password);
			if (authResult.StatusCode == HttpStatusCode.OK)
			{
				return Ok();
			}

			return StatusCode(500);
		}

	}
}
