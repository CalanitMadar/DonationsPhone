using System;
using System.Collections.Generic;

namespace Models;

public partial class Payment
{
    public int Id { get; set; }

    public DateTime? Date { get; set; }

    public string? HebrewDate { get; set; }

    public TimeSpan? Time { get; set; }

    public string? StudentId { get; set; }

    public decimal? SumOfMoney { get; set; }

    public bool? IsGroup { get; set; }

    public virtual Student? Student { get; set; }
}
