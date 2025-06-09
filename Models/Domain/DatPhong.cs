using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Thêm nếu bạn cần các thuộc tính như [Table]
using QuanLyKhachSan.Models.Domain;

namespace QLKS.API.Models.Domain
{
    // Đảm bảo tên bảng khớp với tên trong database schema (QLKS.DatPhong)
    // Nếu bạn không dùng Fluent API để cấu hình schema, bạn có thể thêm [Table("DatPhong", Schema = "QLKS")]
    public class DatPhong
    {
        [Key] // Đánh dấu đây là khóa chính
        public int IdDatPhong { get; set; }

        // Khóa ngoại đến PhieuDatPhong
        public int IdPhieuDatPhong { get; set; }
        [ForeignKey("IdPhieuDatPhong")]
        public PhieuDatPhong PhieuDatPhong { get; set; } // Navigation property

        // Khóa ngoại đến Phong
        public int IdPhong { get; set; }
        [ForeignKey("IdPhong")]
        public Phong Phong { get; set; } // Navigation property

        [Required]
        public DateTime NgayNhanPhong { get; set; }

        [Required]
        public DateTime NgayTraPhong { get; set; }

        [Required]
        [Column(TypeName = "DECIMAL(18, 2)")] // Đảm bảo kiểu dữ liệu trong DB khớp với bảng SQL
        public decimal DonGiaPhong { get; set; }

        public int SoLuongNguoiLon { get; set; } = 1;

        public int SoLuongTreEm { get; set; } = 0;

        [Required]
        [StringLength(50)] // Giới hạn độ dài chuỗi
        public string TrangThaiDatPhong { get; set; } = "Đã xác nhận";

        [StringLength(255)] // Giới hạn độ dài chuỗi
        public string? GhiChu { get; set; } // Dùng ? nếu cho phép null

        public DateTime ThoiGianTao { get; set; } = DateTime.Now; // Giá trị mặc định trong code
    }
}
