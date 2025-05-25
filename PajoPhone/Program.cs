using Microsoft.AspNetCore.Identity;
using PajoPhone.AutoMapperProfiles;
using Microsoft.EntityFrameworkCore;
using PajoPhone;
using PajoPhone.Api.Scraper;
using PajoPhone.Cache;
using PajoPhone.Loader;
using PajoPhone.Middlewares;
using PajoPhone.Models;
using PajoPhone.Repositories.Category;
using PajoPhone.Repositories.Product;
using PajoPhone.Services.Factory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(ProductProfile));
// DI
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString ,ServerVersion.AutoDetect(connectionString) )
);
// Identity with Roles
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        // Password settings
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;

        // User settings
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IProductBuilder,ProductBuilder>()
    .AddScoped<IProductFactory,ProductFactory>();
builder.Services.AddScoped<IProductLoader, ProductLoader>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<PriceCacheManager>();
builder.Services.AddHttpClient<GooshiShopScraper>();
builder.Services.AddMemoryCache();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();
//Seed Data
using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    await SeedDataProvider.InitializeAsync(scopedProvider);
}



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseMiddleware<LoggerMiddleWare>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();