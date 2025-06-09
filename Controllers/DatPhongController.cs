// (Nội dung đã cung cấp ở câu trả lời trước)
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLKS.API.Data;
using QLKS.API.Models.Domain;
using QLKS.API.Models.DTO.DatPhong;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;


[ApiController]
[Route("api/[controller]")]
public class DatPhongController : ControllerBase
{
    private readonly QLKSDbContextcs _dbContext;

    public DatPhongController(QLKSDbContextcs dbContext)
    {
        _dbContext = dbContext;
    }

    // Lấy tất cả lượt đặt phòng
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var datPhongDomain = await _dbContext.DatPhongs.ToListAsync();
        var datPhongDto = datPhongDomain.Select(dp => new DatPhongDto
        {
            IdDatPhong = dp.IdDatPhong,
            IdPhieuDatPhong = dp.IdPhieuDatPhong,
            IdPhong = dp.IdPhong,
            NgayNhanPhong = dp.NgayNhanPhong,
            NgayTraPhong = dp.NgayTraPhong,
            DonGiaPhong = dp.DonGiaPhong,
            SoLuongNguoiLon = dp.SoLuongNguoiLon,
            SoLuongTreEm = dp.SoLuongTreEm,
            TrangThaiDatPhong = dp.TrangThaiDatPhong,
            GhiChu = dp.GhiChu,
            ThoiGianTao = dp.ThoiGianTao
        }).ToList();
        return Ok(datPhongDto);
    }

    // Lấy lượt đặt phòng theo IdDatPhong
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var datPhongDomain = await _dbContext.DatPhongs.FirstOrDefaultAsync(x => x.IdDatPhong == id);
        if (datPhongDomain == null) return NotFound("Không tìm thấy lượt đặt phòng.");

        var datPhongDto = new DatPhongDto
        {
            IdDatPhong = datPhongDomain.IdDatPhong,
            IdPhieuDatPhong = datPhongDomain.IdPhieuDatPhong,
            IdPhong = datPhongDomain.IdPhong,
            NgayNhanPhong = datPhongDomain.NgayNhanPhong,
            NgayTraPhong = datPhongDomain.NgayTraPhong,
            DonGiaPhong = datPhongDomain.DonGiaPhong,
            SoLuongNguoiLon = datPhongDomain.SoLuongNguoiLon,
            SoLuongTreEm = datPhongDomain.SoLuongTreEm,
            TrangThaiDatPhong = datPhongDomain.TrangThaiDatPhong,
            GhiChu = datPhongDomain.GhiChu,
            ThoiGianTao = datPhongDomain.ThoiGianTao
        };
        return Ok(datPhongDto);
    }

    // Lấy các lượt đặt phòng theo IdPhieuDatPhong
    [HttpGet]
    [Route("ByPhieu/{idPhieu:int}")]
    public async Task<IActionResult> GetByPhieuId([FromRoute] int idPhieu)
    {
        var datPhongDomain = await _dbContext.DatPhongs
                                    .Where(dp => dp.IdPhieuDatPhong == idPhieu)
                                    .ToListAsync();
        if (!datPhongDomain.Any()) return NotFound($"Không tìm thấy lượt đặt phòng nào cho phiếu đặt phòng có ID: {idPhieu}");

        var datPhongDtos = datPhongDomain.Select(dp => new DatPhongDto
        {
            IdDatPhong = dp.IdDatPhong,
            IdPhieuDatPhong = dp.IdPhieuDatPhong,
            IdPhong = dp.IdPhong,
            NgayNhanPhong = dp.NgayNhanPhong,
            NgayTraPhong = dp.NgayTraPhong,
            DonGiaPhong = dp.DonGiaPhong,
            SoLuongNguoiLon = dp.SoLuongNguoiLon,
            SoLuongTreEm = dp.SoLuongTreEm,
            TrangThaiDatPhong = dp.TrangThaiDatPhong,
            GhiChu = dp.GhiChu,
            ThoiGianTao = dp.ThoiGianTao
        }).ToList();
        return Ok(datPhongDtos);
    }


    // Thêm mới một lượt đặt phòng
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddDatPhongRequestDto addDatPhongRequestDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var phieuDatPhongExists = await _dbContext.PhieuDatPhongs.AnyAsync(p => p.IdPhieuDatPhong == addDatPhongRequestDto.IdPhieuDatPhong);
        if (!phieuDatPhongExists) return BadRequest($"Phiếu đặt phòng với ID {addDatPhongRequestDto.IdPhieuDatPhong} không tồn tại.");

        var phongExists = await _dbContext.Phongs.AnyAsync(p => p.IdPhong == addDatPhongRequestDto.IdPhong);
        if (!phongExists) return BadRequest($"Phòng với ID {addDatPhongRequestDto.IdPhong} không tồn tại.");

        var isRoomAvailable = await IsRoomAvailable(
            addDatPhongRequestDto.IdPhong,
            addDatPhongRequestDto.NgayNhanPhong,
            addDatPhongRequestDto.NgayTraPhong,
            0 // 0 cho Add mới
        );
        if (!isRoomAvailable) return Conflict("Phòng này đã được đặt hoặc không khả dụng trong khoảng thời gian yêu cầu.");

        var datPhongDomain = new DatPhong
        {
            IdPhieuDatPhong = addDatPhongRequestDto.IdPhieuDatPhong,
            IdPhong = addDatPhongRequestDto.IdPhong,
            NgayNhanPhong = addDatPhongRequestDto.NgayNhanPhong,
            NgayTraPhong = addDatPhongRequestDto.NgayTraPhong,
            DonGiaPhong = addDatPhongRequestDto.DonGiaPhong,
            SoLuongNguoiLon = addDatPhongRequestDto.SoLuongNguoiLon,
            SoLuongTreEm = addDatPhongRequestDto.SoLuongTreEm,
            TrangThaiDatPhong = addDatPhongRequestDto.TrangThaiDatPhong ?? "Đã xác nhận",
            GhiChu = addDatPhongRequestDto.GhiChu,
            ThoiGianTao = DateTime.Now
        };

        await _dbContext.DatPhongs.AddAsync(datPhongDomain);
        await _dbContext.SaveChangesAsync();

        var datPhongDto = new DatPhongDto
        {
            IdDatPhong = datPhongDomain.IdDatPhong,
            IdPhieuDatPhong = datPhongDomain.IdPhieuDatPhong,
            IdPhong = datPhongDomain.IdPhong,
            NgayNhanPhong = datPhongDomain.NgayNhanPhong,
            NgayTraPhong = datPhongDomain.NgayTraPhong,
            DonGiaPhong = datPhongDomain.DonGiaPhong,
            SoLuongNguoiLon = datPhongDomain.SoLuongNguoiLon,
            SoLuongTreEm = datPhongDomain.SoLuongTreEm,
            TrangThaiDatPhong = datPhongDomain.TrangThaiDatPhong,
            GhiChu = datPhongDomain.GhiChu,
            ThoiGianTao = datPhongDomain.ThoiGianTao
        };
        return CreatedAtAction(nameof(GetById), new { id = datPhongDto.IdDatPhong }, datPhongDto);
    }

    // Cập nhật một lượt đặt phòng
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDatPhongRequestDto updateDatPhongRequestDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var datPhongDomain = await _dbContext.DatPhongs.FirstOrDefaultAsync(x => x.IdDatPhong == id);
        if (datPhongDomain == null) return NotFound("Không tìm thấy lượt đặt phòng để cập nhật.");

        if (datPhongDomain.IdPhieuDatPhong != updateDatPhongRequestDto.IdPhieuDatPhong)
        {
            var phieuDatPhongExists = await _dbContext.PhieuDatPhongs.AnyAsync(p => p.IdPhieuDatPhong == updateDatPhongRequestDto.IdPhieuDatPhong);
            if (!phieuDatPhongExists) return BadRequest($"Phiếu đặt phòng với ID {updateDatPhongRequestDto.IdPhieuDatPhong} không tồn tại.");
        }

        if (datPhongDomain.IdPhong != updateDatPhongRequestDto.IdPhong)
        {
            var phongExists = await _dbContext.Phongs.AnyAsync(p => p.IdPhong == updateDatPhongRequestDto.IdPhong);
            if (!phongExists) return BadRequest($"Phòng với ID {updateDatPhongRequestDto.IdPhong} không tồn tại.");
        }

        if (datPhongDomain.IdPhong != updateDatPhongRequestDto.IdPhong ||
            datPhongDomain.NgayNhanPhong != updateDatPhongRequestDto.NgayNhanPhong ||
            datPhongDomain.NgayTraPhong != updateDatPhongRequestDto.NgayTraPhong)
        {
            var isRoomAvailable = await IsRoomAvailable(
                updateDatPhongRequestDto.IdPhong,
                updateDatPhongRequestDto.NgayNhanPhong,
            updateDatPhongRequestDto.NgayTraPhong,
                id // Loại trừ bản ghi hiện tại khi kiểm tra
            );
            if (!isRoomAvailable) return Conflict("Phòng này đã được đặt hoặc không khả dụng trong khoảng thời gian yêu cầu.");
        }

        datPhongDomain.IdPhieuDatPhong = updateDatPhongRequestDto.IdPhieuDatPhong;
        datPhongDomain.IdPhong = updateDatPhongRequestDto.IdPhong;
        datPhongDomain.NgayNhanPhong = updateDatPhongRequestDto.NgayNhanPhong;
        datPhongDomain.NgayTraPhong = updateDatPhongRequestDto.NgayTraPhong;
        datPhongDomain.DonGiaPhong = updateDatPhongRequestDto.DonGiaPhong;
        datPhongDomain.SoLuongNguoiLon = updateDatPhongRequestDto.SoLuongNguoiLon;
        datPhongDomain.SoLuongTreEm = updateDatPhongRequestDto.SoLuongTreEm;
        datPhongDomain.TrangThaiDatPhong = updateDatPhongRequestDto.TrangThaiDatPhong;
        datPhongDomain.GhiChu = updateDatPhongRequestDto.GhiChu;

        await _dbContext.SaveChangesAsync();

        var datPhongDto = new DatPhongDto
        {
            IdDatPhong = datPhongDomain.IdDatPhong,
            IdPhieuDatPhong = datPhongDomain.IdPhieuDatPhong,
            IdPhong = datPhongDomain.IdPhong,
            NgayNhanPhong = datPhongDomain.NgayNhanPhong,
            NgayTraPhong = datPhongDomain.NgayTraPhong,
            DonGiaPhong = datPhongDomain.DonGiaPhong,
            SoLuongNguoiLon = datPhongDomain.SoLuongNguoiLon,
            SoLuongTreEm = datPhongDomain.SoLuongTreEm,
            TrangThaiDatPhong = datPhongDomain.TrangThaiDatPhong,
            GhiChu = datPhongDomain.GhiChu,
            ThoiGianTao = datPhongDomain.ThoiGianTao
        };
        return Ok(datPhongDto);
    }

    // Xóa một lượt đặt phòng
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var datPhongDomain = await _dbContext.DatPhongs.FirstOrDefaultAsync(x => x.IdDatPhong == id);
        if (datPhongDomain == null) return NotFound("Không tìm thấy lượt đặt phòng để xóa.");

        _dbContext.DatPhongs.Remove(datPhongDomain);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    // Hàm kiểm tra phòng trống
    private async Task<bool> IsRoomAvailable(int phongId, DateTime checkInDate, DateTime checkOutDate, int idDatPhongToExclude)
    {
        var overlappingBookings = await _dbContext.DatPhongs
            .Where(dp => dp.IdPhong == phongId &&
                         dp.IdDatPhong != idDatPhongToExclude &&
                         (dp.TrangThaiDatPhong == "Đã xác nhận" || dp.TrangThaiDatPhong == "Đang ở") &&
                         (checkInDate < dp.NgayTraPhong && checkOutDate > dp.NgayNhanPhong))
            .AnyAsync();

        return !overlappingBookings;
    }
}