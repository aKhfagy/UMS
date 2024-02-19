using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Dtos;
using UMS.Core.Interfaces;
using UMS.Core.Models;

namespace UMS.Application.Services
{
	public class HTTPLogService : ServiceBase<Httplog, HttplogDto>, IHTTPLogService
	{
		public HTTPLogService(IRepository<Httplog> repository) : base(repository)
		{
		}
	}
}
