using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace QuanLyKhachSan.helpers;

public static class ImageUploadHelper
{
    public static async Task<string> SaveImageAsync(IFormFile image, string folderName)
    {
        if (image == null || image.Length == 0)
            throw new ArgumentException("Ảnh không hợp lệ");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        return $"/{folderName}/{fileName}";
    }
}
