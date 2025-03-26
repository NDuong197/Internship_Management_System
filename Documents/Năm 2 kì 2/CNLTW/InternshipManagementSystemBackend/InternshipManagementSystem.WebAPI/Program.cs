using InternshipManagementSystem.Infrastructure.Data;
using InternshipManagementSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using InternshipManagementSystem.Core.Interfaces;
using InternshipManagementSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Internship API", Version = "v1" });

    // Thêm xác thực Bearer Token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token theo định dạng: Bearer {your_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
}); 
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

// Seed data tự động
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Đảm bảo database được tạo và áp dụng migrations
    dbContext.Database.Migrate();

    // Seed SinhVien nếu chưa có
    if (!dbContext.SinhViens.Any())
    {
        dbContext.SinhViens.Add(new SinhVien
        {
            HoTen = "Nguyễn Văn A",
            MaSV = "SV001",
            Lop = "22IT3",
            Email = "sinhvien1@example.com",
            MatKhau = "password123",
            SDT = "0123456789",
            GPA = 3.5m
            // SvId sẽ tự động được sinh bởi NEWID() trong database
        });
    }

    // Seed DoanhNghiep nếu chưa có
    if (!dbContext.DoanhNghieps.Any())
    {
        dbContext.DoanhNghieps.AddRange(
            new DoanhNghiep
            {
                TenDN = "Công ty ABC",
                DiaChi = "123 Đường Láng, Hà Nội",
                SoDT = "0987654321",
                Email = "doanhnghiep1@example.com",
                MatKhau = "password123"
                // DnId sẽ tự động được sinh bởi NEWID()
            },
            new DoanhNghiep
            {
                TenDN = "Công ty XYZ",
                DiaChi = "456 Nguyễn Trãi, Hà Nội",
                SoDT = "0987654322",
                Email = "doanhnghiep2@example.com",
                MatKhau = "password123"
                // DnId sẽ tự động được sinh bởi NEWID()
            }
        );
    }

    // Seed Admin nếu chưa có
    if (!dbContext.Admins.Any())
    {
        dbContext.Admins.Add(new Admin
        {
            Email = "admin@example.com",
            MatKhau = "admin123"
            // AdminId sẽ tự động được sinh bởi NEWID()
        });
    }

    // Seed DangKyThucTap nếu chưa có (liên kết SinhVien và DoanhNghiep)
    if (!dbContext.DangKyThucTaps.Any())
    {
        var sinhVien = dbContext.SinhViens.FirstOrDefault(s => s.MaSV == "SV001");
        var doanhNghiep = dbContext.DoanhNghieps.FirstOrDefault(d => d.Email == "doanhnghiep1@example.com");

        if (sinhVien != null && doanhNghiep != null)
        {
            dbContext.DangKyThucTaps.Add(new DangKyThucTap
            {
                SvId = sinhVien.SvId,
                DnId = doanhNghiep.DnId,
                TrangThai = "Chờ duyệt"
                // DkttId tự động bởi NEWID(), NgayDangKy tự động bởi GETDATE()
            });
        }
    }

    // Lưu tất cả thay đổi vào database
    dbContext.SaveChanges();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// Sử dụng CORS
app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();
app.Run();