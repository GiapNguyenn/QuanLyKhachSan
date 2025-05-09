using Microsoft.EntityFrameworkCore;
using QLKS.API.Models.Domain;

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
        }

        public DbSet<ChucVu> chucVus { get; set; }
        public DbSet<Login> logins { get; set; }   
        public DbSet<KhachHang> khachHangs{ get;set; }  
        public DbSet<NhanVien> nhanViens { get; set; }
    }
}
