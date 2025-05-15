using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLKS.API.Data;
using QLKS.API.Models.Domain;
using QLKS.API.Models.DTO;
using QuanLyKhachSan.helpers;
using QuanLyKhachSan.Models.DTO;
using System.Globalization;

namespace QLKS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly QLKSDbContextcs dbContext;

        public NhanVienController(QLKSDbContextcs dbContext)
        {
            this.dbContext = dbContext;
        }
        
       [HttpGet]
        public async Task<IActionResult> GetAllNhanVien([FromQuery] PaginationDto pagination)
        {
            var query = dbContext.nhanViens.AsQueryable();

            var pagedResult = await query
                .Select(nv => new NhanVienDto
                {
                    IdNhanVien = nv.IdNhanVien,
                    HoTen = nv.HoTen ?? "",
                    SDT = nv.SDT ?? "",
                    DiaChi = nv.DiaChi ?? "",
                    Email = nv.Email ?? "",
                    MatKhau = nv.MatKhau ?? "",
                    SaltKey = nv.SaltKey ?? "",
                    Meta = nv.Meta ?? "",
                    Hide = nv.Hide,
                    SapXep = nv.SapXep,
                    DateBegin = nv.DateBegin,
                    IdChucVu = nv.IdChucVu
                })
                .ToPagedResultAsync(pagination);

            return Ok(pagedResult);
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetByIdNhanVien(int id)
        {
            //Get KhachHang Domain Model From Database
            var nhanvien = await dbContext.nhanViens.FirstOrDefaultAsync(x => x.IdNhanVien == id);
            if (nhanvien == null)
            {
                return NotFound();
            }
            // Map/Convert KhachHang Domain Model To Nhan Vien DTO
            var nhanVienDto = new NhanVienDto
            {
                IdNhanVien = nhanvien.IdNhanVien,
                HoTen = nhanvien.HoTen,
                SDT = nhanvien.SDT,
                DiaChi = nhanvien.DiaChi,
                Email = nhanvien.Email,
                MatKhau = nhanvien.MatKhau,
                SaltKey = nhanvien.SaltKey,
                Meta = nhanvien.Meta,
                Hide = nhanvien.Hide,
                SapXep = nhanvien.SapXep,
                DateBegin = nhanvien.DateBegin,
                IdChucVu = nhanvien.IdChucVu,
            };
            return Ok(nhanVienDto);
        }
        //Post To create new NhanVien
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddNhanVienRequestDto addNhanVienRequestDto)
        {
            // Map or Convert DTP to Domain Model
            var nhanVienDomainModel = new NhanVien
            {
                HoTen = addNhanVienRequestDto.HoTen,
                SDT = addNhanVienRequestDto.SDT,
                DiaChi = addNhanVienRequestDto.DiaChi,
                Email = addNhanVienRequestDto.Email,
                MatKhau = addNhanVienRequestDto.MatKhau,
                SaltKey = addNhanVienRequestDto.SaltKey,
                Meta = addNhanVienRequestDto.Meta,
                Hide = addNhanVienRequestDto.Hide,
                SapXep = addNhanVienRequestDto.SapXep,
                DateBegin = addNhanVienRequestDto.DateBegin,
                IdChucVu = addNhanVienRequestDto.IdChucVu,
            };

            // Use Domain Model to create chucvu
            await dbContext.nhanViens.AddAsync(nhanVienDomainModel);
            await dbContext.SaveChangesAsync();

            //Map Domain model back to DTO
            var nhanVienDto = new NhanVienDto
            {
                IdNhanVien = nhanVienDomainModel.IdNhanVien,
                HoTen = nhanVienDomainModel.HoTen,
                SDT = nhanVienDomainModel.SDT,
                DiaChi = nhanVienDomainModel.DiaChi,
                Email = nhanVienDomainModel.Email,
                MatKhau = nhanVienDomainModel.MatKhau,
                SaltKey = nhanVienDomainModel.SaltKey,
                Meta = nhanVienDomainModel.Meta,
                Hide = nhanVienDomainModel.Hide,
                SapXep = nhanVienDomainModel.SapXep,
                DateBegin = nhanVienDomainModel.DateBegin,
                IdChucVu = nhanVienDomainModel.IdChucVu,

            };


            return CreatedAtAction(nameof(GetByIdNhanVien), new { id = nhanVienDomainModel.IdNhanVien }, nhanVienDto);

        }
        // Update NhanVien
        //Put
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateNhanVienRequestDto updateNhanVienRequestDto)
        {
            var nhanVienDomainModel = await dbContext.nhanViens.FirstOrDefaultAsync(x => x.IdNhanVien == id);
            if (nhanVienDomainModel == null)
            {
                return NotFound();

            }
            //Map DTO to Domain model

            nhanVienDomainModel.HoTen = updateNhanVienRequestDto.HoTen;
            nhanVienDomainModel.SDT = updateNhanVienRequestDto.SDT;
            nhanVienDomainModel.DiaChi = updateNhanVienRequestDto.DiaChi;
            nhanVienDomainModel.Email = updateNhanVienRequestDto.Email;
            nhanVienDomainModel.MatKhau = updateNhanVienRequestDto.MatKhau;
            nhanVienDomainModel.SaltKey = updateNhanVienRequestDto.SaltKey;
            nhanVienDomainModel.Meta = updateNhanVienRequestDto.Meta;
            nhanVienDomainModel.Hide = updateNhanVienRequestDto.Hide;
            nhanVienDomainModel.SapXep = updateNhanVienRequestDto.SapXep;
            nhanVienDomainModel.DateBegin = updateNhanVienRequestDto.DateBegin;
            nhanVienDomainModel.IdChucVu = updateNhanVienRequestDto.IdChucVu;

            await dbContext.SaveChangesAsync();

            // Convert Domain model to DTO
            var nhanVienDto = new NhanVienDto()
            {
                IdNhanVien = nhanVienDomainModel.IdNhanVien,
                HoTen = nhanVienDomainModel.HoTen,
                SDT = nhanVienDomainModel.SDT,
                DiaChi = nhanVienDomainModel.DiaChi,
                Email = nhanVienDomainModel.Email,
                MatKhau = nhanVienDomainModel.MatKhau,
                SaltKey = nhanVienDomainModel.SaltKey,
                Meta = nhanVienDomainModel.Meta,
                Hide = nhanVienDomainModel.Hide,
                SapXep = nhanVienDomainModel.SapXep,
                DateBegin = nhanVienDomainModel.DateBegin,
                IdChucVu = nhanVienDomainModel.IdChucVu,


            };

            return Ok(nhanVienDto);

        }
        //Delete NhanVien
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var nhanVienDomainModel = await dbContext.nhanViens.FirstOrDefaultAsync(x => x.IdNhanVien == id);
            if (nhanVienDomainModel == null)
            {
                return NotFound();

            }
            dbContext.nhanViens.Remove(nhanVienDomainModel);
            await dbContext.SaveChangesAsync();

            var nhanVienDto = new NhanVienDto()
            {
                IdNhanVien = nhanVienDomainModel.IdNhanVien,
                HoTen = nhanVienDomainModel.HoTen,
                SDT = nhanVienDomainModel.SDT,
                DiaChi = nhanVienDomainModel.DiaChi,
                Email = nhanVienDomainModel.Email,
                MatKhau = nhanVienDomainModel.MatKhau,
                SaltKey = nhanVienDomainModel.SaltKey,
                Meta = nhanVienDomainModel.Meta,
                Hide = nhanVienDomainModel.Hide,
                SapXep = nhanVienDomainModel.SapXep,
                DateBegin = nhanVienDomainModel.DateBegin,
                IdChucVu = nhanVienDomainModel.IdChucVu,
            };
            return Ok(nhanVienDto);

        }

    }
}
