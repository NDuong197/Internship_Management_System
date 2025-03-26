namespace InternshipManagementSystem.Core.Entities
{
    public class DangKyThucTap
    {
        public Guid DkttId { get; set; }
        public Guid SvId { get; set; }
        public Guid DnId { get; set; }
        public DateTime NgayDangKy { get; set; }
        public string TrangThai { get; set; } = string.Empty;

        public SinhVien SinhVien { get; set; } = null!;
        public DoanhNghiep DoanhNghiep { get; set; } = null!;
    }
}