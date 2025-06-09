using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLKS.API.Models.Domain;
using QLKS.API.Models.DTO.ChiTietDichVu; // Namespace chứa DTOs cho ChiTietDichVu
using QLKS.API.Data; // Đảm bảo đúng namespace cho DbContext
using QuanLyKhachSan.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChiTietDichVuController : ControllerBase
{
    private readonly QLKSDbContextcs _dbContext; // Sử dụng tên DbContext của bạn

    public ChiTietDichVuController(QLKSDbContextcs dbContext) // Sử dụng tên DbContext của bạn
    {
        _dbContext = dbContext;
    }

    // GET ChiTietDichVu theo IdPhieuDatPhong
    // GET: /api/ChiTietDichVu/{idPhieu}
    [HttpGet("{idPhieu:int}")] // Chỉ rõ idPhieu là số nguyên
    public async Task<IActionResult> GetByPhieuDatPhong(int idPhieu)
    {
        var list = await _dbContext.ChiTietDichVus
            .Where(c => c.IdPhieuDatPhong == idPhieu)
            .Include(c => c.DichVu) // Đảm bảo include DichVu để lấy TenDichVu và DonGia
            .Select(c => new ChiTietDichVuDto
            {
                IdChiTietDichVu = c.IdChiTietDichVu,
                IdPhieuDatPhong = c.IdPhieuDatPhong,
                MaDichVu = c.MaDichVu, // MaDichVu là int
                SoLuong = c.SoLuong,
                TenDichVu = c.DichVu.TenDichVu,
                DonGia = c.DichVu.DonGia
            })
            .ToListAsync();

        return Ok(list);
    }

    // ADD ChiTietDichVu
    // POST: /api/ChiTietDichVu
    [HttpPost]
    public async Task<IActionResult> Add(AddChiTietDichVuRequestDto dto)
    {
        // Kiểm tra xem dịch vụ có tồn tại không
        var dichVuExists = await _dbContext.DichVus.AnyAsync(dv => dv.MaDichVu == dto.MaDichVu);
        if (!dichVuExists)
        {
            return BadRequest($"Mã dịch vụ '{dto.MaDichVu}' không tồn tại.");
        }

        var chiTiet = new ChiTietDichVu
        {
            IdPhieuDatPhong = dto.IdPhieuDatPhong,
            MaDichVu = dto.MaDichVu, // MaDichVu là int
            SoLuong = dto.SoLuong
        };

        await _dbContext.ChiTietDichVus.AddAsync(chiTiet);
        await _dbContext.SaveChangesAsync();

        // Trả về DTO của ChiTietDichVu đã thêm (có thể cần truy vấn lại để lấy TenDichVu và DonGia)
        var chiTietDto = await _dbContext.ChiTietDichVus
            .Where(c => c.IdChiTietDichVu == chiTiet.IdChiTietDichVu)
            .Include(c => c.DichVu)
            .Select(c => new ChiTietDichVuDto
            {
                IdChiTietDichVu = c.IdChiTietDichVu,
                IdPhieuDatPhong = c.IdPhieuDatPhong,
                MaDichVu = c.MaDichVu,
                SoLuong = c.SoLuong,
                TenDichVu = c.DichVu.TenDichVu,
                DonGia = c.DichVu.DonGia
            })
            .FirstOrDefaultAsync();

        return CreatedAtAction(nameof(GetByPhieuDatPhong), new { idPhieu = chiTietDto.IdPhieuDatPhong }, chiTietDto);
    }

    // UPDATE ChiTietDichVu
    // PUT: /api/ChiTietDichVu/{id}
    [HttpPut("{id:int}")] // Chỉ rõ id là số nguyên
    public async Task<IActionResult> Update(int id, UpdateChiTietDichVuRequestDto dto)
    {
        var chiTiet = await _dbContext.ChiTietDichVus.FindAsync(id);
        if (chiTiet == null) return NotFound("Không tìm thấy chi tiết dịch vụ.");

        chiTiet.SoLuong = dto.SoLuong;
        await _dbContext.SaveChangesAsync();

        // Trả về DTO đã cập nhật
        var chiTietDto = await _dbContext.ChiTietDichVus
            .Where(c => c.IdChiTietDichVu == chiTiet.IdChiTietDichVu)
            .Include(c => c.DichVu)
            .Select(c => new ChiTietDichVuDto
            {
                IdChiTietDichVu = c.IdChiTietDichVu,
                IdPhieuDatPhong = c.IdPhieuDatPhong,
                MaDichVu = c.MaDichVu,
                SoLuong = c.SoLuong,
                TenDichVu = c.DichVu.TenDichVu,
                DonGia = c.DichVu.DonGia
            })
            .FirstOrDefaultAsync();

        return Ok(chiTietDto);
    }

    // DELETE ChiTietDichVu
    // DELETE: /api/ChiTietDichVu/{id}
    [HttpDelete("{id:int}")] // Chỉ rõ id là số nguyên
    public async Task<IActionResult> Delete(int id)
    {
        var chiTiet = await _dbContext.ChiTietDichVus.FindAsync(id);
        if (chiTiet == null) return NotFound("Không tìm thấy chi tiết dịch vụ để xóa.");

        _dbContext.ChiTietDichVus.Remove(chiTiet);
        await _dbContext.SaveChangesAsync();

        return NoContent(); // Trả về 204 No Content
    }
}