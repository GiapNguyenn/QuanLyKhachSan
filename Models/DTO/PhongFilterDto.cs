using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKhachSan.Models.DTO;

public class PhongFilterDto
{
    public int? TrangThai { get; set; }
    public bool? Hide { get; set; }
    public int? IdLoaiPhong { get; set; }
    public decimal? MinGiaPhong { get; set; }
    public decimal? MaxGiaPhong { get; set; }
}
