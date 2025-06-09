using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QLKS.API.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSan.Models.Domain;

public class PhieuDatPhong
{
    [Key]
    public int IdPhieuDatPhong { get; set; }

    public DateTime NgayDatPhong { get; set; }
    public DateTime NgayVao { get; set; }
    public DateTime NgayRa { get; set; }

    public string MaNhanPhong { get; set; }
    public int TinhTrangDatPhong { get; set; }
    public int TinhTrangThanhToan { get; set; }
    public decimal TongTien { get; set; }
    public string Meta { get; set; }

    public bool Hide { get; set; }
    public int Order { get; set; }
    public DateTime DateBegin { get; set; }

    [ForeignKey("KhachHang")]
    public int IdKhachHang { get; set; }
    public KhachHang? KhachHang { get; set; }
    [ForeignKey("NhanVien")]
    public int IdNhanVien { get; set; }
    public NhanVien? NhanVien { get; set; }
    [ForeignKey("Phong")]
    public int? IdPhong { get; set; }
    public Phong? Phong { get; set; }
}
