using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using UMS.Core.Dtos;
using UMS.Core.Interfaces;
using UMS.Core.Models;
using UMS.Core.Utils;

namespace UMS.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentService _studentService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public StudentController(
            ILogger<StudentController> logger,
            IStudentService studentService,
			IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _studentService = studentService;
			_httpContextAccessor = httpContextAccessor;
        }

		[HttpGet("GetAll")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<List<StudentDto>> GetAllStudents()
        {
            try
            {
				var list = new List<StudentDto>();

				var result = _studentService.GetAll();

				if (result.StatusCode == HttpStatusCode.OK && result.Data != null)
				{
					list = result.Data;
				}
				else
				{
					return new ResultDto<List<StudentDto>>()
					{
						Message = result.Message,
						StatusCode = HttpStatusCode.InternalServerError,
					};
				}


				return new ResultDto<List<StudentDto>>() 
                { 
                    Data = list,
                    Message = "Success",
					StatusCode = HttpStatusCode.OK,
				};
			}
            catch(Exception ex)
            {
                return new ResultDto<List<StudentDto>>()
                {
                    Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
            }
        }
		[HttpPut("IsPaged")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<IsPagedOutputDto<StudentDto>> IsPaged(IsPagedInputDto input)
        {
            try
            {
				var result = _studentService.IsPaged(input);

				if (result.StatusCode != HttpStatusCode.OK || result.Data == null)
				{
					return new ResultDto<IsPagedOutputDto<StudentDto>>()
					{
						Message = result.Message,
						StatusCode = HttpStatusCode.InternalServerError,
					};
				}

				return result;
			}
            catch(Exception ex)
            {
                return new ResultDto<IsPagedOutputDto<StudentDto>>()
                {
                    Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
            }
        }
        [HttpGet("GetSubjects")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<List<StudentSubjectDto>> GetSubjects(int id)
        {
            try
            {
				var list = new List<StudentSubjectDto>();

				var result = _studentService.GetStudentSubjects(id);

				if (result.StatusCode == HttpStatusCode.OK && result.Data != null)
				{
					list = result.Data;
				}

				return new ResultDto<List<StudentSubjectDto>>() 
                { 
                    Data = list,
                    Message = "Success",
					StatusCode = HttpStatusCode.OK,
				};
			}
            catch (Exception ex)
            {
                return new ResultDto<List<StudentSubjectDto>>()
                {
                    Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
            }
        }
		[HttpPost("Enroll")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<bool> EnrollStudent(int studentId, int subjectId)
        {
			var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				throw new Exception("Can't get user Id");
			}
			var result = _studentService.Enroll(studentId, subjectId, userId);

            if(result.StatusCode == HttpStatusCode.OK)
            {
                return new ResultDto<bool>() 
                { 
                    Data = true,
					StatusCode = HttpStatusCode.OK,
					Message = "Success"
                };
            }

			return new ResultDto<bool>()
			{
				Data = false,
				StatusCode = HttpStatusCode.InternalServerError,
				Message = result.Message
			};
		}
        [HttpPost("Add")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<StudentDto> Add(StudentDto studentDto)
        {
            try
            {
				var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				if (userId == null)
				{
					throw new Exception("Can't get user Id");
				}
				studentDto.CreationDate = DateTime.Now;
				studentDto.CreatorUserId = userId;
				var response = _studentService.Add(studentDto);
				return response;
            }
            catch(Exception ex)
            {
                return new ResultDto<StudentDto>()
                {
                    Message= ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                };
            }
        }
		[HttpPut("Edit")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<StudentDto> Edit(StudentDto studentDto)
		{
			try
			{
				var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				if (userId == null)
				{
					throw new Exception("Can't get user Id");
				}
				studentDto.ModificationDate = DateTime.Now;
				studentDto.ModificationUserId = userId;
				var response = _studentService.Edit(studentDto);
				return response;
			}
			catch (Exception ex)
			{
				return new ResultDto<StudentDto>()
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
				var response = _studentService.SoftDelete(obj);
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
				var response = _studentService.Restore(id);
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
				var response = _studentService.Delete(id);
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
