using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Dtos;
using UMS.Core.Models;

namespace UMS.Core.Interfaces
{
	public interface ITeacherService : IServiceBase<Teacher, TeacherDto>
	{
		public ResultDto Assign(int TeacherId, int SubjectId, string userId);
		public ResultDto<List<SubjectDto>> GetAssignedSubjects(int id);
		public ResultDto SoftDelete(DeleteDto dto);
		public ResultDto Restore(int id);
		public ResultDto<IsPagedOutputDto<TeacherDto>> IsPaged(IsPagedInputDto input);
	}
}
