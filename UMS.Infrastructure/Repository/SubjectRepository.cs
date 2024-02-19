using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Interfaces;
using UMS.Core.Models;

namespace UMS.Infrastructure.Repository
{
	public class SubjectRepository : Repository<Subject>, ISubjectRepository
	{
		public SubjectRepository(IConfiguration configuration) : base(configuration)
		{
		}
	}
}
