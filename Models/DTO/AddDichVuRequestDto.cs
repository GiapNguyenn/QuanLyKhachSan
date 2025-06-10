using System.ComponentModel.DataAnnotations;

namespace QLKS.API.Models.DTO.DichVu
{
    public class AddDichVuRequestDto
    {
        // Xóa MaDichVu khỏi DTO này vì nó là IDENTITY trong DB
        // public int MaDichVu { get; set; } // BỎ DÒNG NÀY

        [Required(ErrorMessage = "Tên dịch vụ là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên dịch vụ không được vượt quá 100 ký tự")]
        public string TenDichVu { get; set; }

        [Required(ErrorMessage = "Đơn giá là bắt buộc")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Đơn giá phải là số dương")]
        public decimal DonGia { get; set; }

        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string? MoTa { get; set; }

        [StringLength(255, ErrorMessage = "Đường dẫn ảnh minh họa không được vượt quá 255 ký tự")]
        public string? AnhMinhHoa { get; set; }
    }
}