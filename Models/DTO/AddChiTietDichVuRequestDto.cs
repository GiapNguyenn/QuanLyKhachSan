using System.ComponentModel.DataAnnotations;

namespace QLKS.API.Models.DTO.ChiTietDichVu
{
    public class AddChiTietDichVuRequestDto
    {
        [Required(ErrorMessage = "ID Phiếu đặt phòng là bắt buộc")]
        public int IdPhieuDatPhong { get; set; }

        [Required(ErrorMessage = "Mã dịch vụ là bắt buộc")]
        public int MaDichVu { get; set; } // Thay đổi từ string sang int

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }
    }
}