using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Models;

namespace UMS.Core.Interfaces
{
	public interface IUserRepository : IRepository<User>
	{
		User GetByUsername(string username);
	}
}
