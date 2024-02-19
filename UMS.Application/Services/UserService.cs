using AutoMapper;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Utils;
using UMS.Core.Dtos;
using UMS.Core.Interfaces;
using UMS.Core.Models;

namespace UMS.Application.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly Mapper _mapper;
		private readonly IAuthService _authService;

		public UserService(
			IUserRepository userRepository, 
			IAuthService authService)
		{
			_userRepository = userRepository;
			_mapper = MapperConfig.InitializeAutomapper();
			_authService = authService;
		}

		public ResultDto<string> Authenticate(string username, string password)
		{
			var user = _userRepository.GetByUsername(username);

			if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
			{
				return new ResultDto<string>() 
				{ 
					Message = "Username or password is incorrect",
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
			var authToken = _authService.GenerateJwtToken(_mapper.Map<UserDto>(user));
			if(authToken.Data == null || authToken.StatusCode != HttpStatusCode.OK)
			{
				return new ResultDto<string>()
				{
					Message = authToken.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
			return new ResultDto<string>()
			{
				Data = authToken.Data,
				Message = "Successful Login",
				StatusCode = HttpStatusCode.OK,
			};
		}

		public ResultDto Register(string username, string password)
		{
			try
			{
				User user = new User()
				{
					Username = username,
					Password = BCrypt.Net.BCrypt.HashPassword(password),
					Role = 1 // viewer (can't edit)
				};

				_userRepository.Insert(user);
				_userRepository.Save();
				return new ResultDto()
				{
					Message = "Added new user",
					StatusCode = HttpStatusCode.OK,
				};

			}
			catch(Exception ex)
			{
				return new ResultDto()
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}
	}
}
