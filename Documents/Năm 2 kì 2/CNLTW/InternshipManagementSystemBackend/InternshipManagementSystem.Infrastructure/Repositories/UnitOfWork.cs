// UnitOfWork.cs
using InternshipManagementSystem.Core.Entities;
using InternshipManagementSystem.Core.Interfaces;
using InternshipManagementSystem.Infrastructure.Data;

namespace InternshipManagementSystem.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<SinhVien> _sinhViens;
        private IRepository<DoanhNghiep> _doanhNghieps;
        private IRepository<DangKyThucTap> _dangKyThucTaps;
        private IRepository<Admin> _admins;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<SinhVien> SinhViens => _sinhViens ??= new Repository<SinhVien>(_context);
        public IRepository<DoanhNghiep> DoanhNghieps => _doanhNghieps ??= new Repository<DoanhNghiep>(_context);
        public IRepository<DangKyThucTap> DangKyThucTaps => _dangKyThucTaps ??= new Repository<DangKyThucTap>(_context);
        public IRepository<Admin> Admins => _admins ??= new Repository<Admin>(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}