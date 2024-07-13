using PajoPhone.AutoMapperProfiles;
using Microsoft.EntityFrameworkCore;
using PajoPhone;
using PajoPhone.Models;
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
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.SeedData();
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
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();