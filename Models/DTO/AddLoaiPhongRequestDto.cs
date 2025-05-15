namespace QLKS.API.Models.DTO
{
    public class AddLoaiPhongRequestDto
    {
        public string? TenLoaiPhong { get; set; }
        public string? Meta { get; set; }
        public bool Hide { get; set; }
        public int ThuTuHienThi { get; set; }
        public DateTime DateBegin { get; set; }
    }
}
