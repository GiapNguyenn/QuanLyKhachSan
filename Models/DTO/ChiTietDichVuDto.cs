namespace QLKS.API.Models.DTO.ChiTietDichVu
{
    public class ChiTietDichVuDto
    {
        public int IdChiTietDichVu { get; set; }
        public int IdPhieuDatPhong { get; set; }
        public int MaDichVu { get; set; } // Thay đổi từ string sang int
        public int SoLuong { get; set; }
        public string TenDichVu { get; set; } // Vẫn là string vì lấy từ bảng DichVu
        public decimal DonGia { get; set; } // Vẫn là decimal
    }
}