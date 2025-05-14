using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuanLyKhachSan.Models.DTO;
using QuanLyKhachSan.Models.Domain;
using QLKS.API.Data;
using Microsoft.EntityFrameworkCore;


namespace QuanLyKhachSan.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhieuDatPhongController : Controller
{
 private readonly QLKSDbContextcs _context;

        public PhieuDatPhongController(QLKSDbContextcs context)
        {
            _context = context;
        }

        // GET: api/PhieuDatPhong
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _context.PhieuDatPhongs.ToListAsync();
            return Ok(result);
        }

        // GET: api/PhieuDatPhong/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.PhieuDatPhongs.FindAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/PhieuDatPhong
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PhieuDatPhong model)
        {
            await _context.PhieuDatPhongs.AddAsync(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.IdPhieuDatPhong }, model);
        }

        // PUT: api/PhieuDatPhong/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PhieuDatPhong model)
        {
            var existing = await _context.PhieuDatPhongs.FindAsync(id);
            if (existing == null) return NotFound();

            _context.Entry(existing).CurrentValues.SetValues(model);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/PhieuDatPhong/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.PhieuDatPhongs.FindAsync(id);
            if (existing == null) return NotFound();

            _context.PhieuDatPhongs.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    
    
}
