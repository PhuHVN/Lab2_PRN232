using System;
using System.Collections.Generic;

namespace PRN232.LaptopShop.Repository.Entities;

public partial class Account
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }
}
