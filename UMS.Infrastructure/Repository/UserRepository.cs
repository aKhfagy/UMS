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
	public class UserRepository : Repository<User>, IUserRepository
	{
		public UserRepository(IConfiguration configuration) : base(configuration)
		{
		}

		public User GetByUsername(string username)
		{
			try
			{
				var user = _context.Users.SingleOrDefault(u => u.Username == username);
				if (user == null)
				{
					throw new Exception("UserName does not exist in Db");
				}

				return user;
			}catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}

		}
	}
}
