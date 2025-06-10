using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLKS.API.Data;
using QLKS.API.Models.Domain;
using QLKS.API.Models.DTO;

namespace QLKS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly QLKSDbContextcs dbContext;

        public HoaDonController(QLKSDbContextcs dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateHoaDon([FromBody] AddHoaDonRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Lấy thông tin phiếu đặt phòng
            var phieu = await dbContext.PhieuDatPhongs
                .Include(p => p.Phong)
                .Include(p => p.KhachHang)
                .Include(p => p.NhanVien)
                .FirstOrDefaultAsync(p => p.IdPhieuDatPhong == dto.IdPhieuDatPhong);

            if (phieu == null)
                return NotFound("Không tìm thấy phiếu đặt phòng.");

            // Tính số ngày ở
            var soNgay = (phieu.NgayRa - phieu.NgayVao).Days;
            if (soNgay <= 0)
                return BadRequest("Ngày ra phải sau ngày vào.");

            // Tính tổng tiền nếu không được truyền từ client
            var giaPhong = phieu.Phong?.GiaPhong ?? 0;
            var giamGia = phieu.Phong?.GiamGia ?? 0;
            var tongTien = dto.TongTien > 0
                ? dto.TongTien
                : Math.Max(giaPhong - giamGia, 0) * soNgay;

            // Tạo mới hóa đơn
            var hoaDon = new HoaDon
            {
                IdPhieuDatPhong = dto.IdPhieuDatPhong,
                TongTien = tongTien,
                Meta = dto.Meta,
                Hide = dto.Hide,  // ✅ Nếu dto.Hide là bool, cần ép về int
                DateBegin = dto.DateBegin,
            };

            dbContext.HoaDons.Add(hoaDon);
            await dbContext.SaveChangesAsync();

            // ✅ Trả về DTO có kèm trạng thái thanh toán
            return Ok(new HoaDonDto
            {
                IdHoaDon = hoaDon.IdHoaDon,
                TongTien = hoaDon.TongTien,
                Meta = hoaDon.Meta,
                Hide = hoaDon.Hide,
                DateBegin = hoaDon.DateBegin,

                IdPhieuDatPhong = phieu.IdPhieuDatPhong,
                TenPhong = phieu.Phong?.TenPhong,
                TenKhachHang = phieu.KhachHang?.HoTen,
                TenNhanVien = phieu.NhanVien?.HoTen,
                NgayVao = phieu.NgayVao,
                NgayRa = phieu.NgayRa,
                TrangThaiThanhToan = phieu.TinhTrangThanhToan // ✅ Thêm trường này
            });
        }


        [HttpGet("list")]
        public async Task<IActionResult> GetAllHoaDon()
        {
            var hoaDons = await dbContext.HoaDons
                .Include(h => h.PhieuDatPhong)
                    .ThenInclude(p => p.Phong)
                .Include(h => h.PhieuDatPhong.KhachHang)
                .Include(h => h.PhieuDatPhong.NhanVien)
                .Select(h => new HoaDonDto
                {
                    IdHoaDon = h.IdHoaDon,
                    TongTien = h.TongTien,
                    Meta = h.Meta,
                    Hide = h.Hide ,        // <-- Chuyển từ int sang bool                   
                    DateBegin = h.DateBegin,
                    IdPhieuDatPhong = h.IdPhieuDatPhong,
                    TenPhong = h.PhieuDatPhong.Phong.TenPhong,
                    TenKhachHang = h.PhieuDatPhong.KhachHang.HoTen,
                    TenNhanVien = h.PhieuDatPhong.NhanVien.HoTen,
                    NgayVao = h.PhieuDatPhong.NgayVao,
                    NgayRa = h.PhieuDatPhong.NgayRa
                })
                .ToListAsync();

            return Ok(hoaDons);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hoaDon = await dbContext.HoaDons
                .Include(h => h.PhieuDatPhong)
                    .ThenInclude(p => p.KhachHang)
                .Include(h => h.PhieuDatPhong)
                    .ThenInclude(p => p.NhanVien)
                .Include(h => h.PhieuDatPhong)
                    .ThenInclude(p => p.Phong)
                .FirstOrDefaultAsync(h => h.IdHoaDon == id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            var result = new HoaDonDto
            {
                IdHoaDon = hoaDon.IdHoaDon,
                DateBegin = hoaDon.DateBegin,
                
                TongTien = hoaDon.TongTien,
                TenKhachHang = hoaDon.PhieuDatPhong?.KhachHang?.HoTen,
                SDT = hoaDon.PhieuDatPhong?.KhachHang?.SDT,
                Email = hoaDon.PhieuDatPhong?.KhachHang?.Email,
                TenNhanVien = hoaDon.PhieuDatPhong?.NhanVien?.HoTen,
                TenPhong = hoaDon.PhieuDatPhong?.Phong?.TenPhong,
                GiaPhong = hoaDon.PhieuDatPhong?.Phong?.GiaPhong ?? 0
            };

            return Ok(result);
        }





    }
}
