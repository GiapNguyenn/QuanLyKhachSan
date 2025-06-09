using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QLKS.API.Models.Domain;
using QLKS.API.Data;
using QuanLyKhachSan.helpers;
using QuanLyKhachSan.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace QuanLyKhachSan.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhongController : ControllerBase
{
    private readonly QLKSDbContextcs _context;

    public PhongController(QLKSDbContextcs context)
    {
        _context = context;
    }

    [HttpGet("filter")]
    public async Task<IActionResult> Filter([FromQuery] PhongFilterDto filter, [FromQuery] PaginationDto pagination)
    {
        var query = _context.Phongs.AsQueryable();
        var result = await PhongQueryHelper.GetPagedResultAsync(query, filter, pagination);
        return Ok(result);
    }
   [HttpGet("with-status")]
    public async Task<IActionResult> GetPhongsWithStatus()
    {
        var phongList = await _context.Phongs
            // .Include(p => p.LoaiPhong) // Bạn không cần Include LoaiPhong nếu chỉ lấy giá từ Phong
                                       // Tuy nhiên, nếu bạn muốn lấy TenLoaiPhong thì vẫn cần Include!
            .Select(p => new
            {
                p.IdPhong,
                p.TenPhong,
                p.IdLoaiPhong,
                p.TrangThai,
                p.HinhAnh,
                // Lấy tên loại phòng từ thuộc tính điều hướng (nếu LoaiPhong tồn tại và được include)
                LoaiPhong = p.LoaiPhong != null ? p.LoaiPhong.TenLoaiPhong : "N/A", // Đảm bảo LoaiPhong không null
                // LẤY GIÁ TRỰC TIẾP TỪ PHÒNG
                Gia = p.GiaPhong, // <<< THAY ĐỔI Ở ĐÂY!
                IsBooked = _context.DatPhongs.Any(dp =>
                    dp.IdPhong == p.IdPhong &&
                    (dp.TrangThaiDatPhong == "Đã xác nhận" || dp.TrangThaiDatPhong == "Đang ở")
                )
            })
            .ToListAsync();

        return Ok(phongList);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var phong = await _context.Phongs.FindAsync(id);
        if (phong == null) return NotFound();
        return Ok(phong);
    }
     [HttpGet("search")]
    public async Task<IActionResult> SearchByName([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return BadRequest("Từ khóa tìm kiếm không được để trống.");

        var result = await _context.Phongs
            .Where(p => EF.Functions.Like(p.TenPhong, $"%{keyword}%"))
            .ToListAsync();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Phong phong)
    {
        await _context.Phongs.AddAsync(phong);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = phong.IdPhong }, phong);
    }
    [HttpPost("{id}/upload-image")]
    public async Task<IActionResult> UploadImage(int id, IFormFile image)
    {
        var phong = await _context.Phongs.FindAsync(id);
        if (phong == null) return NotFound();

        try
        {
            var imageUrl = await ImageUploadHelper.SaveImageAsync(image, "uploads/phongs");
            phong.HinhAnh = imageUrl;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Upload thành công", imageUrl });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Phong phong)
    {
        var existing = await _context.Phongs.FindAsync(id);
        if (existing == null) return NotFound();

        // update fields
        _context.Entry(existing).CurrentValues.SetValues(phong);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var phong = await _context.Phongs.FindAsync(id);
        if (phong == null) return NotFound();

        _context.Phongs.Remove(phong);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}
