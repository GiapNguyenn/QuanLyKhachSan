// (Nội dung đã cung cấp ở câu trả lời trước)
using System;

namespace QLKS.API.Models.DTO.DatPhong
{
    public class DatPhongDto
    {
        public int IdDatPhong { get; set; }
        public int IdPhieuDatPhong { get; set; }
        public int IdPhong { get; set; }
        public DateTime NgayNhanPhong { get; set; }
        public DateTime NgayTraPhong { get; set; }
        public decimal DonGiaPhong { get; set; }
        public int SoLuongNguoiLon { get; set; }
        public int SoLuongTreEm { get; set; }
        public string TrangThaiDatPhong { get; set; }
        public string? GhiChu { get; set; }
        public DateTime ThoiGianTao { get; set; }
    }
}