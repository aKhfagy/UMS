using System;
using System.Collections.Generic;

namespace UMS.Core.Models;

public partial class Teacher
{
    public int Id { get; set; }

    public string CreatorUserId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime? CreationDate { get; set; }

    public string? ModificationUserId { get; set; }

    public DateTime? ModificationDate { get; set; }

    public bool IsDeleted { get; set; }

    public string? DeletionUserId { get; set; }

    public DateTime? DeletionDate { get; set; }

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
