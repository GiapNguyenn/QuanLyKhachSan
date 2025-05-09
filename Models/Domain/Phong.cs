namespace QLKS.API.Models.Domain
{
    public class Phong
    {
        public int idPhong {  get; set; }
        public string TenPhong { get; set; }
        public decimal GiaPhong { get; set; }
        public decimal GiamGia { get; set; }
        public int SoLuong { get; set; }
        public int SoNguoiLon { get; set; }
        public int SoTreEm { get;set; }
        public int DienTich { get; set; }
        public string MoTa { get; set; }
        public string Meta { get; set; }
        public bool Hide { get; set; }
        public int ThuTuSapXep { get; set; }
        public DateTime DateBegin { get; set; }
        public string HinhAnh1 { get; set; }
        public string HinhAnh2 { get; set; }
        public string HinhAnh3 { get; set; }
        public string HinhAnh4 { get; set; }
        public string HinhAnh5 { get; set; }
        public int IdLoaiPhong { get; set; }
    }
}
