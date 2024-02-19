using System;
using System.Collections.Generic;

namespace UMS.Core.Models;

public partial class Httplog
{
    public int Id { get; set; }

    public int? StatusCode { get; set; }

    public DateTime? Date { get; set; }

    public string? Type { get; set; }

    public string? Request { get; set; }

    public string? Body { get; set; }
}
