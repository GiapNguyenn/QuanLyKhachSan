using System.ComponentModel.DataAnnotations;

namespace QLKS.API.Models.DTO
{
    public class AddHoaDonRequestDto
    {

        public decimal TongTien { get; set; } // nếu = 0 thì server sẽ tính
        public string Meta { get; set; }
        public bool Hide { get; set; }
        public bool HienThi { get; set; }
        public DateTime DateBegin { get; set; }

        [Required]
        public int IdPhieuDatPhong { get; set; }

    }
}
