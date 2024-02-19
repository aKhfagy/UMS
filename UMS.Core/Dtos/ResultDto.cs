using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Core.Dtos
{
	public class ResultDto<T>
	{
		public T? Data { get; set; }
		public string Message { get; set; }
		public HttpStatusCode StatusCode { get; set; }
	}
	public class ResultDto
	{
		public string Message { get; set; }
		public HttpStatusCode StatusCode { get; set; }
	}
}
