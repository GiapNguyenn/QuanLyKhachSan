using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLKS.API.Data;
using QLKS.API.Models.Domain;
using QLKS.API.Models.DTO.DichVu;

[ApiController]
[Route("api/[controller]")] // Route sẽ là /api/DichVu
public class DichVuController : ControllerBase
{
    private readonly QLKSDbContextcs _dbContext; // Sử dụng tên DbContext của bạn

    public DichVuController(QLKSDbContextcs dbContext) // Sử dụng tên DbContext của bạn
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var dichVuDomain = await _dbContext.DichVus.ToListAsync();

        // Chuyển đổi Domain Model sang DTO
        var dichVuDto = dichVuDomain.Select(dv => new DichVuDto
        {
            MaDichVu = dv.MaDichVu, // MaDichVu là int
            TenDichVu = dv.TenDichVu,
            DonGia = dv.DonGia,
            MoTa = dv.MoTa,
            AnhMinhHoa = dv.AnhMinhHoa
        }).ToList();

        return Ok(dichVuDto);
    }

    // GET DICHVU BY ID (Lấy dịch vụ theo MãDichVu)
    // GET: /api/DichVu/{MaDichVu}
    [HttpGet]
    [Route("{MaDichVu:int}")] // Chỉ rõ MaDichVu là số nguyên
    public async Task<IActionResult> GetById([FromRoute] int MaDichVu) // Thay đổi kiểu sang int
    {
        var dichVuDomain = await _dbContext.DichVus.FirstOrDefaultAsync(x => x.MaDichVu == MaDichVu);

        if (dichVuDomain == null)
        {
            return NotFound("Không tìm thấy dịch vụ.");
        }

        // Chuyển đổi Domain Model sang DTO
        var dichVuDto = new DichVuDto
        {
            MaDichVu = dichVuDomain.MaDichVu,
            TenDichVu = dichVuDomain.TenDichVu,
            DonGia = dichVuDomain.DonGia,
            MoTa = dichVuDomain.MoTa,
            AnhMinhHoa = dichVuDomain.AnhMinhHoa
        };

        return Ok(dichVuDto);
    }
    [HttpGet]
    [Route("search")] // Đặt một route riêng để tránh xung đột với GetById
    public async Task<IActionResult> SearchByName([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            // Nếu tên tìm kiếm rỗng, trả về tất cả hoặc BadRequest tùy theo nghiệp vụ
            return BadRequest("Tên dịch vụ cần tìm không được để trống.");
        }

        var dichVuDomain = await _dbContext.DichVus
                                .Where(dv => dv.TenDichVu.Contains(name))
                                .ToListAsync();

        if (!dichVuDomain.Any()) // Kiểm tra nếu không tìm thấy dịch vụ nào
        {
            return NotFound($"Không tìm thấy dịch vụ nào có tên chứa '{name}'.");
        }

        // Chuyển đổi Domain Model sang DTOs
        var dichVuDtos = dichVuDomain.Select(dv => new DichVuDto
        {
            MaDichVu = dv.MaDichVu,
            TenDichVu = dv.TenDichVu,
            DonGia = dv.DonGia,
            MoTa = dv.MoTa,
            AnhMinhHoa = dv.AnhMinhHoa
        }).ToList();

        return Ok(dichVuDtos);
    }


    // ADD NEW DICHVU (Thêm mới dịch vụ)
    // POST: /api/DichVu
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddDichVuRequestDto addDichVuRequestDto)
    {
        // Kiểm tra validation từ DTO
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Trả về lỗi validation
        }

        // Chuyển đổi DTO sang Domain Model
        var dichVuDomain = new DichVu
        {
            // Không gán MaDichVu ở đây vì DB sẽ tự tạo (IDENTITY)
            TenDichVu = addDichVuRequestDto.TenDichVu,
            DonGia = addDichVuRequestDto.DonGia,
            MoTa = addDichVuRequestDto.MoTa,
            AnhMinhHoa = addDichVuRequestDto.AnhMinhHoa
        };

        // Không cần kiểm tra MaDichVu đã tồn tại vì nó là IDENTITY

        await _dbContext.DichVus.AddAsync(dichVuDomain);
        await _dbContext.SaveChangesAsync();

        // Chuyển đổi Domain Model trở lại DTO để trả về (MaDichVu đã được DB gán)
        var dichVuDto = new DichVuDto
        {
            MaDichVu = dichVuDomain.MaDichVu, // Lấy MaDichVu đã được DB gán
            TenDichVu = dichVuDomain.TenDichVu,
            DonGia = dichVuDomain.DonGia,
            MoTa = dichVuDomain.MoTa,
            AnhMinhHoa = dichVuDomain.AnhMinhHoa
        };

        // Trả về 201 CreatedAtAction với URL của dịch vụ mới được tạo
        return CreatedAtAction(nameof(GetById), new { MaDichVu = dichVuDto.MaDichVu }, dichVuDto);
    }

    // UPDATE DICHVU (Cập nhật dịch vụ)
    // PUT: /api/DichVu/{MaDichVu}
    [HttpPut]
    [Route("{MaDichVu:int}")] // Chỉ rõ MaDichVu là số nguyên
    public async Task<IActionResult> Update([FromRoute] int MaDichVu, [FromBody] UpdateDichVuRequestDto updateDichVuRequestDto) // Thay đổi kiểu sang int
    {
        // Kiểm tra validation từ DTO
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Trả về lỗi validation
        }

        var dichVuDomain = await _dbContext.DichVus.FirstOrDefaultAsync(x => x.MaDichVu == MaDichVu);

        if (dichVuDomain == null)
        {
            return NotFound("Không tìm thấy dịch vụ để cập nhật.");
        }

        // Cập nhật các thuộc tính
        dichVuDomain.TenDichVu = updateDichVuRequestDto.TenDichVu;
        dichVuDomain.DonGia = updateDichVuRequestDto.DonGia;
        dichVuDomain.MoTa = updateDichVuRequestDto.MoTa;
        dichVuDomain.AnhMinhHoa = updateDichVuRequestDto.AnhMinhHoa;

        await _dbContext.SaveChangesAsync();

        // Chuyển đổi Domain Model trở lại DTO để trả về
        var dichVuDto = new DichVuDto
        {
            MaDichVu = dichVuDomain.MaDichVu,
            TenDichVu = dichVuDomain.TenDichVu,
            DonGia = dichVuDomain.DonGia,
            MoTa = dichVuDomain.MoTa,
            AnhMinhHoa = dichVuDomain.AnhMinhHoa
        };

        return Ok(dichVuDto);
    }

    // DELETE DICHVU (Xóa dịch vụ)
    // DELETE: /api/DichVu/{MaDichVu}
    [HttpDelete]
    [Route("{MaDichVu:int}")] // Chỉ rõ MaDichVu là số nguyên
    public async Task<IActionResult> Delete([FromRoute] int MaDichVu) // Thay đổi kiểu sang int
    {
        var dichVuDomain = await _dbContext.DichVus.FirstOrDefaultAsync(x => x.MaDichVu == MaDichVu);

        // Kiểm tra xem dịch vụ có bị tham chiếu bởi ChiTietDichVu không
        var hasRelatedChiTiet = await _dbContext.ChiTietDichVus.AnyAsync(ct => ct.MaDichVu == MaDichVu);
        if (hasRelatedChiTiet)
        {
            return BadRequest("Không thể xóa dịch vụ này vì đang có phiếu chi tiết dịch vụ tham chiếu đến nó.");
        }

        if (dichVuDomain == null)
        {
            return NotFound("Không tìm thấy dịch vụ để xóa.");
        }

        _dbContext.DichVus.Remove(dichVuDomain);
        await _dbContext.SaveChangesAsync();

        return NoContent(); // Trả về 204 No Content cho yêu cầu xóa thành công
    }
}