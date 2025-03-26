using InternshipManagementSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using InternshipManagementSystem.Infrastructure.Data;

namespace InternshipManagementSystem.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<DoanhNghiep> DoanhNghieps { get; set; }
        public DbSet<DangKyThucTap> DangKyThucTaps { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Định nghĩa ValueConverter cho TrangThai
            var trangThaiConverter = new ValueConverter<string, string>(
                v => v,
                v => ValidateTrangThai(v)
            );

            // SinhVien
            modelBuilder.Entity<SinhVien>(entity =>
            {
                entity.HasKey(e => e.SvId);
                entity.Property(e => e.SvId).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.HoTen).IsRequired().HasMaxLength(64);
                entity.Property(e => e.MaSV).IsRequired().HasMaxLength(10);
                entity.HasIndex(e => e.MaSV).IsUnique();
                entity.Property(e => e.Lop).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(50);
                entity.Property(e => e.MatKhau).IsRequired().HasMaxLength(128);
                entity.Property(e => e.SDT).IsRequired().HasMaxLength(15);
                entity.Property(e => e.GPA).IsRequired().HasPrecision(3, 2);
            });

            // DoanhNghiep
            modelBuilder.Entity<DoanhNghiep>(entity =>
            {
                entity.HasKey(e => e.DnId);
                entity.Property(e => e.DnId).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.TenDN).IsRequired().HasMaxLength(100);
                entity.Property(e => e.DiaChi).IsRequired().HasMaxLength(200);
                entity.Property(e => e.SoDT).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.MatKhau).IsRequired().HasMaxLength(128);
            });

            // DangKyThucTap
            modelBuilder.Entity<DangKyThucTap>(entity =>
            {
                entity.HasKey(e => e.DkttId);
                entity.Property(e => e.DkttId).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.NgayDangKy).IsRequired().HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.TrangThai).IsRequired().HasMaxLength(20);
                entity.Property(e => e.TrangThai).HasConversion(trangThaiConverter);

                entity.HasOne(e => e.SinhVien)
                      .WithMany(s => s.DangKyThucTaps)
                      .HasForeignKey(e => e.SvId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.DoanhNghiep)
                      .WithMany(d => d.DangKyThucTaps)
                      .HasForeignKey(e => e.DnId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Admin
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.AdminId);
                entity.Property(e => e.AdminId).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.Email).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.MatKhau).IsRequired().HasMaxLength(128);
            });

            // Không có .HasData() ở đây nữa
        }
        // Phương thức kiểm tra giá trị TrangThai
        private static string ValidateTrangThai(string value)
        {
            return value switch
            {
                "Chờ duyệt" => "Chờ duyệt",
                "Đã duyệt" => "Đã duyệt",
                "Từ chối" => "Từ chối",
                _ => throw new ArgumentException("Invalid TrangThai value")
            };
        }
    }
}