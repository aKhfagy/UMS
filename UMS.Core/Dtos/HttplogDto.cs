using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Core.Dtos
{
	public class HttplogDto
	{
		public int? StatusCode { get; set; }

		public DateTime? Date { get; set; }

		public string? Type { get; set; }

		public string? Request { get; set; }

		public string? Body { get; set; }
	}
}
