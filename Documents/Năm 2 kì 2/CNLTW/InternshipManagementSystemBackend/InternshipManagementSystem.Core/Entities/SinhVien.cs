namespace InternshipManagementSystem.Core.Entities
{
    public class SinhVien
    {
        public Guid SvId { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string MaSV { get; set; } = string.Empty;
        public string Lop { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
        public string SDT { get; set; } = string.Empty;
        public decimal GPA { get; set; }

        public List<DangKyThucTap> DangKyThucTaps { get; set; } = new List<DangKyThucTap>();
    }
}