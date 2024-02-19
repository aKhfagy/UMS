using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Dtos;
using UMS.Core.Models;

namespace UMS.Application.Utils
{
	public class MapperConfig
	{
		public static Mapper InitializeAutomapper()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<Student, StudentDto>();
				cfg.CreateMap<StudentDto, Student>();

				cfg.CreateMap<StudentSubject, StudentSubjectDto>()
				.ForMember(
					x => x.Grade, 
					y => y.MapFrom(
						z => 100));
				cfg.CreateMap<StudentSubjectDto, StudentSubject>();

				cfg.CreateMap<Subject, SubjectDto>();
				cfg.CreateMap<SubjectDto, Subject>();

				cfg.CreateMap<Teacher, TeacherDto>();
				cfg.CreateMap<TeacherDto, Teacher>();

				cfg.CreateMap<User, UserDto>();
				cfg.CreateMap<UserDto, User>();

				cfg.CreateMap<Httplog, HttplogDto>();
				cfg.CreateMap<HttplogDto, Httplog>();
			});

			var mapper = new Mapper(config);
			return mapper;
		}
	}
}
