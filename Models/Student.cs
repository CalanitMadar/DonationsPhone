using System;
using System.Collections.Generic;

namespace Models;

public partial class Student
{
    public int IdNumber { get; set; }

    public string Id { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Age { get; set; }

    public string? Group { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
