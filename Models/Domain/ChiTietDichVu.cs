using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QLKS.API.Models.Domain; 
using QuanLyKhachSan.Models.Domain;

namespace QLKS.API.Models.Domain
{
    public class ChiTietDichVu
    {
        [Key]
        public int IdChiTietDichVu { get; set; }

        public int IdPhieuDatPhong { get; set; }
        [ForeignKey("IdPhieuDatPhong")]
        public PhieuDatPhong PhieuDatPhong { get; set; } // Đảm bảo PhieuDatPhong đã được khai báo

        public int MaDichVu { get; set; } // Thay đổi kiểu dữ liệu từ string sang int
        [ForeignKey("MaDichVu")]
        public DichVu DichVu { get; set; } // Đảm bảo DichVu đã được khai báo

        [Required]
        public int SoLuong { get; set; }
    }
}