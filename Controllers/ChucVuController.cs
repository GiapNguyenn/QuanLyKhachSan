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
    public class ChucVuController : ControllerBase
    {
        private readonly QLKSDbContextcs dbContext;

        public ChucVuController(QLKSDbContextcs dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get Data From Database - Domain model
            var chucVuDomains = await dbContext.chucVus.ToListAsync();

            // Map Domain Models to DTOs
            var chucvuDto = new List<ChucVuDto>();
            foreach (var chuvuDomain in chucVuDomains)
            {
                chucvuDto.Add(new ChucVuDto()
                {
                    IdChucVu = chuvuDomain.IdChucVu,
                    TenChucVu = chuvuDomain.TenChucVu,
                    Meta = chuvuDomain.Meta,
                    Hide = chuvuDomain.Hide,
                    ThuTuHienThi = chuvuDomain.ThuTuHienThi,
                    DateBegin = chuvuDomain.DateBegin
                });

                // Return DTOs
            }
                return Ok(chucvuDto);        
        }
        // Get By ID Chuc Vu
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Get ChuVu Domain Model From Database
            var chucvu = await dbContext.chucVus.FirstOrDefaultAsync(x => x.IdChucVu == id);
            if (chucvu == null)
            {
                return NotFound();
            }

            // Map/Convert Chucvu Domain Model to ChucVu DTO
            var chucvuDto = new ChucVuDto
            {
                IdChucVu = chucvu.IdChucVu,
                TenChucVu = chucvu.TenChucVu,
                Meta = chucvu.Meta,
                Hide = chucvu.Hide,
                ThuTuHienThi = chucvu.ThuTuHienThi,
                DateBegin = chucvu.DateBegin
            };
            return Ok(chucvuDto);
        }
        //Post To create new ChucVu
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddChucVuRequestDto addChucVuRequestDto)
        {
            // Map or Convert DTP to Domain Model
            var chucvuDomainModel = new ChucVu
            { 
                TenChucVu = addChucVuRequestDto.TenChucVu,
                Meta = addChucVuRequestDto.Meta,    
                Hide = addChucVuRequestDto.Hide,
                ThuTuHienThi = addChucVuRequestDto.ThuTuHienThi,
                DateBegin = addChucVuRequestDto.DateBegin
            };

            // Use Domain Model to create chucvu
            await dbContext.chucVus.AddAsync(chucvuDomainModel);
            await dbContext.SaveChangesAsync();

            //Map Domain model back to DTO
            var chucvuDto = new ChucVuDto
            { 
                IdChucVu = chucvuDomainModel.IdChucVu,
                TenChucVu = chucvuDomainModel.TenChucVu,
                Meta = chucvuDomainModel.Meta,
                Hide = chucvuDomainModel.Hide,
                ThuTuHienThi = chucvuDomainModel.ThuTuHienThi,
                DateBegin= chucvuDomainModel.DateBegin  
                
            };


            return CreatedAtAction(nameof(GetById), new { id = chucvuDomainModel.IdChucVu }, chucvuDto);

        }
        // Update chucvu
        //Put
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateChucVuRequestDto updateChucVuRequestDto)
        {
            var chuvuDomainModel = await dbContext.chucVus.FirstOrDefaultAsync(x => x.IdChucVu == id);
            if(chuvuDomainModel == null)
            {
                return NotFound();

            }
            //Map DTO to Domain model
            chuvuDomainModel.TenChucVu = updateChucVuRequestDto.TenChucVu;
            chuvuDomainModel.Meta = updateChucVuRequestDto.Meta;
            chuvuDomainModel.Hide = updateChucVuRequestDto.Hide;    
            chuvuDomainModel.ThuTuHienThi = updateChucVuRequestDto.ThuTuHienThi;
            chuvuDomainModel.DateBegin = updateChucVuRequestDto.DateBegin;

            await dbContext.SaveChangesAsync();

            // Convert Domain model to DTO
            var chucVuDto = new ChucVuDto()
            {
                IdChucVu = chuvuDomainModel.IdChucVu,
                TenChucVu = chuvuDomainModel.TenChucVu,
                Meta = chuvuDomainModel.Meta,
                Hide = chuvuDomainModel.Hide,
                ThuTuHienThi = chuvuDomainModel.ThuTuHienThi,
                DateBegin = chuvuDomainModel.DateBegin


            };

            return Ok(chucVuDto);

        }

        //Delete ChucVu
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var chucvuDomainModel = await dbContext.chucVus.FirstOrDefaultAsync(x => x.IdChucVu == id);    
            if (chucvuDomainModel == null)
            {
                return NotFound();

            }
            dbContext.chucVus.Remove(chucvuDomainModel);
            await dbContext.SaveChangesAsync();

            var chuVuDto = new ChucVuDto()
            {
                IdChucVu = chucvuDomainModel.IdChucVu,
                TenChucVu = chucvuDomainModel.TenChucVu,
                Meta = chucvuDomainModel.Meta,
                Hide = chucvuDomainModel.Hide,
                ThuTuHienThi = chucvuDomainModel.ThuTuHienThi,
                DateBegin = chucvuDomainModel.DateBegin
            };
            return Ok(chuVuDto);

        }


    }
}
