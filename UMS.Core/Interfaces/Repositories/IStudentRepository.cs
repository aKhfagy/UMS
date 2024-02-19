using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Models;

namespace UMS.Core.Interfaces
{
	public interface IStudentRepository : IRepository<Student>
	{
		IEnumerable<StudentSubject> GetStudentSubjects(int id);
		void EnrollStudent (int studentId, int subjectId, string userId);
	}
}
