using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Dtos;
using UMS.Core.Models;

namespace UMS.Core.Interfaces
{
	public interface IStudentService : IServiceBase<Student, StudentDto>
	{
		public ResultDto Enroll(int StudentId, int SubjectId, string userId);
		public ResultDto<List<StudentSubjectDto>> GetStudentSubjects(int id);
		public ResultDto SoftDelete(DeleteDto dto);
		public ResultDto Restore(int id);
		public ResultDto<IsPagedOutputDto<StudentDto>> IsPaged(IsPagedInputDto input);
	}
}
