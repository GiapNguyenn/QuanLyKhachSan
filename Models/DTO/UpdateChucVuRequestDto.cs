namespace QLKS.API.Models.DTO
{
    public class UpdateChucVuRequestDto
    {
        public string TenChucVu { get; set; }
        public string Meta { get; set; }
        public bool Hide { get; set; }
        public int ThuTuHienThi { get; set; }
        public DateTime DateBegin { get; set; }
    }
}
