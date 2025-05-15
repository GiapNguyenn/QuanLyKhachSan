using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLKS.API.Data;
using QLKS.API.Models.Domain;
using QLKS.API.Models.DTO;

namespace QLKS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiPhongController : ControllerBase
    {
        private readonly QLKSDbContextcs dbContext;

        public LoaiPhongController(QLKSDbContextcs dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get Data From Database - Domain model
            var loaiPhongDomains = await dbContext.LoaiPhongs.ToListAsync();

            // Map Domain Models to DTOs
            var loaiPhongDto = new List<LoaiPhongDto>();
            foreach (var lp in loaiPhongDomains)
            {
                loaiPhongDto.Add(new LoaiPhongDto()
                {
                    IdLoaiPhong = lp.IdLoaiPhong,
                    TenLoaiPhong = lp.TenLoaiPhong,
                    Meta = lp.Meta,
                    Hide = lp.Hide,
                    ThuTuHienThi = lp.ThuTuHienThi,
                    DateBegin = lp.DateBegin
                });

                // Return DTOs
            }
            return Ok(loaiPhongDto);
        }



        // Get By ID LoaiPhong
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Get LoaiPhong Domain Model From Database
            var loaiPhong = await dbContext.LoaiPhongs.FirstOrDefaultAsync(x => x.IdLoaiPhong == id);
            if (loaiPhong == null)
            {
                return NotFound();
            }

            // Map/Convert LoaiPhong Domain Model to LoaiPhong DTO
            var loaiPhongDto = new LoaiPhongDto()
            {
                IdLoaiPhong = loaiPhong.IdLoaiPhong,
                TenLoaiPhong = loaiPhong.TenLoaiPhong,
                Meta = loaiPhong.Meta,
                Hide = loaiPhong.Hide,
                ThuTuHienThi = loaiPhong.ThuTuHienThi,
                DateBegin = loaiPhong.DateBegin
            };
            return Ok(loaiPhongDto);
        }
        //Post To create new LoaiPhong
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddLoaiPhongRequestDto addLoaiPhongRequestDto)
        {
            // Map or Convert DTP to Domain Model
            var LoaiPhongDomainModel = new LoaiPhong
            {
                TenLoaiPhong = addLoaiPhongRequestDto.TenLoaiPhong ?? "",
                Meta = addLoaiPhongRequestDto.Meta??"",
                Hide = addLoaiPhongRequestDto.Hide,
                ThuTuHienThi = addLoaiPhongRequestDto.ThuTuHienThi,
                DateBegin = addLoaiPhongRequestDto.DateBegin
            };

            // Use Domain Model to create LoaiPhong
            await dbContext.LoaiPhongs.AddAsync(LoaiPhongDomainModel);
            await dbContext.SaveChangesAsync();

            //Map Domain model back to DTO
            var loaiPhongDto = new LoaiPhongDto
            {
                IdLoaiPhong = LoaiPhongDomainModel.IdLoaiPhong,
                TenLoaiPhong = LoaiPhongDomainModel.TenLoaiPhong,
                Meta = LoaiPhongDomainModel.Meta,
                Hide = LoaiPhongDomainModel.Hide,
                ThuTuHienThi = LoaiPhongDomainModel.ThuTuHienThi,
                DateBegin = LoaiPhongDomainModel.DateBegin

            };


            return CreatedAtAction(nameof(GetById), new { id = LoaiPhongDomainModel.IdLoaiPhong }, loaiPhongDto);


        }
        // Update LoaiPhong
        //Put
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateLoaiPhongRequestDto updateLoaiPhongRequestDto)
        {
            var LoaiPhongDomainModel = await dbContext.LoaiPhongs.FirstOrDefaultAsync(x => x.IdLoaiPhong == id);
            if (LoaiPhongDomainModel == null)
            {
                return NotFound();

            }
            //Map DTO to Domain model
            LoaiPhongDomainModel.TenLoaiPhong = updateLoaiPhongRequestDto.TenLoaiPhong ??"";
            LoaiPhongDomainModel.Meta = updateLoaiPhongRequestDto.Meta??"";
            LoaiPhongDomainModel.Hide = updateLoaiPhongRequestDto.Hide;
            LoaiPhongDomainModel.ThuTuHienThi = updateLoaiPhongRequestDto.ThuTuHienThi;
            LoaiPhongDomainModel.DateBegin = updateLoaiPhongRequestDto.DateBegin;

            await dbContext.SaveChangesAsync();

            // Convert Domain model to DTO
            var loaiPhongDto = new LoaiPhongDto()
            {
                IdLoaiPhong = LoaiPhongDomainModel.IdLoaiPhong,
                TenLoaiPhong = LoaiPhongDomainModel.TenLoaiPhong,
                Meta = LoaiPhongDomainModel.Meta,
                Hide = LoaiPhongDomainModel.Hide,
                ThuTuHienThi = LoaiPhongDomainModel.ThuTuHienThi,
                DateBegin = LoaiPhongDomainModel.DateBegin


            };

            return Ok(loaiPhongDto);

        }
        //Delete LoaiPhong
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var loaiPhongDomainModel = await dbContext.LoaiPhongs.FirstOrDefaultAsync(x => x.IdLoaiPhong == id);
            if (loaiPhongDomainModel == null)
            {
                return NotFound();

            }
            dbContext.LoaiPhongs.Remove(loaiPhongDomainModel);
            await dbContext.SaveChangesAsync();

            var loaiPhongDto = new LoaiPhongDto()
            {
                IdLoaiPhong = loaiPhongDomainModel.IdLoaiPhong,
                TenLoaiPhong = loaiPhongDomainModel.TenLoaiPhong,
                Meta = loaiPhongDomainModel.Meta,
                Hide = loaiPhongDomainModel.Hide,
                ThuTuHienThi = loaiPhongDomainModel.ThuTuHienThi,
                DateBegin = loaiPhongDomainModel.DateBegin
            };
            return Ok(loaiPhongDto);
        }




        }
}
