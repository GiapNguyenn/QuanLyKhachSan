using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace QLKS.API.Models.Domain
{
    [Table("ChucVu")]
    public class ChucVu
    {
        [Key]
        public int IdChucVu { get; set; }
        public string TenChucVu { get; set; }         
        public string Meta { get; set; }    
        public bool Hide { get; set; }  
        public int ThuTuHienThi { get; set; }   
        public DateTime DateBegin { get; set; }         
    }
}
