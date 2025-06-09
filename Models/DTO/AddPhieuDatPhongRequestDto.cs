// Models/DTO/AddPhieuDatPhongRequestDto.cs (Phiên bản đã sửa)
using System;
using System.ComponentModel.DataAnnotations; // Thêm để dùng [Required], [MaxLength]

namespace QuanLyKhachSan.Models.DTO;

public class AddPhieuDatPhongRequestDto
{
    [Required] // Nên là bắt buộc
    public DateTime NgayDatPhong { get; set; }
    [Required]
    public DateTime NgayVao { get; set; }
    [Required]
    public DateTime NgayRa { get; set; }

    [Required] // Vì trong DB là NOT NULL và string
    [MaxLength(200)] // Khớp với DB
    public string MaNhanPhong { get; set; }

    [Required] // Vì trong DB là NOT NULL và int
    public int TinhTrangDatPhong { get; set; }

    [Required] // Vì trong DB là NOT NULL và int
    public int TinhTrangThanhToan { get; set; }

    // KHÔNG CẦN TongTien ở đây, để backend tính toán
    // public decimal TongTien { get; set; }

    public string? Meta { get; set; } // Có thể nullable
    public bool Hide { get; set; }
    public int Order { get; set; }
    public DateTime DateBegin { get; set; }

    [Required]
    public int IdKhachHang { get; set; }
    [Required]
    public int IdNhanVien { get; set; }

    // THÊM IdPhong vào đây!
    [Required]
    public int IdPhong { get; set; }
}