using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Có thể không cần nếu bạn không dùng logger
using QuanLyKhachSan.Models.DTO; // Đảm bảo namespace này đúng cho DTOs của bạn
using QuanLyKhachSan.Models.Domain; // Đảm bảo namespace này đúng cho Domain Models của bạn
using QLKS.API.Data; // Đảm bảo namespace này đúng cho DbContext của bạn
using QuanLyKhachSan.helpers; // Đảm bảo namespace này đúng cho PagedResult/PaginationDto
using Microsoft.EntityFrameworkCore; // Để dùng Include, CountAsync, ToListAsync

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

        var dtos = items.Select(datPhong => new PhieuDatPhongDto // Sử dụng PhieuDatPhongDto để trả về
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
            TotalRecords = totalItems,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize
        };
        return Ok(pagedResult);
    }

    // GET: api/PhieuDatPhong/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        // Để trả về đầy đủ thông tin như GetAll, bạn nên include các navigation properties
        var item = await _context.PhieuDatPhongs
            .Include(p => p.KhachHang)
            .Include(p => p.NhanVien)
            .Include(p => p.Phong)
            .FirstOrDefaultAsync(p => p.IdPhieuDatPhong == id);

        if (item == null) return NotFound();

        // Chuyển đổi sang DTO trước khi trả về
        var dto = new PhieuDatPhongDto
        {
            IdPhieuDatPhong = item.IdPhieuDatPhong,
            NgayDatPhong = item.NgayDatPhong,
            NgayVao = item.NgayVao,
            NgayRa = item.NgayRa,
            MaNhanPhong = item.MaNhanPhong,
            TinhTrangDatPhong = item.TinhTrangDatPhong,
            TinhTrangThanhToan = item.TinhTrangThanhToan,
            TongTien = item.TongTien,
            Meta = item.Meta,
            Hide = item.Hide,
            Order = item.Order,
            DateBegin = item.DateBegin,
            IdKhachHang = item.IdKhachHang,
            IdNhanVien = item.IdNhanVien,
            IdPhong = item.IdPhong,
            TenPhong = item.Phong?.TenPhong,
            TenKhachHang = item.KhachHang?.HoTen,
            SDT = item.KhachHang?.SDT,
            CCCD = item.KhachHang?.CCCD,
            Email = item.KhachHang?.Email,
            TenNhanVien = item.NhanVien?.HoTen
        };
        return Ok(dto);
    }

    // Phương thức POST chính để đặt phòng, nhận vào AddPhieuDatPhongRequestDto
    // Đã đổi tên từ PostPhieuDatPhong và bỏ route suffix ("create-from-dto")
    [HttpPost] // Đây là phương thức POST mặc định cho "api/PhieuDatPhong"
    public async Task<IActionResult> Create([FromBody] AddPhieuDatPhongRequestDto addDto)
    {
        // ModelState.IsValid đã được xử lý tự động bởi [ApiController] nếu bạn đã cấu hình
        // Nhưng kiểm tra tường minh cũng không thừa nếu bạn muốn kiểm soát thông báo lỗi.
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // --- Bắt đầu xác thực và xử lý logic ---

        // Tìm phòng theo Id
        var phong = await _context.Phongs.FindAsync(addDto.IdPhong);
        if (phong == null)
            return NotFound("Phòng không tồn tại."); // Đổi thông báo lỗi để rõ ràng hơn

        // Tìm khách hàng theo Id
        var khachHang = await _context.khachHangs.FindAsync(addDto.IdKhachHang);
        if (khachHang == null)
            return NotFound("Khách hàng không tồn tại.");

        // Tìm nhân viên theo Id
        var nhanVien = await _context.nhanViens.FindAsync(addDto.IdNhanVien);
        if (nhanVien == null)
            return NotFound("Nhân viên không tồn tại.");

        // Kiểm tra phòng có sẵn trong khoảng thời gian này không
        // Logic kiểm tra phòng trống cần được bổ sung hoặc cải thiện nếu muốn đặt phòng chính xác
        // Ví dụ: kiểm tra xem có PhieuDatPhong nào khác chồng lấn với NgayVao-NgayRa của phòng này không
        var isPhongOccupied = await _context.PhieuDatPhongs
            .AnyAsync(pdp =>
                pdp.IdPhong == addDto.IdPhong &&
                pdp.TinhTrangDatPhong != 3 && // Loại trừ trạng thái 'Đã hủy' hoặc 'Hoàn thành' nếu có
                (
                    (addDto.NgayVao < pdp.NgayRa && addDto.NgayRa > pdp.NgayVao) // Thời gian đặt phòng có chồng lấn
                )
            );

        if (isPhongOccupied)
        {
            return BadRequest("Phòng đã có người đặt hoặc không khả dụng trong khoảng thời gian này.");
        }


        // Tính số ngày ở
        var soNgayO = (addDto.NgayRa.Date - addDto.NgayVao.Date).Days;
        if (soNgayO <= 0)
            return BadRequest("Ngày trả phòng phải sau ngày nhận phòng.");

        // Tính tổng tiền (giá gốc - giảm giá) * số ngày
        var giaGoc = phong.GiaPhong ?? 0;
        var giamGia = phong.GiamGia ?? 0;
        var tongTien = Math.Max(giaGoc - giamGia, 0) * soNgayO;

        // Tạo phiếu đặt phòng mới (Domain Model) từ AddPhieuDatPhongRequestDto
        var phieu = new PhieuDatPhong
        {
            NgayDatPhong = addDto.NgayDatPhong,
            NgayVao = addDto.NgayVao,
            NgayRa = addDto.NgayRa,
            MaNhanPhong = addDto.MaNhanPhong,
            TinhTrangDatPhong = addDto.TinhTrangDatPhong,
            TinhTrangThanhToan = addDto.TinhTrangThanhToan,
            Meta = addDto.Meta,
            Hide = addDto.Hide,
            Order = addDto.Order,
            DateBegin = addDto.DateBegin,

            IdPhong = phong.IdPhong, // Gán Id từ đối tượng Phong tìm được
            IdKhachHang = khachHang.IdKhachHang, // Gán Id từ đối tượng KhachHang tìm được
            IdNhanVien = nhanVien.IdNhanVien,   // Gán Id từ đối tượng NhanVien tìm được

            TongTien = tongTien, // Sử dụng tổng tiền đã tính
        };

        // Lưu vào database
        _context.PhieuDatPhongs.Add(phieu);
        await _context.SaveChangesAsync();

        // Trả về DTO của phiếu đã tạo để frontend có thông tin đầy đủ
        var responseDto = new PhieuDatPhongDto
        {
            IdPhieuDatPhong = phieu.IdPhieuDatPhong,
            NgayDatPhong = phieu.NgayDatPhong,
            NgayVao = phieu.NgayVao,
            NgayRa = phieu.NgayRa,
            MaNhanPhong = phieu.MaNhanPhong,
            TinhTrangDatPhong = phieu.TinhTrangDatPhong,
            TinhTrangThanhToan = phieu.TinhTrangThanhToan,
            TongTien = phieu.TongTien,
            Meta = phieu.Meta,
            Hide = phieu.Hide,
            Order = phieu.Order,
            DateBegin = phieu.DateBegin,
            IdKhachHang = phieu.IdKhachHang,
            IdNhanVien = phieu.IdNhanVien,
            IdPhong = phieu.IdPhong,
            TenPhong = phong.TenPhong,
            TenKhachHang = khachHang.HoTen,
            TenNhanVien = nhanVien.HoTen,
            SDT = khachHang.SDT,
            CCCD = khachHang.CCCD,
            Email = khachHang.Email
        };

        return CreatedAtAction(nameof(GetById), new { id = responseDto.IdPhieuDatPhong }, responseDto);
    }

    // Phương thức PUT và DELETE không cần thay đổi gì lớn
    // PUT: api/PhieuDatPhong/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AddPhieuDatPhongRequestDto updateDto) // Nên nhận DTO để cập nhật
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingPhieu = await _context.PhieuDatPhongs.FindAsync(id);
        if (existingPhieu == null)
            return NotFound();

        // Cập nhật các trường từ DTO vào Domain Model hiện có
        existingPhieu.NgayDatPhong = updateDto.NgayDatPhong;
        existingPhieu.NgayVao = updateDto.NgayVao;
        existingPhieu.NgayRa = updateDto.NgayRa;
        existingPhieu.MaNhanPhong = updateDto.MaNhanPhong;
        existingPhieu.TinhTrangDatPhong = updateDto.TinhTrangDatPhong;
        existingPhieu.TinhTrangThanhToan = updateDto.TinhTrangThanhToan;
        existingPhieu.Meta = updateDto.Meta;
        existingPhieu.Hide = updateDto.Hide;
        existingPhieu.Order = updateDto.Order;
        existingPhieu.DateBegin = updateDto.DateBegin;
        existingPhieu.IdKhachHang = updateDto.IdKhachHang;
        existingPhieu.IdNhanVien = updateDto.IdNhanVien;
        existingPhieu.IdPhong = updateDto.IdPhong;

        // Tính lại tổng tiền nếu ngày hoặc phòng thay đổi
        var phong = await _context.Phongs.FindAsync(updateDto.IdPhong);
        if (phong != null)
        {
            var soNgayO = (updateDto.NgayRa.Date - updateDto.NgayVao.Date).Days;
            var giaGoc = phong.GiaPhong ?? 0;
            var giamGia = phong.GiamGia ?? 0;
            existingPhieu.TongTien = Math.Max(giaGoc - giamGia, 0) * soNgayO;
        }
        else
        {
             // Xử lý trường hợp phòng không tồn tại khi cập nhật
            return BadRequest("Phòng không tồn tại khi cập nhật.");
        }


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
}