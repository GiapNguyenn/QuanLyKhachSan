using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKhachSan.Models.DTO;

public class PhieuDatPhongDto
{
       public int IdPhieuDatPhong { get; set; }
    public DateTime NgayDatPhong { get; set; }
    public DateTime NgayVao { get; set; }
    public DateTime NgayRa { get; set; }

    public string MaNhanPhong { get; set; }
    public int TinhTrangDatPhong { get; set; }
    public int TinhTrangThanhToan { get; set; }
    public decimal TongTien { get; set; } // Đây là nơi TongTien nên xuất hiện
    public string Meta { get; set; }

    public bool Hide { get; set; }
    public int Order { get; set; }
    public DateTime DateBegin { get; set; }

    public int IdKhachHang { get; set; }
    public int IdNhanVien { get; set; }
    public int? IdPhong { get; set; } // IdPhong là có thể null? DB của bạn là NOT NULL
    public string? TenNhanVien { get; set; }
    public string? TenPhong { get; set; }
    public string? TenKhachHang { get; set; }
    public string? SDT { get; set; }
    public string? CCCD { get; set; }
    public string? Email { get; set; }

}
