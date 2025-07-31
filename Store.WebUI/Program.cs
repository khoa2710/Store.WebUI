using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.WebUI.Data;
using Store.WebUI.Entities;
using Store.WebUI.Repositories;
using Store.WebUI.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);





builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IUploadService, UploadService>();
// Add services to the container.
builder.Services.AddControllersWithViews();

//get connection stsrring
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
//============================
//-- Add identity --authentication

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
})
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

// JWT setup
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]); // gotta be mapped with appsettings
//=====================

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
