using System.ComponentModel.DataAnnotations;
namespace QLKS.API.Models.DTO.DichVu
{
    public class DichVuDto
    {
        public int MaDichVu { get; set; } // Thay đổi kiểu dữ liệu từ string sang int
        public string TenDichVu { get; set; }
        public decimal DonGia { get; set; }
        public string? MoTa { get; set; }
        public string? AnhMinhHoa { get; set; }
    }
}