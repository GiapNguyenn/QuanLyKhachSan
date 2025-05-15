namespace QLKS.API.Models.DTO
{
    public class AddTienNghiRequestDto
    {
        public string TenTienNghi { get; set; } = string.Empty; // not null
        public string? MoTa { get; set; }                      // nullable
        public string? AnhDaiDien { get; set; }                // nullable
        public string? Meta { get; set; }                      // nullable
        public bool? Hide { get; set; }                        // nullable
        public int? ThuTuHienThi { get; set; }                 // nullable
        public DateTime? DateBegin { get; set; }               // nullable
        public int? IdLoaiTienNghi { get; set; }               // nullable
    }
}
