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
	public class StudentService : ServiceBase<Student, StudentDto>, IStudentService
	{
		private readonly IStudentRepository _studentRepository;
		public StudentService(
			IRepository<Student> repository, 
			IStudentRepository studentRepository) : base(repository)
		{
			_studentRepository = studentRepository;
		}

		public ResultDto Enroll(int StudentId, int SubjectId, string userId)
		{
			try
			{
				_studentRepository.EnrollStudent(StudentId, SubjectId, userId);
				return new ResultDto()
				{
					StatusCode = HttpStatusCode.OK,
					Message = "Student Enrolled."
				};
			}
			catch (Exception ex)
			{
				return new ResultDto()
				{
					StatusCode = HttpStatusCode.InternalServerError,
					Message = ex.Message,
				};
			}
		}

		public ResultDto<List<StudentSubjectDto>> GetStudentSubjects(int id)
		{
			try
			{
				var result = _studentRepository.GetStudentSubjects(id);
				return new ResultDto<List<StudentSubjectDto>>()
				{
					Data = _mapper.Map<List<StudentSubjectDto>>(result),
					Message = "Successful Operation",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch (Exception ex)
			{
				return new ResultDto<List<StudentSubjectDto>>
				{
					Message = ex.Message,
					StatusCode = HttpStatusCode.InternalServerError,
				};
			}
		}

		public ResultDto<IsPagedOutputDto<StudentDto>> IsPaged(IsPagedInputDto input)
		{
			try
			{
				if(input.PageIndex <= 0)
				{
					throw new Exception("Page index can't be less than one");
				}
				var dto = new IsPagedOutputDto<StudentDto>()
				{
					PageIndex = input.PageIndex,
					PageSize = input.PageSize,
					IsDeleted = input.IsDeleted,
				};

				var list = _studentRepository
					.GetAll()
					.Where(x => x.IsDeleted == input.IsDeleted)
					.ToList();
				dto.TotalCount = list.Count;
				dto.PageCount = (int)Math.Ceiling((decimal)list.Count / (decimal)input.PageSize);
				list = list
					.Skip((input.PageIndex - 1) * input.PageSize)
					.Take(input.PageSize)
					.ToList();

				dto.values = _mapper.Map<List<StudentDto>>(list);

				return new ResultDto<IsPagedOutputDto<StudentDto>>()
				{
					Data = dto,
					Message = "Got list successfuly",
					StatusCode = HttpStatusCode.OK,
				};
			}
			catch(Exception ex)
			{
				return new ResultDto<IsPagedOutputDto<StudentDto>>
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
