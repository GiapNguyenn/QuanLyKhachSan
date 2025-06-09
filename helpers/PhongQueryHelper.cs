using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QLKS.API.Models.Domain;
using QuanLyKhachSan.Models.DTO;


namespace QuanLyKhachSan.helpers;

public static class PhongQueryHelper
{
     public static IQueryable<Phong> ApplyFilter(IQueryable<Phong> query, PhongFilterDto filter)
    {
        if (filter == null)
            return query;

        if (filter.TrangThai.HasValue)
            query = query.Where(p => p.TrangThai == filter.TrangThai.Value);

        if (filter.Hide.HasValue)
            query = query.Where(p => p.Hide == filter.Hide.Value);

        if (filter.IdLoaiPhong.HasValue)
            query = query.Where(p => p.IdLoaiPhong == filter.IdLoaiPhong.Value);

        if (filter.MinGiaPhong.HasValue)
            query = query.Where(p => p.GiaPhong >= filter.MinGiaPhong.Value);

        if (filter.MaxGiaPhong.HasValue)
            query = query.Where(p => p.GiaPhong <= filter.MaxGiaPhong.Value);

        return query;
    }

    public static async Task<PagedResult<PhongDto>> GetPagedResultAsync(
        IQueryable<Phong> query,
        PhongFilterDto filter,
        PaginationDto pagination)
    {
        query = ApplyFilter(query, filter);

        var pagedData = await query.ToPagedResultAsync(pagination);

        // Map sang DTO
        return new PagedResult<PhongDto>
        {
            TotalRecords = pagedData.TotalRecords,
            PageNumber = pagedData.PageNumber,
            PageSize = pagedData.PageSize,
            Items = pagedData.Items.Select(p => new PhongDto
            {
                IdPhong = p.IdPhong,
                TenPhong = p.TenPhong,
                GiaPhong = p.GiaPhong ?? 0,
                GiamGia = p.GiamGia ?? 0,
                SoLuong = p.SoLuong,
                SoNguoiLon = p.SoNguoiLon,
                SoTreEm = p.SoTreEm,
                DienTich = p.DienTich,
                MoTa = p.MoTa ?? "",
                Meta = p.Meta ?? "",
                Hide = p.Hide,
                ThuTuSapXep = p.ThuTuSapXep,
                DateBegin = p.DateBegin,
                HinhAnh = p.HinhAnh ?? "",
                TrangThai = p.TrangThai,
                IdLoaiPhong = p.IdLoaiPhong
            }).ToList()
        };
    }
}
