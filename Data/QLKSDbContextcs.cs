using Microsoft.EntityFrameworkCore;
using QLKS.API.Models.Domain;
using QuanLyKhachSan.Models.Domain;

namespace QLKS.API.Data
{
    public class QLKSDbContextcs : DbContext
    {
         public  QLKSDbContextcs(DbContextOptions dbContextOptions) : base(dbContextOptions) 
        { 

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChucVu>().ToTable("ChucVu", "QLKS");
            modelBuilder.Entity<Login>().ToTable("Login", "QLKS");
            modelBuilder.Entity<KhachHang>().ToTable("KhachHang", "QLKS");
            modelBuilder.Entity<NhanVien>().ToTable("NhanVien", "QLKS");
            modelBuilder.Entity<PhieuDatPhong>().ToTable("PhieuDatPhong", "QLKS");
            modelBuilder.Entity<Phong>().ToTable("Phong", "QLKS");
            modelBuilder.Entity<LoaiPhong>().ToTable("LoaiPhong", "QLKS");
            modelBuilder.Entity<TienNghi>().ToTable("TienNghi", "QLKS");
            modelBuilder.Entity<DatPhong>().ToTable("DatPhong", "QLKS");
            modelBuilder.Entity<HoaDon>().ToTable("HoaDon", "QLKS");
        }

        public DbSet<ChucVu> chucVus { get; set; }
        public DbSet<Login> logins { get; set; }   
        public DbSet<KhachHang> khachHangs{ get;set; }  
        public DbSet<NhanVien> nhanViens { get; set; }
        public DbSet<Phong> Phongs { get; set; }
        public DbSet<PhieuDatPhong> PhieuDatPhongs { get; set; }
        public DbSet<LoaiPhong> LoaiPhongs { get; set; }
        public DbSet<TienNghi> TienNghis { get; set; } 
        public DbSet<DatPhong> DatPhongs { get; set; } 
        public DbSet<HoaDon> HoaDons { get; set; }   
    }
}
