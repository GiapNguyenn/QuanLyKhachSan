namespace QLKS.API.Models.DTO
{
    public class AddKhachHangRequestDto
    {
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
