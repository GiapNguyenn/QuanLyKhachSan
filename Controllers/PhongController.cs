using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QLKS.API.Models.Domain;
using QLKS.API.Data;
using Microsoft.EntityFrameworkCore;

namespace QuanLyKhachSan.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhongController : ControllerBase
{
    private readonly QLKSDbContextcs _context;

    public PhongController(QLKSDbContextcs context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var phongList = await _context.Phongs.ToListAsync();
        return Ok(phongList);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var phong = await _context.Phongs.FindAsync(id);
        if (phong == null) return NotFound();
        return Ok(phong);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Phong phong)
    {
        await _context.Phongs.AddAsync(phong);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = phong.IdPhong }, phong);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Phong phong)
    {
        var existing = await _context.Phongs.FindAsync(id);
        if (existing == null) return NotFound();

        // update fields
        _context.Entry(existing).CurrentValues.SetValues(phong);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var phong = await _context.Phongs.FindAsync(id);
        if (phong == null) return NotFound();

        _context.Phongs.Remove(phong);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}
