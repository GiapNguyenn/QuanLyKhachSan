// (Nội dung đã cung cấp ở câu trả lời trước)
using System.ComponentModel.DataAnnotations;

namespace QLKS.API.Models.DTO.DatPhong
{
    public class AddDatPhongRequestDto
    {
        [Required(ErrorMessage = "Mã phiếu đặt phòng là bắt buộc.")]
        public int IdPhieuDatPhong { get; set; }

        [Required(ErrorMessage = "Mã phòng là bắt buộc.")]
        public int IdPhong { get; set; }

        [Required(ErrorMessage = "Ngày nhận phòng là bắt buộc.")]
        [DataType(DataType.DateTime)]
        public DateTime NgayNhanPhong { get; set; }

        [Required(ErrorMessage = "Ngày trả phòng là bắt buộc.")]
        [DataType(DataType.DateTime)]
        public DateTime NgayTraPhong { get; set; }

        [Required(ErrorMessage = "Đơn giá phòng là bắt buộc.")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Đơn giá phòng phải là số dương.")]
        public decimal DonGiaPhong { get; set; }

        [Required(ErrorMessage = "Số lượng người lớn là bắt buộc.")]
        [Range(1, 10, ErrorMessage = "Số lượng người lớn phải từ 1 đến 10.")]
        public int SoLuongNguoiLon { get; set; } = 1;

        [Range(0, 5, ErrorMessage = "Số lượng trẻ em phải từ 0 đến 5.")]
        public int SoLuongTreEm { get; set; } = 0;

        [StringLength(50, ErrorMessage = "Trạng thái đặt phòng không được vượt quá 50 ký tự.")]
        public string? TrangThaiDatPhong { get; set; } = "Đã xác nhận";

        [StringLength(255, ErrorMessage = "Ghi chú không được vượt quá 255 ký tự.")]
        public string? GhiChu { get; set; }
    }
}