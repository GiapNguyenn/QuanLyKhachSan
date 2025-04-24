using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace QLKS.API.Models.Domain
{
    [Table("Login", Schema = "QLKS")] 
    public class Login
    {
        [Key]
        public int UserID { get; set; }
        
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        public string? Email { get; set; }
    }
    
}