using QuanLyKhachSan.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKS.API.Models.Domain
{
    public class HoaDon
    {
        [Key]
        public int IdHoaDon { get; set; }           // Khóa chính
        public decimal TongTien { get; set; }       // Tổng tiền thanh toán
        public string Meta { get; set; }            // Thông tin bổ sung (ghi chú, metadata)
        public bool Hide { get; set; }              // Cờ ẩn (ẩn khỏi danh sách nếu cần)
        public DateTime DateBegin { get; set; }     // Ngày lập hóa đơn
       

        // Khóa ngoại
        [ForeignKey("PhieuDatPhong")]
        public int IdPhieuDatPhong { get; set; }    // FK tới bảng PhieuDatPhong

        // Navigation Property (nếu dùng Entity Framework)
        public virtual PhieuDatPhong PhieuDatPhong { get; set; }
    }
}
