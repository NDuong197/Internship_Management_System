using InternshipManagementSystem.Core.Entities;
using InternshipManagementSystem.Infrastructure.Data;
using InternshipManagementSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace InternshipManagementSystem.Tests
{
    public class SinhVienRepositoryTests
    {
        [Fact]
        public async Task GetByIdAsync_ReturnsSinhVien_WhenSinhVienExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var sinhVien = new SinhVien
            {
                SvId = Guid.NewGuid(),
                HoTen = "Nguyen Van A",
                MaSV = "SV001",
                Lop = "22IT3",
                Email = "a@example.com",
                MatKhau = "password",
                SDT = "0123456789",
                GPA = 3.5m
            };
            context.SinhViens.Add(sinhVien);
            await context.SaveChangesAsync();

            var repository = new Repository<SinhVien>(context);

            // Act
            var result = await repository.GetByIdAsync(sinhVien.SvId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(sinhVien.HoTen, result.HoTen);
        }
    }
}