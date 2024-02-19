using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UMS.Core.Interfaces;
using UMS.Core.Models;

namespace UMS.Infrastructure.Repository
{
	public class StudentRepository : Repository<Student>, IStudentRepository
	{
		private readonly IRepository<StudentSubject> _studentSubjectRepository;

		private readonly IRepository<Subject> _subjectRepository;

		public StudentRepository(
			IRepository<StudentSubject> studentSubjectRepository,
			IRepository<Subject> subjectRepository, IConfiguration configuration) : base(configuration) 
		{
			
			_studentSubjectRepository = studentSubjectRepository;
			_subjectRepository = subjectRepository;
		}

		public void EnrollStudent(int studentId, int subjectId, string userId)
		{
			using (IDbConnection dbConnection = new SqlConnection(configuration["ConnectionStrings:Default"]))
			{
				dbConnection.Open();
				var currentDate = DateTime.Now;
				string sql = @"INSERT INTO StudentSubject (
						StudentId, SubjectId, Grade, CreationDate, ModificationUserId, ModificationDate, 
						IsDeleted, DeletionUserId, DeletionDate, CreatorUserId)
						VALUES (@StudentId, @SubjectId, @Grade, @CreationDate, NULL, NULL, 0, NULL, NULL, @UserId)";

				dbConnection.Execute(sql, new { 
					StudentId = studentId, 
					SubjectId = subjectId, 
					Grade = 10, 
					CreationDate = currentDate, 
					UserId = userId });
			}
		}

		public IEnumerable<StudentSubject> GetStudentSubjects(int id)
		{
			var studentSubjects = _studentSubjectRepository.GetAll();
			var subjects = _subjectRepository.GetAll();

			var query = from studentSubject in studentSubjects
						join sub in subjects on studentSubject.SubjectId equals sub.Id
						where studentSubject.StudentId == id
						select new StudentSubject
						{
							StudentId = studentSubject.StudentId,
							SubjectId = studentSubject.SubjectId,
							Subject = sub
						};

			IEnumerable<StudentSubject> result = query.ToList();

			return result;
		}
	}
}
