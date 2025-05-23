using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKhachSan.Models.DTO;

public class PhieuDatPhongDto
{
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

        public int IdKhachHang { get; set; }
        public int IdNhanVien { get; set; }
}
