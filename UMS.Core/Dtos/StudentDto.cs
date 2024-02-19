using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Models;

namespace UMS.Core.Dtos
{
	public class StudentDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public string CreatorUserId { get; set; } = null!;

		public DateTime? CreationDate { get; set; }

		public string? ModificationUserId { get; set; }

		public DateTime? ModificationDate { get; set; }

		public bool IsDeleted { get; set; }

		public string? DeletionUserId { get; set; }

		public DateTime? DeletionDate { get; set; }

	}
}
