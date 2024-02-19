using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Core.Dtos
{
	public class TokenDto
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public TokenDto()
		{
			AccessToken = string.Empty;
			RefreshToken = string.Empty;
		}
	}
}
