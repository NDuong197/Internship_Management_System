using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipManagementSystem.Application.DTOs
{
    public class DangKyThucTapDTO
    {
        public Guid DkttId { get; set; }
        public string Email { get; set; } = string.Empty; 
        public Guid DnId { get; set; }
        public DateTime NgayDangKy { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }
}