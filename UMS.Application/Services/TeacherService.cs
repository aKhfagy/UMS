using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Dtos;
using UMS.Core.Models;
using UMS.Infrastructure.Repository;
using UMS.Core.Interfaces;
using UMS.Application.Utils;
using System.Net;

namespace UMS.Application.Services
{
	public class TeacherService : ServiceBase<Teacher, TeacherDto>, ITeacherService
	{
		private readonly ITeacherRepository _teacherRepository;
		private readonly IRepository<Subject> _subjectRepository;
		public TeacherService(
			IRepository<Teacher> repository, 
			ITeacherRepository teacherRepository,
			IRepository<Subject> subjectRepository) : base(repository)
		{
			_teacherRepository = teacherRepository;
			_subjectRepository = subjectRepository;
		}

		public ResultDto Assign(int TeacherId, int SubjectId, string userId)
		{
			try
			{
				Subject subject = _subjectRepository.GetById(SubjectId);

				if (subject == null)
				{
					return new ResultDto()
					{
						Message = "Subject does not exist",
						StatusCode = HttpStatusCode.InternalServerError,
					};
				}

				subject.TeacherId = TeacherId;
				subject.ModificationUserId = userId;
				subject.ModificationDate = DateTime.Now;
				_subjectRepository.Update(subject);
				_subjectRepository.Save();

				return new ResultDto()
				{
					Message = "Teacher assigned successfuly",
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

		public ResultDto<List<SubjectDto>> GetAssignedSubjects(int id)
		{
			try
			{
				var result = _teacherRepository.GetAssignedSubjects(id);
				return new ResultDto<List<SubjectDto>>()
				{
					Data = _mapper.Map<List<SubjectDto>>(result),
					Message = "Successful Operation",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch(Exception ex)
			{
				return new ResultDto<List<SubjectDto>>()
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}

		public ResultDto<IsPagedOutputDto<TeacherDto>> IsPaged(IsPagedInputDto input)
		{
			try
			{
				if (input.PageIndex <= 0)
				{
					throw new Exception("Page index can't be less than one");
				}
				var dto = new IsPagedOutputDto<TeacherDto>()
				{
					PageIndex = input.PageIndex,
					PageSize = input.PageSize,
					IsDeleted = input.IsDeleted,
				};

				var list = _teacherRepository
					.GetAll()
					.Where(x => x.IsDeleted == input.IsDeleted)
					.ToList();
				dto.TotalCount = list.Count;
				dto.PageCount = (int)Math.Ceiling((decimal)list.Count / (decimal)input.PageSize);
				list = list
					.Skip((input.PageIndex - 1) * input.PageSize)
					.Take(input.PageSize)
					.ToList();

				dto.values = _mapper.Map<List<TeacherDto>>(list);

				return new ResultDto<IsPagedOutputDto<TeacherDto>>()
				{
					Data = dto,
					Message = "Got list successfuly",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch (Exception ex)
			{
				return new ResultDto<IsPagedOutputDto<TeacherDto>>
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
				if(obj == null)
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
			catch(Exception ex) {
				return new ResultDto()
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}
	}
}
