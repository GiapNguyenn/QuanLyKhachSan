using System.ComponentModel.DataAnnotations;

namespace QLKS.API.Models.DTO
{
    public class LoaiPhongDto
    {
        [Key]
        public int IdLoaiPhong { get; set; }
        public string TenLoaiPhong { get; set; }
        public string Meta { get; set; }
        public bool Hide { get; set; }
        public int ThuTuHienThi { get; set; }
        public DateTime DateBegin { get; set; }
    }
}
