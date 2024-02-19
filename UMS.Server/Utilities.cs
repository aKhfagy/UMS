using UMS.Application.Services;
using UMS.Core.Models;
using UMS.Infrastructure.Repository;
using UMS.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UMS.Core.Interfaces.Repositories;

namespace UMS.Server
{
	public class Utilities
	{
		public static void InjectRepositories(IServiceCollection services)
		{
			services.AddScoped<IRepository<Student>, Repository<Student>>();
			services.AddScoped<IRepository<StudentSubject>, Repository<StudentSubject>>();
			services.AddScoped<IRepository<Subject>, Repository<Subject>>();
			services.AddScoped<IRepository<Teacher>, Repository<Teacher>>();
			services.AddScoped<IRepository<User>, Repository<User>>();
			services.AddScoped<IRepository<Httplog>, Repository<Httplog>>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<ISubjectRepository, SubjectRepository>();
			services.AddScoped<IStudentRepository, StudentRepository>();
			services.AddScoped<ITeacherRepository, TeacherRepository>();
			services.AddScoped<IHTTPLogRepository, HTTPLogRepository>();
		}

		public static void InjectServices(IServiceCollection services)
		{
			services.AddScoped<IStudentService, StudentService>();
			services.AddScoped<ISubjectService, SubjectService>();
			services.AddScoped<ITeacherService, TeacherService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IHTTPLogService, HTTPLogService>();
			services.AddScoped<IUserIdentityService, UserIdentityService>();
			services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		}

		private static Random random = new Random();

		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabdcefghijklmnopqrstuvwxyz0123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}
