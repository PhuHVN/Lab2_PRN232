using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.LaptopShop.Service.DTO
{
    public class AccountResponse
    {
        public int AccountId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Role { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
