namespace InternshipManagementSystem.Core.Entities
{
    public class DoanhNghiep
    {
        public Guid DnId { get; set; }
        public string TenDN { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public string SoDT { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;

        public List<DangKyThucTap> DangKyThucTaps { get; set; } = new List<DangKyThucTap>();
    }
}