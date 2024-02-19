using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Core.Dtos
{
	public class UserDto
	{
		public int Id { get; set; }

		public string Username { get; set; } = null!;

		public string Password { get; set; } = null!;

		public int Role { get; set; }
	}

	public class LoginDto
	{
		public string Username { get; set; } = null!;

		public string Password { get; set; } = null!;
	}
}
