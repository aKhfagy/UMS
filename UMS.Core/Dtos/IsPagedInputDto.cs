using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Core.Dtos
{
	public class IsPagedInputDto
	{
		public int PageIndex { get; set; }
		public int PageSize { get; set; } = 5;
		public bool IsDeleted { get; set; }
	}
}
