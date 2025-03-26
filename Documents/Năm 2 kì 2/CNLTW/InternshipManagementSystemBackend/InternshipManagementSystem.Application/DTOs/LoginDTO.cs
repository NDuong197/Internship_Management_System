using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipManagementSystem.Application.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // "SinhVien", "DoanhNghiep", "Admin"
    }
}
