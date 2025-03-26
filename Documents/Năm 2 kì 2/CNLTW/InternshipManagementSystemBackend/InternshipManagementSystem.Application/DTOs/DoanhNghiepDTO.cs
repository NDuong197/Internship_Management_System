using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipManagementSystem.Application.DTOs
{
    public class DoanhNghiepDTO
    {
        public Guid DnId { get; set; }
        public string TenDN { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public string SoDT { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
    }
}