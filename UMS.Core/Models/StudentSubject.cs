using System;
using System.Collections.Generic;

namespace UMS.Core.Models;

public partial class StudentSubject
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int SubjectId { get; set; }

    public decimal? Grade { get; set; }

    public DateTime? CreationDate { get; set; }

    public string? ModificationUserId { get; set; }

    public DateTime? ModificationDate { get; set; }

    public bool IsDeleted { get; set; }

    public string? DeletionUserId { get; set; }

    public DateTime? DeletionDate { get; set; }

    public string CreatorUserId { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
