namespace QLKS.API.Models.DTO
{
    public class AddDatPhongRequestDto
    {
        public DateTime ThoiGianNhanPhong { get; set; }
        public DateTime ThoiGianTraPhong { get; set; }
        public string? TrangThai { get; set; }
        public string? GhiChu { get; set; }
        public DateTime? ThoiGianTao { get; set; }
        public int IdKhachHang { get; set; }
        public int IdPhong { get; set; }
    }
}
