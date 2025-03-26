using InternshipManagementSystem.Application.DTOs;
using InternshipManagementSystem.Core.Entities;
using InternshipManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipManagementSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DangKyThucTapController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DangKyThucTapController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dangKyThucTaps = await _unitOfWork.DangKyThucTaps.GetAllAsync();
            return Ok(dangKyThucTaps);
        }

        [HttpPost]
        [Authorize(Roles = "SinhVien")]
        public async Task<IActionResult> Create([FromBody] DangKyThucTapDTO dangKyThucTapDTO)
        {
            // Tìm sinh viên bằng email
            var sinhVien = await _unitOfWork.SinhViens.FirstOrDefaultAsync(s => s.Email == dangKyThucTapDTO.Email);
            if (sinhVien == null) return BadRequest("Email không hợp lệ hoặc không tồn tại.");

            var dangKyThucTap = new DangKyThucTap
            {
                DkttId = Guid.NewGuid(),
                SvId = sinhVien.SvId, // Lấy ID từ Email
                DnId = dangKyThucTapDTO.DnId,
                NgayDangKy = DateTime.Now,
                TrangThai = "Chờ duyệt"
            };

            await _unitOfWork.DangKyThucTaps.AddAsync(dangKyThucTap);
            await _unitOfWork.CompleteAsync();
            return Ok(dangKyThucTap);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "DoanhNghiep,Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DangKyThucTapDTO dangKyThucTapDTO)
        {
            var dangKyThucTap = await _unitOfWork.DangKyThucTaps.GetByIdAsync(id);
            if (dangKyThucTap == null) return NotFound();

            dangKyThucTap.TrangThai = dangKyThucTapDTO.TrangThai;

            await _unitOfWork.DangKyThucTaps.UpdateAsync(dangKyThucTap);
            await _unitOfWork.CompleteAsync();
            return Ok(dangKyThucTap);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dangKyThucTap = await _unitOfWork.DangKyThucTaps.GetByIdAsync(id);
            if (dangKyThucTap == null) return NotFound();

            await _unitOfWork.DangKyThucTaps.DeleteAsync(dangKyThucTap);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
        [HttpGet("doanh-nghieps")]
        public async Task<IActionResult> GetAllDoanhNghieps()
        {
            var doanhNghieps = await _unitOfWork.DoanhNghieps.GetAllAsync();
            return Ok(doanhNghieps);
        }

    }
}
