using System.ComponentModel.DataAnnotations;

namespace QLKS.API.Models.DTO
{
    public class HoaDonDto
    {
        [Key]
        public int IdHoaDon { get; set; }
        public decimal TongTien { get; set; }
        public string Meta { get; set; }
        public bool Hide { get; set; }
        public DateTime DateBegin { get; set; }

        // Thông tin từ Phiếu Đặt Phòng (Foreign Key)
        public int IdPhieuDatPhong { get; set; }

        // Các thuộc tính mở rộng để hiển thị thuận tiện
        public string TenPhong { get; set; }
        public string LoaiPhong { get; set; }
        public string SDT { get; set; }
        public string? Email { get; set; }
        public decimal GiaPhong { get; set; }
        public string TenNhanVien { get; set; } 
        public string TenKhachHang { get; set; }
        public DateTime NgayVao { get; set; }
        public DateTime NgayRa { get; set; }
        public int TrangThaiThanhToan { get; set; }
    }
}
