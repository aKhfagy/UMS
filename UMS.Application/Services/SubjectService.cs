using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Dtos;
using UMS.Core.Models;
using UMS.Infrastructure.Repository;
using UMS.Core.Interfaces;
using System.Net;

namespace UMS.Application.Services
{
	public class SubjectService : ServiceBase<Subject, SubjectDto>, ISubjectService
	{
		public SubjectService(IRepository<Subject> repository) : base(repository)
		{
		}


		public ResultDto<IsPagedOutputDto<SubjectDto>> IsPaged(IsPagedInputDto input)
		{
			try
			{
				if (input.PageIndex <= 0)
				{
					throw new Exception("Page index can't be less than one");
				}
				var dto = new IsPagedOutputDto<SubjectDto>()
				{
					PageIndex = input.PageIndex,
					PageSize = input.PageSize,
					IsDeleted = input.IsDeleted,
				};

				var list = _repository
					.GetAll()
					.Where(x => x.IsDeleted == input.IsDeleted)
					.ToList();
				dto.TotalCount = list.Count;
				dto.PageCount = (int)Math.Ceiling((decimal)list.Count / (decimal)input.PageSize);
				list = list
					.Skip((input.PageIndex - 1) * input.PageSize)
					.Take(input.PageSize)
					.ToList();

				dto.values = _mapper.Map<List<SubjectDto>>(list);

				return new ResultDto<IsPagedOutputDto<SubjectDto>>()
				{
					Data = dto,
					Message = "Got list successfuly",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch (Exception ex)
			{
				return new ResultDto<IsPagedOutputDto<SubjectDto>>
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}
		public ResultDto Restore(int id)
		{
			try
			{
				var obj = _repository.GetById(id);
				if (obj == null)
				{
					return new ResultDto()
					{
						Message = "User does not exist",
						StatusCode = HttpStatusCode.InternalServerError,
					};
				}
				obj.IsDeleted = false;
				obj.DeletionDate = null;
				obj.DeletionUserId = null;
				_repository.Update(obj);
				_repository.Save();

				return new ResultDto()
				{
					Message = "Soft Delete Successful",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch (Exception ex)
			{
				return new ResultDto()
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}
		public ResultDto SoftDelete(DeleteDto dto)
		{
			try
			{
				var obj = _repository.GetById(dto.ObjId);
				if (obj == null)
				{
					return new ResultDto()
					{
						Message = "User does not exist",
						StatusCode = HttpStatusCode.InternalServerError,
					};
				}
				obj.IsDeleted = true;
				obj.DeletionDate = DateTime.Now;
				obj.DeletionUserId = dto.UserId;
				_repository.Update(obj);
				_repository.Save();

				return new ResultDto()
				{
					Message = "Soft Delete Successful",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch (Exception ex)
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
