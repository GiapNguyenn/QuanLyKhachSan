using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QLKS.API.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ Ép ứng dụng lắng nghe đúng PORT do Render chỉ định
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://*:{port}");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<QLKSDbContextcs>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QLKSConnectionString")));

var configuration = builder.Configuration;

// ✅ Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://vite.hoanglong.dev")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ✅ Cấu hình Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v3", new OpenApiInfo { Title = "Your API", Version = "v3" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Vui lòng nhập chuỗi kết nối JWT tại đây",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = configuration["Jwt:ValidIssuer"],
            ValidAudience = configuration["Jwt:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v3/swagger.json", "Your API V3");
    c.RoutePrefix = ""; // 👉 Swagger làm trang chủ
});

// ❌ Không nên bật HTTPS redirect trên Render Free
// app.UseHttpsRedirection();

app.UseRouting();

// ✅ Kích hoạt chính sách CORS
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
