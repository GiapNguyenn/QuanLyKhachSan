// Models/Domain/PhieuDatPhong.cs (Phiên bản đã sửa)
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QLKS.API.Models.Domain;
// using System.Collections.Generic; // Có thể không cần nếu không dùng ICollection khác

namespace QuanLyKhachSan.Models.Domain
{
    [Table("PhieuDatPhong")]
    public class PhieuDatPhong
    {
        [Key]
        public int IdPhieuDatPhong { get; set; }

        [Required]
        public DateTime NgayDatPhong { get; set; } // Ngày lập phiếu

        [Required]
        public DateTime NgayVao { get; set; }
        [Required]
        public DateTime NgayRa { get; set; }

        // SỬA ĐỔI Ở ĐÂY:
        // Đặt là 'string' (không 'string?') vì trong DB là 'not null'
        // Đặt MaxLength phù hợp với nvarchar(200) trong DB, hoặc nhỏ hơn
        [Required] // Thêm Required vì trong DB là NOT NULL
        [MaxLength(200)] // Điều chỉnh để khớp với nvarchar(200) trong DB
        public string MaNhanPhong { get; set; } // Đổi từ string? thành string

       
        public int TinhTrangDatPhong { get; set; }
        public int TinhTrangThanhToan { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TongTien { get; set; }

        public string? Meta { get; set; }
        public bool Hide { get; set; }
        public int Order { get; set; }
        public DateTime DateBegin { get; set; }

        [Required]
        [ForeignKey("KhachHang")]
        public int IdKhachHang { get; set; }
        public KhachHang KhachHang { get; set; } // Navigation property

        [Required]
        [ForeignKey("NhanVien")]
        public int IdNhanVien { get; set; }
        public NhanVien NhanVien { get; set; } // Navigation property

        [Required] // Giữ IdPhong ở đây vì mỗi phiếu đặt phòng giờ là cho 1 phòng duy nhất
        [ForeignKey("Phong")]
        public int IdPhong { get; set; }
        public Phong Phong { get; set; } // Navigation property

        // Các dòng comment out đã được loại bỏ để code gọn hơn
    }
}