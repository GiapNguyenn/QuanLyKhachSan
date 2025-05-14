using QuanLyKhachSan.Models.Domain;
using System.ComponentModel.DataAnnotations;
namespace QLKS.API.Models.Domain
{
    public class Phong
    {
        [Key]
        public int IdPhong { get; set; }
        public string TenPhong { get; set; }
        public decimal? GiaPhong { get; set; }
        public decimal? GiamGia { get; set; }
        public int SoLuong { get; set; }
        public int SoNguoiLon { get; set; }
        public int SoTreEm { get; set; }
        public int DienTich { get; set; }
        public string MoTa { get; set; }
        public string Meta { get; set; }
        public bool Hide { get; set; }
        public int ThuTuSapXep { get; set; }
        public DateTime DateBegin { get; set; }
        public string? HinhAnh { get; set; } 
        public int TrangThai { get; set; }
        public int IdLoaiPhong { get; set; }
    }
}
