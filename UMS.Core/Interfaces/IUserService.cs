using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Dtos;
using UMS.Core.Models;

namespace UMS.Core.Interfaces
{
	public interface IUserService
	{
		public ResultDto<string> Authenticate(string username, string password);
		public ResultDto Register(string username, string password);
	}
}
