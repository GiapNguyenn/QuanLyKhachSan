using System.ComponentModel.DataAnnotations;
using QuanLyKhachSan.Models.Domain;

namespace QLKS.API.Models.Domain
{
    public class KhachHang
    {
        [Key]
        public int IdKhachHang { get; set; }
        public string? HoTen { get; set; }
        public string? SDT { get; set; }
        public string? CCCD { get; set; }
        public string? Email { get; set; }
        public string? MatKhau { get; set; }
        public string? SaltKey { get; set; }
        public string? Meta { get; set; }
        public bool Hide { get; set; }
        public int SapXep { get; set; }
        public DateTime DataBegin { get; set; }
    }
}
