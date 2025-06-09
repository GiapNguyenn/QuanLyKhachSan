using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuanLyKhachSan.Models.DTO;
using QuanLyKhachSan.Models.Domain;
using QLKS.API.Data;
using QuanLyKhachSan.helpers;
using QuanLyKhachSan.Models.DTO;
using Microsoft.EntityFrameworkCore;


namespace QuanLyKhachSan.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhieuDatPhongController : Controller
{
 private readonly QLKSDbContextcs _context;

        public PhieuDatPhongController(QLKSDbContextcs context)
        {
            _context = context;
        }

    // GET: api/PhieuDatPhong
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationDto pagination)
    {
        var query = _context.PhieuDatPhongs
    .Include(p => p.KhachHang)
    .Include(p => p.NhanVien)
    .Include(p => p.Phong)
    .AsQueryable();

        var totalItems = await query.CountAsync();

        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        var dtos = items.Select(datPhong => new PhieuDatPhongDto
        {
            IdPhieuDatPhong = datPhong.IdPhieuDatPhong,
            NgayDatPhong = datPhong.NgayDatPhong,
            NgayVao = datPhong.NgayVao,
            NgayRa = datPhong.NgayRa,
            MaNhanPhong = datPhong.MaNhanPhong,
            TinhTrangDatPhong = datPhong.TinhTrangDatPhong,
            TinhTrangThanhToan = datPhong.TinhTrangThanhToan,
            TongTien = datPhong.TongTien,
            Meta = datPhong.Meta,
            Hide = datPhong.Hide,
            Order = datPhong.Order,
            DateBegin = datPhong.DateBegin,
            IdKhachHang = datPhong.IdKhachHang,
            IdNhanVien = datPhong.IdNhanVien,
            IdPhong = datPhong.IdPhong,
            TenPhong = datPhong.Phong?.TenPhong,
            TenKhachHang = datPhong.KhachHang?.HoTen,
            SDT = datPhong.KhachHang?.SDT,
            CCCD = datPhong.KhachHang?.CCCD,
            Email = datPhong.KhachHang?.Email,
            TenNhanVien = datPhong.NhanVien?.HoTen
        }).ToList();

        var pagedResult = new PagedResult<PhieuDatPhongDto>
        {
            Items = dtos,
            TotalRecords = totalItems,  // đổi thành TotalRecords
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize
        };
        return Ok(pagedResult);
    }

        // GET: api/PhieuDatPhong/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.PhieuDatPhongs.FindAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/PhieuDatPhong
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PhieuDatPhong model)
        {
            await _context.PhieuDatPhongs.AddAsync(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.IdPhieuDatPhong }, model);
        }

        // PUT: api/PhieuDatPhong/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PhieuDatPhong model)
        {
            var existing = await _context.PhieuDatPhongs.FindAsync(id);
            if (existing == null) return NotFound();

            _context.Entry(existing).CurrentValues.SetValues(model);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/PhieuDatPhong/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.PhieuDatPhongs.FindAsync(id);
            if (existing == null) return NotFound();

            _context.PhieuDatPhongs.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    [HttpPost("create-from-dto")]
    public async Task<IActionResult> PostPhieuDatPhong([FromBody] PhieuDatPhongDto dto)
    {
        // Kiểm tra dữ liệu đầu vào
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Tìm phòng theo Id
        var phong = await _context.Phongs.FindAsync(dto.IdPhong);
        if (phong == null)
            return NotFound("Không tìm thấy phòng.");

        // Tìm khách hàng theo Id
        var khachHang = await _context.khachHangs.FindAsync(dto.IdKhachHang);
        if (khachHang == null)
            return NotFound("Không tìm thấy khách hàng.");

        // Tìm nhân viên theo Id
        var nhanVien = await _context.nhanViens.FindAsync(dto.IdNhanVien);
        if (nhanVien == null)
            return NotFound("Không tìm thấy nhân viên.");

        // Tính số ngày ở
        var soNgayO = (dto.NgayRa.Date - dto.NgayVao.Date).Days;
        if (soNgayO <= 0)
            return BadRequest("Ngày ra phải sau ngày vào.");

        // Tính tổng tiền (giá gốc - giảm giá) * số ngày
        var giaGoc = phong.GiaPhong ?? 0;
        var giamGia = phong.GiamGia ?? 0;
        var tongTien = Math.Max(giaGoc - giamGia, 0) * soNgayO;

        // Tạo phiếu đặt phòng mới
        var phieu = new PhieuDatPhong
        {
            NgayDatPhong = dto.NgayDatPhong,
            NgayVao = dto.NgayVao,
            NgayRa = dto.NgayRa,
            MaNhanPhong = dto.MaNhanPhong,
            TinhTrangDatPhong = dto.TinhTrangDatPhong,
            TinhTrangThanhToan = dto.TinhTrangThanhToan,
            Meta = dto.Meta,

            IdPhong = phong.IdPhong,
            IdKhachHang = khachHang.IdKhachHang,
            IdNhanVien = nhanVien.IdNhanVien,

            TongTien = tongTien,
            Hide = false,
            Order = 0,
            DateBegin = DateTime.Now
        };

        _context.PhieuDatPhongs.Add(phieu);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Đặt phòng thành công",
            IdPhieuDatPhong = phieu.IdPhieuDatPhong,
            TongTien = phieu.TongTien,
            TenPhong = phong.TenPhong,
            TenKhachHang = khachHang.HoTen,
            TenNhanVien = nhanVien.HoTen
        });
    }
}
