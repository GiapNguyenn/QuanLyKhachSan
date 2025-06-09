using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLKS.API.Data;
using QLKS.API.Models.Domain;
using QLKS.API.Models.DTO;
using QuanLyKhachSan.helpers;
using QuanLyKhachSan.Models.DTO;

namespace QLKS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TienNghiController : ControllerBase
    {
        private readonly QLKSDbContextcs dbContext;
        public TienNghiController(QLKSDbContextcs dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationDto pagination)
        {
            var query = dbContext.TienNghis.AsQueryable();

            var pagedResult = await query
                .Select(tiennghi => new TienNghiDto
                {
                    Id = tiennghi.Id,
                    TenTienNghi = tiennghi.TenTienNghi,
                    MoTa = tiennghi.MoTa ?? "",
                    AnhDaiDien = tiennghi.AnhDaiDien ?? "",
                    Meta = tiennghi.Meta ?? "",
                    Hide = tiennghi.Hide ?? false,
                    ThuTuHienThi = tiennghi.ThuTuHienThi ?? 0,
                    DateBegin = tiennghi.DateBegin ?? DateTime.MinValue,
                    IdLoaiTienNghi = tiennghi.IdLoaiTienNghi ?? 0
                })
                .ToPagedResultAsync(pagination);

            return Ok(pagedResult);
        }
        // Get By ID TienNghi
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Get TienNghi Domain Model From Database
            var tiennghi = await dbContext.TienNghis.FirstOrDefaultAsync(x => x.Id == id);
            if (tiennghi == null)
            {
                return NotFound();
            }

            // Map/Convert TienNghi Domain Model to TienNghi DTO
            var tienNghiDto = new TienNghiDto
            {
                Id = tiennghi.Id,
                TenTienNghi = tiennghi.TenTienNghi,
                MoTa = tiennghi.MoTa ?? "",
                AnhDaiDien = tiennghi.AnhDaiDien ?? "",
                Meta = tiennghi.Meta ?? "",
                Hide = tiennghi.Hide ?? false,
                ThuTuHienThi = tiennghi.ThuTuHienThi ?? 0,
                DateBegin = tiennghi.DateBegin ?? DateTime.MinValue,
                IdLoaiTienNghi = tiennghi.IdLoaiTienNghi ?? 0
            };
            return Ok(tienNghiDto);
        }
        //Post To create new TienNghi
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddTienNghiRequestDto addTienNghiRequestDto)
        {
            // Map or Convert DTP to Domain Model
            var tienNghiDomainModel = new TienNghi
            {
                TenTienNghi = addTienNghiRequestDto.TenTienNghi,
                MoTa = addTienNghiRequestDto.MoTa ?? "",
                AnhDaiDien = addTienNghiRequestDto.AnhDaiDien ?? "",
                Meta = addTienNghiRequestDto.Meta ?? "",
                Hide = addTienNghiRequestDto.Hide ?? false,
                ThuTuHienThi = addTienNghiRequestDto.ThuTuHienThi ?? 0,
                DateBegin = addTienNghiRequestDto.DateBegin ?? DateTime.MinValue,
                IdLoaiTienNghi = addTienNghiRequestDto.IdLoaiTienNghi ?? 0
            };

            // Use Domain Model to create TienNghi
            await dbContext.TienNghis.AddAsync(tienNghiDomainModel);
            await dbContext.SaveChangesAsync();

            //Map Domain model back to DTO
            var tiennghiDto = new TienNghiDto
            {
                Id = tienNghiDomainModel.Id,
                TenTienNghi = tienNghiDomainModel.TenTienNghi,
                MoTa = tienNghiDomainModel.MoTa ?? "",
                AnhDaiDien = tienNghiDomainModel.AnhDaiDien ?? "",
                Meta = tienNghiDomainModel.Meta ?? "",
                Hide = tienNghiDomainModel.Hide ?? false,
                ThuTuHienThi = tienNghiDomainModel.ThuTuHienThi ?? 0,
                DateBegin = tienNghiDomainModel.DateBegin ?? DateTime.MinValue,
                IdLoaiTienNghi = tienNghiDomainModel.IdLoaiTienNghi ?? 0

            };


            return CreatedAtAction(nameof(GetById), new { id = tienNghiDomainModel.Id }, tiennghiDto);

        }
        // Update TienNghi
        //Put
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTienNghhiRequestDto updateTienNghhiRequestDto)
        {
            var tienNghiDomainModel = await dbContext.TienNghis.FirstOrDefaultAsync(x => x.Id == id);
            if (tienNghiDomainModel == null)
            {
                return NotFound();

            }
            //Map DTO to Domain model
            tienNghiDomainModel.TenTienNghi = updateTienNghhiRequestDto.TenTienNghi;
            tienNghiDomainModel.MoTa = updateTienNghhiRequestDto.MoTa;
            tienNghiDomainModel.AnhDaiDien = updateTienNghhiRequestDto.AnhDaiDien;
            tienNghiDomainModel.Meta = updateTienNghhiRequestDto.Meta;
            tienNghiDomainModel.Hide = updateTienNghhiRequestDto.Hide;
            tienNghiDomainModel.ThuTuHienThi = updateTienNghhiRequestDto.ThuTuHienThi;
            tienNghiDomainModel.DateBegin = updateTienNghhiRequestDto.DateBegin;
            tienNghiDomainModel.IdLoaiTienNghi = updateTienNghhiRequestDto.IdLoaiTienNghi;

            await dbContext.SaveChangesAsync();

            // Convert Domain model to DTO
            var tiennghiDto = new TienNghiDto()
            {
                Id = tienNghiDomainModel.Id,
                TenTienNghi = tienNghiDomainModel.TenTienNghi,
                MoTa = tienNghiDomainModel.MoTa ?? "",
                AnhDaiDien = tienNghiDomainModel.AnhDaiDien ?? "",
                Meta = tienNghiDomainModel.Meta ?? "",
                Hide = tienNghiDomainModel.Hide ?? false,
                ThuTuHienThi = tienNghiDomainModel.ThuTuHienThi ?? 0,
                DateBegin = tienNghiDomainModel.DateBegin ?? DateTime.MinValue,
                IdLoaiTienNghi = tienNghiDomainModel.IdLoaiTienNghi ?? 0


            };

            return Ok(tiennghiDto);

        }
        //Delete TienNghi
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var tiennghiDomainModel = await dbContext.TienNghis.FirstOrDefaultAsync(x => x.Id == id);
            if (tiennghiDomainModel == null)
            {
                return NotFound();

            }
            dbContext.TienNghis.Remove(tiennghiDomainModel);
            await dbContext.SaveChangesAsync();

            var tiennghiDto = new TienNghiDto()
            {
                Id = tiennghiDomainModel.Id,
                TenTienNghi = tiennghiDomainModel.TenTienNghi,
                MoTa = tiennghiDomainModel.MoTa ?? "",
                AnhDaiDien = tiennghiDomainModel.AnhDaiDien ?? "",
                Meta = tiennghiDomainModel.Meta ?? "",
                Hide = tiennghiDomainModel.Hide ?? false,
                ThuTuHienThi = tiennghiDomainModel.ThuTuHienThi ?? 0,
                DateBegin = tiennghiDomainModel.DateBegin ?? DateTime.MinValue,
                IdLoaiTienNghi = tiennghiDomainModel.IdLoaiTienNghi ?? 0
            };
            return Ok(tiennghiDto);

        }

    }
}
