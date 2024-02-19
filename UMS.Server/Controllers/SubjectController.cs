using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using UMS.Application.Services;
using UMS.Core.Dtos;
using UMS.Core.Interfaces;
using UMS.Core.Utils;

namespace UMS.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly ILogger<SubjectController> _logger;
        private readonly ISubjectService _subjectService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public SubjectController(
            ILogger<SubjectController> logger,
			ISubjectService subjectService,
			IHttpContextAccessor httpContextAccessor)		
		{
			_logger = logger;
			_subjectService = subjectService;
			_httpContextAccessor = httpContextAccessor;
		}

		[HttpGet("GetAll")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<List<SubjectDto>> GetAllSubjects()
        {
            try
            {
				var list = new List<SubjectDto>();

				var result = _subjectService.GetAll();

				if (result.StatusCode == HttpStatusCode.OK && result.Data != null)
				{
					list = result.Data;
				}
				else
				{
					return new ResultDto<List<SubjectDto>>()
					{
						Message = result.Message,
						StatusCode = HttpStatusCode.InternalServerError,
					};
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
		[HttpPut("IsPaged")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<IsPagedOutputDto<SubjectDto>> IsPaged(IsPagedInputDto input)
		{
			try
			{
				var result = _subjectService.IsPaged(input);

				if (result.StatusCode != HttpStatusCode.OK || result.Data == null)
				{
					return new ResultDto<IsPagedOutputDto<SubjectDto>>()
					{
						Message = result.Message,
						StatusCode = HttpStatusCode.InternalServerError,
					};
				}

				return result;
			}
			catch (Exception ex)
			{
				return new ResultDto<IsPagedOutputDto<SubjectDto>>()
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}
		[HttpPost("Add")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<SubjectDto> Add(SubjectDto subjectDto)
		{
			try
			{
				var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				if (userId == null)
				{
					throw new Exception("Can't get user Id");
				}
				subjectDto.CreationDate = DateTime.Now;
				subjectDto.CreatorUserId = userId;
				var response = _subjectService.Add(subjectDto);
				return response;
			}
			catch (Exception ex)
			{
				return new ResultDto<SubjectDto>()
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}
		[HttpPut("Edit")]
		[Authorize(Roles = UserRoles.Admin)]
		public ResultDto<SubjectDto> Edit(SubjectDto subjectDto)
		{
			try
			{
				var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				if (userId == null)
				{
					throw new Exception("Can't get user Id");
				}
				subjectDto.ModificationDate = DateTime.Now;
				subjectDto.ModificationUserId = userId;
				var response = _subjectService.Edit(subjectDto);
				return response;
			}
			catch (Exception ex)
			{
				return new ResultDto<SubjectDto>()
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
				var response = _subjectService.SoftDelete(obj);
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
				var response = _subjectService.Restore(id);
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
				var response = _subjectService.Delete(id);
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
