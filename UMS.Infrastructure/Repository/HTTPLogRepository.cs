using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Interfaces;
using UMS.Core.Interfaces.Repositories;
using UMS.Core.Models;

namespace UMS.Infrastructure.Repository
{
	public class HTTPLogRepository : Repository<Httplog>, IHTTPLogRepository
	{
		public HTTPLogRepository(IConfiguration configuration) : base(configuration)
		{
		}
	}
}
