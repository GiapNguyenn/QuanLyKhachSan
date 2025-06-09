using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLKS.API.Data;
using QuanLyKhachSan.helpers;
using QuanLyKhachSan.Models.DTO;
using QLKS.API.Models.Domain;
using QLKS.API.Models.DTO;
using System.Globalization;

namespace QLKS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly QLKSDbContextcs dbContext;

        public KhachHangController(QLKSDbContextcs dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationDto pagination)
        {
            var query = dbContext.khachHangs.AsQueryable();

            var pagedResult = await query
                .Select(kh => new KhachHangDto
                {
                    IdKhachHang = kh.IdKhachHang,
                    HoTen = kh.HoTen ?? "",
                    SDT = kh.SDT ?? "",
                    CCCD = kh.CCCD ?? "",
                    Email = kh.Email ?? "",
                    MatKhau = kh.MatKhau ?? "",
                    SaltKey = kh.SaltKey ?? "",
                    Meta = kh.Meta ?? "",
                    Hide = kh.Hide,
                    SapXep = kh.SapXep,
                    DataBegin = kh.DataBegin
                })
                .ToPagedResultAsync(pagination);

            return Ok(pagedResult);
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetByIdKhachHang(int id)
        {
            //Get KhachHang Domain Model From Database
            var khachhang = await dbContext.khachHangs.FirstOrDefaultAsync(x => x.IdKhachHang == id);
            if (khachhang == null)
            {
                return NotFound();
            }
            // Map/Convert KhachHang Domain Model To KhachHang DTO
            var khachHangDto = new KhachHangDto
            {
                IdKhachHang = khachhang.IdKhachHang,
                HoTen = khachhang.HoTen,
                SDT = khachhang.SDT,
                CCCD = khachhang.CCCD,
                Email = khachhang.Email,
                MatKhau = khachhang.MatKhau,
                SaltKey = khachhang.SaltKey,
                Meta = khachhang.Meta,
                Hide = khachhang.Hide,
                SapXep = khachhang.SapXep,
                DataBegin = khachhang.DataBegin,
            };
            return Ok(khachHangDto);    
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchKhachHang([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Vui lòng cung cấp từ khóa tìm kiếm.");
            }

            query = query.ToLower();

            var khachHangs = await dbContext.khachHangs
                .Where(x =>
                    (x.HoTen != null && x.HoTen.ToLower().Contains(query)) ||
                    (x.SDT != null && x.SDT.ToLower().Contains(query)) ||
                    (x.CCCD != null && x.CCCD.ToLower().Contains(query)) ||
                    (x.Email != null && x.Email.ToLower().Contains(query)))
                .Select(khachhang => new KhachHangDto
                {
                    IdKhachHang = khachhang.IdKhachHang,
                    HoTen = khachhang.HoTen,
                    SDT = khachhang.SDT,
                    CCCD = khachhang.CCCD,
                    Email = khachhang.Email,
                    MatKhau = khachhang.MatKhau,
                    SaltKey = khachhang.SaltKey,
                    Meta = khachhang.Meta,
                    Hide = khachhang.Hide,
                    SapXep = khachhang.SapXep,
                    DataBegin = khachhang.DataBegin,
                })
                .ToListAsync();

            return Ok(khachHangs);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddKhachHangRequestDto addKhachHangRequestDto)
        {
            //Map or conver DTP to Domain Model
            var khachHangDomainMoDel = new KhachHang
            {
                HoTen = addKhachHangRequestDto.HoTen,
                SDT = addKhachHangRequestDto.SDT,
                CCCD = addKhachHangRequestDto.CCCD,
                Email = addKhachHangRequestDto.Email,
                MatKhau = addKhachHangRequestDto.MatKhau,
                SaltKey = addKhachHangRequestDto.SaltKey,
                Meta = addKhachHangRequestDto.Meta,
                Hide = addKhachHangRequestDto.Hide,
                SapXep = addKhachHangRequestDto.SapXep,
                DataBegin = addKhachHangRequestDto.DataBegin,
            };
            // Use Domain Maodel to create KhachHang
            await dbContext.khachHangs.AddAsync(khachHangDomainMoDel);
            await dbContext.SaveChangesAsync();
            // Map Doamin Model to create Khach Hang
            var khachhangdto = new KhachHangDto
            {
                IdKhachHang = khachHangDomainMoDel.IdKhachHang,
                HoTen = khachHangDomainMoDel.HoTen,
                SDT = khachHangDomainMoDel.SDT,
                CCCD = khachHangDomainMoDel.CCCD,
                Email = khachHangDomainMoDel.Email,
                MatKhau = khachHangDomainMoDel.MatKhau,
                SaltKey = khachHangDomainMoDel.SaltKey,
                Meta = khachHangDomainMoDel.Meta,
                Hide = khachHangDomainMoDel.Hide,
                SapXep = khachHangDomainMoDel.SapXep,
                DataBegin = khachHangDomainMoDel.DataBegin,
            };

            return CreatedAtAction(nameof(GetByIdKhachHang), new { id = khachHangDomainMoDel.IdKhachHang }, khachhangdto);
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateKhachHangRequestDto updateKhachHangRequestDto)
        {
            var khachHangDomainModel = await dbContext.khachHangs.FirstOrDefaultAsync(x => x.IdKhachHang == id);
            if(khachHangDomainModel == null)
            {
                return NotFound();
            }
            //Map DTO to Dmian Model 
            khachHangDomainModel.HoTen = updateKhachHangRequestDto.HoTen;
            khachHangDomainModel.SDT = updateKhachHangRequestDto.SDT;
            khachHangDomainModel.CCCD = updateKhachHangRequestDto.CCCD;
            khachHangDomainModel.Email = updateKhachHangRequestDto.Email;
            khachHangDomainModel.MatKhau = updateKhachHangRequestDto.MatKhau;
            khachHangDomainModel.SaltKey = updateKhachHangRequestDto.SaltKey;
            khachHangDomainModel.Meta = updateKhachHangRequestDto.Meta;
            khachHangDomainModel.Hide = updateKhachHangRequestDto.Hide;
            khachHangDomainModel.SapXep = updateKhachHangRequestDto.SapXep;
            khachHangDomainModel.DataBegin = updateKhachHangRequestDto.DataBegin;
            await dbContext.SaveChangesAsync();
            // Convert Domain model to DTO
            var khachHangDto = new KhachHangDto()
            {
                IdKhachHang = khachHangDomainModel.IdKhachHang,
                HoTen = khachHangDomainModel.HoTen,
                SDT = khachHangDomainModel.SDT,
                CCCD = khachHangDomainModel.CCCD,
                Email = khachHangDomainModel.Email,
                MatKhau = khachHangDomainModel.MatKhau,
                SaltKey = khachHangDomainModel.SaltKey,
                Meta = khachHangDomainModel.Meta,
                Hide = khachHangDomainModel.Hide,
                SapXep = khachHangDomainModel.SapXep,
                DataBegin = khachHangDomainModel.DataBegin,
            };
            return Ok(khachHangDto);

        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var khachHangDomainModel = await dbContext.khachHangs.FirstOrDefaultAsync(x => x.IdKhachHang == id);
            if (khachHangDomainModel == null)
            {
                return NotFound();

            }
            dbContext.khachHangs.Remove(khachHangDomainModel);
            await dbContext.SaveChangesAsync();
            var khachHangdto = new KhachHangDto()
            {
                IdKhachHang = khachHangDomainModel.IdKhachHang,
                HoTen = khachHangDomainModel.HoTen,
                SDT = khachHangDomainModel.SDT,
                CCCD = khachHangDomainModel.CCCD,
                Email = khachHangDomainModel.Email,
                MatKhau = khachHangDomainModel.MatKhau,
                SaltKey = khachHangDomainModel.SaltKey,
                Meta = khachHangDomainModel.Meta,
                Hide = khachHangDomainModel.Hide,
                SapXep = khachHangDomainModel.SapXep,
                DataBegin = khachHangDomainModel.DataBegin,
            };
            return Ok(khachHangdto);
        }



    }

}
