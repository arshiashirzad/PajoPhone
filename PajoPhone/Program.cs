using AutoMapper;   
using PajoPhone.AutoMapperProfiles;
using Microsoft.EntityFrameworkCore;
using PajoPhone;
using PajoPhone.Models;
using PajoPhone.Services.Factory;
using Product = PajoPhone.Models.Product;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(ProductProfile));
// DI
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString ,ServerVersion.AutoDetect(connectionString) )
);
builder.Services.AddScoped<IProductBuilder,ProductBuilder>()
    .AddScoped<IProductFactory,ProductFactory>();
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