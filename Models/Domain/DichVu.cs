using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKS.API.Models.Domain
{
    public class DichVu
    {
        [Key] // Đánh dấu MaDichVu là khóa chính
        // Không cần [Required] và [Column(TypeName = "nvarchar(50)")] vì nó là IDENTITY INT
        public int MaDichVu { get; set; } // Thay đổi kiểu dữ liệu từ string sang int

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string TenDichVu { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DonGia { get; set; }

        [Column(TypeName = "nvarchar(255)")] // Kích thước là 255
        public string? MoTa { get; set; }

        [Column(TypeName = "nvarchar(255)")] // Kích thước là 255
        public string? AnhMinhHoa { get; set; }

        // Navigation property cho ChiTietDichVu
        public ICollection<ChiTietDichVu> ChiTietDichVus { get; set; } = new List<ChiTietDichVu>(); // Khởi tạo để tránh null reference
    }
}