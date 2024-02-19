using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Interfaces;
using UMS.Core.Models;

namespace UMS.Infrastructure.Repository
{
	public class TeacherRepository : Repository<Teacher>, ITeacherRepository
	{
		public TeacherRepository(IConfiguration configuration) : base(configuration)
		{
		}

		public IEnumerable<Subject> GetAssignedSubjects(int id)
		{
			using (IDbConnection dbConnection = 
				new SqlConnection(configuration["ConnectionStrings:Default"]))
			{
				dbConnection.Open();
				string query = "SELECT * FROM Subject WHERE TeacherID = @TeacherId";
				return dbConnection.Query<Subject>(query, new { TeacherId = id });
			}
		}
	}
}
