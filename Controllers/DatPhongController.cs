using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLKS.API.Data;
using QLKS.API.Models.Domain;
using QLKS.API.Models.DTO;
using System.Runtime.Intrinsics.Arm;

namespace QLKS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatPhongController : ControllerBase
    {
        private readonly QLKSDbContextcs dbContext;

        public DatPhongController(QLKSDbContextcs dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get Data From Database - Domain model
            var datPhongDomains = await dbContext.DatPhongs.ToListAsync();

            // Map Domain Models to DTOs
            var datPhongDto = new List<DatPhongDto>();
            foreach (var dp in datPhongDomains)
            {
                datPhongDto.Add(new DatPhongDto()
                {
                    MaDatPhong = dp.MaDatPhong,
                    ThoiGianNhanPhong = dp.ThoiGianNhanPhong,
                    ThoiGianTraPhong = dp.ThoiGianTraPhong, 
                    TrangThai = dp.TrangThai,
                    GhiChu = dp.GhiChu, 
                    ThoiGianTao =dp.ThoiGianTao,      
                    IdKhachHang = dp.IdKhachHang,   
                    IdPhong = dp.IdPhong,   
                });

                
            }
            // Return DTOs
            return Ok(datPhongDto);
        }
        // Get By ID Dat Phong
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Get DatPhong Domain Model From Database
            var datPhong = await dbContext.DatPhongs.FirstOrDefaultAsync(x => x.MaDatPhong == id);
            if (datPhong == null)
            {
                return NotFound();
            }

            // Map/Convert DatPhong Domain Model to DatPhongDTO DTO
            var datPhongDto = new DatPhongDto()
            {
                MaDatPhong = datPhong.MaDatPhong,
                ThoiGianNhanPhong = datPhong.ThoiGianNhanPhong,
                ThoiGianTraPhong = datPhong.ThoiGianTraPhong,
                TrangThai = datPhong.TrangThai,
                GhiChu = datPhong.GhiChu,
                ThoiGianTao = datPhong.ThoiGianTao,
                IdKhachHang = datPhong.IdKhachHang,
                IdPhong = datPhong.IdPhong,
            };
            return Ok(datPhongDto);
        }
        //Post To create new datphong
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddDatPhongRequestDto addDatPhongRequestDto)
        {
            // Map or Convert DTP to Domain Model
            var datphongDomainModel = new DatPhong
            {
                ThoiGianNhanPhong = addDatPhongRequestDto.ThoiGianNhanPhong,
                ThoiGianTraPhong = addDatPhongRequestDto.ThoiGianTraPhong,
                TrangThai = addDatPhongRequestDto.TrangThai,
                GhiChu = addDatPhongRequestDto.GhiChu,
                ThoiGianTao = addDatPhongRequestDto.ThoiGianTao,
                IdKhachHang = addDatPhongRequestDto.IdKhachHang,
                IdPhong = addDatPhongRequestDto.IdPhong,
            };

            // Use Domain Model to create DatPhong
            await dbContext.DatPhongs.AddAsync(datphongDomainModel);
            await dbContext.SaveChangesAsync();

            //Map Domain model back to DTO
            var datphongDto = new DatPhongDto
            {
                MaDatPhong = datphongDomainModel.MaDatPhong,
                ThoiGianNhanPhong = datphongDomainModel.ThoiGianNhanPhong,
                ThoiGianTraPhong = datphongDomainModel.ThoiGianTraPhong,
                TrangThai = datphongDomainModel.TrangThai,
                GhiChu = datphongDomainModel.GhiChu,
                ThoiGianTao = datphongDomainModel.ThoiGianTao,
                IdKhachHang = datphongDomainModel.IdKhachHang,
                IdPhong = datphongDomainModel.IdPhong,

            };


            return CreatedAtAction(nameof(GetById), new { id = datphongDomainModel.MaDatPhong }, datphongDto);

        }
        // Update DatPhong
        //Put
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDatPhongRequestDto updateDatPhongRequestDto)
        {
            var datPhongDomainModel = await dbContext.DatPhongs.FirstOrDefaultAsync(x => x.MaDatPhong == id);
            if (datPhongDomainModel == null)
            {
                return NotFound();

            }
            //Map DTO to Domain model
            datPhongDomainModel.ThoiGianNhanPhong = updateDatPhongRequestDto.ThoiGianNhanPhong;
            datPhongDomainModel.ThoiGianTraPhong = updateDatPhongRequestDto.ThoiGianTraPhong;
            datPhongDomainModel.TrangThai = updateDatPhongRequestDto.TrangThai;
            datPhongDomainModel.GhiChu = updateDatPhongRequestDto.GhiChu;
            datPhongDomainModel.ThoiGianTao = updateDatPhongRequestDto.ThoiGianTao;
            datPhongDomainModel.IdKhachHang = updateDatPhongRequestDto.IdKhachHang;
            datPhongDomainModel.IdPhong = updateDatPhongRequestDto.IdPhong;



            await dbContext.SaveChangesAsync();

            // Convert Domain model to DTO
            var datPhongDto = new DatPhongDto()
            {
                MaDatPhong = datPhongDomainModel.MaDatPhong,
                ThoiGianNhanPhong = datPhongDomainModel.ThoiGianNhanPhong,
                ThoiGianTraPhong = datPhongDomainModel.ThoiGianTraPhong,
                TrangThai = datPhongDomainModel.TrangThai,
                GhiChu = datPhongDomainModel.GhiChu,
                ThoiGianTao = datPhongDomainModel.ThoiGianTao,
                IdKhachHang = datPhongDomainModel.IdKhachHang,
                IdPhong = datPhongDomainModel.IdPhong,


            };

            return Ok(datPhongDto);

        }
        //Delete DatPhong
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var datphongDomainModel = await dbContext.DatPhongs.FirstOrDefaultAsync(x => x.MaDatPhong == id);
            if (datphongDomainModel == null)
            {
                return NotFound();

            }
            dbContext.DatPhongs.Remove(datphongDomainModel);
            await dbContext.SaveChangesAsync();

            var datphongDto = new DatPhongDto()
            {
                MaDatPhong = datphongDomainModel.MaDatPhong,
                ThoiGianNhanPhong = datphongDomainModel.ThoiGianNhanPhong,
                ThoiGianTraPhong = datphongDomainModel.ThoiGianTraPhong,
                TrangThai = datphongDomainModel.TrangThai,
                GhiChu = datphongDomainModel.GhiChu,
                ThoiGianTao = datphongDomainModel.ThoiGianTao,
                IdKhachHang = datphongDomainModel.IdKhachHang,
                IdPhong = datphongDomainModel.IdPhong,
            };
            return Ok(datphongDto);

        }

    }
}
