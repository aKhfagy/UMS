using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Core.Dtos
{
	public class IsPagedOutputDto<T> where T : class
	{
		public int PageIndex { get; set; }
		public int PageSize { get; set; } = 5;
		public int PageCount { get; set; }
		public int TotalCount { get; set; }
		public bool IsDeleted { get; set; }
		public List<T> values { get; set; }
		public IsPagedOutputDto() 
		{ 
			values = new List<T>();
		}
	}
}
