namespace QLKS.API.Models.DTO
{
    public class UpdateNhanVienRequestDto
    {
        public string? HoTen { get; set; }
        public string? SDT { get; set; }
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
        public string? MatKhau { get; set; }
        public string? SaltKey { get; set; }
        public string? Meta { get; set; }
        public bool Hide { get; set; }
        public int SapXep { get; set; }
        public DateTime DateBegin { get; set; }
        public int IdChucVu { get; set; }
    }
}
