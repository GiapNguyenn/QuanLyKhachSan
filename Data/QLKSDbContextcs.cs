using Microsoft.EntityFrameworkCore;
using QLKS.API.Models.Domain;
using QuanLyKhachSan.Models.Domain;

namespace QLKS.API.Data
{
    public class QLKSDbContextcs : DbContext
    {
        // Để sửa lỗi trong constructor, bạn cần chỉ rõ kiểu cho DbContextOptions.
        // Nó phải là DbContextOptions<TDbContext>
        public QLKSDbContextcs(DbContextOptions<QLKSDbContextcs> dbContextOptions) : base(dbContextOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Các mapping ToTable của bạn vẫn giữ nguyên
            modelBuilder.Entity<ChucVu>().ToTable("ChucVu", "QLKS");
            modelBuilder.Entity<Login>().ToTable("Login", "QLKS");
            modelBuilder.Entity<KhachHang>().ToTable("KhachHang", "QLKS");
            modelBuilder.Entity<NhanVien>().ToTable("NhanVien", "QLKS");
            modelBuilder.Entity<PhieuDatPhong>().ToTable("PhieuDatPhong", "QLKS");
            modelBuilder.Entity<Phong>().ToTable("Phong", "QLKS");
            modelBuilder.Entity<LoaiPhong>().ToTable("LoaiPhong", "QLKS");
            modelBuilder.Entity<TienNghi>().ToTable("TienNghi", "QLKS");
            modelBuilder.Entity<HoaDon>().ToTable("HoaDon", "QLKS");
            modelBuilder.Entity<DichVu>().ToTable("DichVu", "QLKS"); // Đảm bảo đã thêm cái này cho bảng DichVu
            modelBuilder.Entity<ChiTietDichVu>().ToTable("ChiTietDichVu", "QLKS");

        }

        // Các DbSet của bạn vẫn giữ nguyên
        public DbSet<ChucVu> chucVus { get; set; }
        public DbSet<Login> logins { get; set; }
        public DbSet<KhachHang> khachHangs{ get;set; }
        public DbSet<NhanVien> nhanViens { get; set; }
        public DbSet<Phong> Phongs { get; set; }
        public DbSet<PhieuDatPhong> PhieuDatPhongs { get; set; }
        public DbSet<LoaiPhong> LoaiPhongs { get; set; }
        public DbSet<TienNghi> TienNghis { get; set; } 
        public DbSet<HoaDon> HoaDons { get; set; }   
        public DbSet<DichVu> DichVus { get; set; } // Đảm bảo đã thêm DbSet này
        public DbSet<ChiTietDichVu> ChiTietDichVus { get; set; }
    }
}