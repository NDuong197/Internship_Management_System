using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipManagementSystem.Application.DTOs
{
    public class SinhVienDTO
    {
        public Guid SvId { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string MaSV { get; set; } = string.Empty;
        public string Lop { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
        public string SDT { get; set; } = string.Empty;
        public decimal GPA { get; set; }
    }
}