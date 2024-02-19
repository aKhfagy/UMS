using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using UMS.Application.Services;
using UMS.Core.Dtos;
using UMS.Core.Interfaces;
using UMS.Core.Utils;

namespace UMS.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ILogger<TeacherController> _logger;
        private readonly ITeacherService _teacherService;
		private readonly IHttpContextAccessor _httpContextAccessor;

        public TeacherController(
			ILogger<TeacherController> logger,
			ITeacherService teacherService,
			IHttpContextAccessor httpContextAccessor)
		{
			_logger = logger;
			_teacherService = teacherService;
			_httpContextAccessor = httpContextAccessor;
		}

		[HttpGet("GetAll")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<List<TeacherDto>> GetAllteachers()
        {
            try
            {
				var list = new List<TeacherDto>();

				var result = _teacherService.GetAll();

				if (result.StatusCode == HttpStatusCode.OK && result.Data != null)
				{
					list = result.Data;
				}
				else
				{
					return new ResultDto<List<TeacherDto>>()
					{
						Message = result.Message,
						StatusCode = HttpStatusCode.InternalServerError,
					};
				}

				return new ResultDto<List<TeacherDto>>() 
                { 
                    Data = list,
                    Message = "Success",
					StatusCode = HttpStatusCode.OK,
				};
			}
            catch(Exception ex)
            {
                return new ResultDto<List<TeacherDto>>()
                {
                    Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
            }

        }
		[HttpPut("IsPaged")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<IsPagedOutputDto<TeacherDto>> IsPaged(IsPagedInputDto input)
		{
			try
			{
				var result = _teacherService.IsPaged(input);

				if (result.StatusCode != HttpStatusCode.OK || result.Data == null)
				{
					return new ResultDto<IsPagedOutputDto<TeacherDto>>()
					{
						Message = result.Message,
						StatusCode = HttpStatusCode.InternalServerError,
					};
				}

				return result;
			}
			catch (Exception ex)
			{
				return new ResultDto<IsPagedOutputDto<TeacherDto>>()
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}
		[HttpGet("GetSubjects")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<List<SubjectDto>> GetAssignedSubjects(int id)
        {
            try
            {
				var list = new List<SubjectDto>();

				var result = _teacherService.GetAssignedSubjects(id);

				if (result.StatusCode == HttpStatusCode.OK && result.Data != null)
				{
					list = result.Data;
				}

				return new ResultDto<List<SubjectDto>>() 
                { 
                    Data = list,
                    Message = "Success",
					StatusCode = HttpStatusCode.OK,
				};
			}
            catch (Exception ex)
            {
                return new ResultDto<List<SubjectDto>>()
                {
                    Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
            }
        }

        [HttpPost("Assign")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<bool> AssignSubject(int teacherId, int subjectId)
        {
			var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				throw new Exception("Can't get user Id");
			}
			var result = _teacherService.Assign(teacherId, subjectId, userId);

			if (result.StatusCode == HttpStatusCode.OK)
			{
                return new ResultDto<bool>() {
                    Data = true,
                    Message = "Success",
					StatusCode = HttpStatusCode.OK,
				};
			}

			return new ResultDto<bool>()
			{
                Data = false,
				Message = result.Message,
				StatusCode = HttpStatusCode.InternalServerError,
			};
		}
		[HttpPost("Add")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<TeacherDto> Add(TeacherDto teacherDto)
		{
			try
			{
				var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				if (userId == null)
				{
					throw new Exception("Can't get user Id");
				}
				teacherDto.CreationDate = DateTime.Now;
				teacherDto.CreatorUserId = userId;
				var response = _teacherService.Add(teacherDto);
				return response;
			}
			catch (Exception ex)
			{
				return new ResultDto<TeacherDto>()
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}
		[HttpPut("Edit")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<TeacherDto> Edit(TeacherDto teacherDto)
		{
			try
			{
				var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				if (userId == null)
				{
					throw new Exception("Can't get user Id");
				}
				teacherDto.ModificationDate = DateTime.Now;
				teacherDto.ModificationUserId = userId;
				var response = _teacherService.Edit(teacherDto);
				return response;
			}
			catch (Exception ex)
			{
				return new ResultDto<TeacherDto>()
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}
		[HttpPost("SoftDelete")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<bool> SoftDelete(int id)
		{
			try
			{
				var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				if (userId == null)
				{
					throw new Exception("Can't get user Id");
				}
				var obj = new DeleteDto();
				obj.ObjId = id;
				obj.UserId = userId;
				var response = _teacherService.SoftDelete(obj);
				if (response.StatusCode != HttpStatusCode.OK)
				{
					return new ResultDto<bool>()
					{
						Data = false,
						Message = response.Message,
						StatusCode = HttpStatusCode.InternalServerError,
					};
				}

				return new ResultDto<bool>()
				{
					Data = true,
					Message = "Success",
					StatusCode = HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ResultDto<bool>()
				{
					Data = false,
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}

		}
		[HttpPost("Restore")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<bool> Restore(int id)
		{
			try
			{
				var response = _teacherService.Restore(id);
				if (response.StatusCode != HttpStatusCode.OK)
				{
					return new ResultDto<bool>()
					{
						Data = false,
						Message = response.Message,
						StatusCode = HttpStatusCode.InternalServerError,
					};
				}

				return new ResultDto<bool>()
				{
					Data = true,
					Message = "Success",
					StatusCode = HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ResultDto<bool>()
				{
					Data = false,
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}
		[HttpDelete("Delete")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<bool> Delete(int id)
		{
			try
			{
				var response = _teacherService.Delete(id);
				if (response.StatusCode != HttpStatusCode.OK)
				{
					return new ResultDto<bool>()
					{
						Data = false,
						Message = response.Message,
						StatusCode = HttpStatusCode.InternalServerError,
					};
				}

				return new ResultDto<bool>()
				{
					Data = true,
					Message = "Success",
					StatusCode = HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ResultDto<bool>()
				{
					Data = false,
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}
	}
}
