using PajoPhone.AutoMapperProfiles;
using Microsoft.EntityFrameworkCore;
using PajoPhone;
using PajoPhone.Loader;
using PajoPhone.Models;
using PajoPhone.Repositories.Category;
using PajoPhone.Services.Factory;

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
builder.Services.AddScoped<IProductLoader, ProductLoader>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddHttpClient<GooshiShopScraper>();
var app = builder.Build();
//Seed Data
SeedDataProvider.Initialize(app.Services);
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseMiddleware<LoggerMiddleWare>();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();