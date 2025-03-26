using InternshipManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// IUnitOfWork.cs
namespace InternshipManagementSystem.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<SinhVien> SinhViens { get; }
        IRepository<DoanhNghiep> DoanhNghieps { get; }
        IRepository<DangKyThucTap> DangKyThucTaps { get; }
        IRepository<Admin> Admins { get; }
        Task<int> CompleteAsync();
    }
}