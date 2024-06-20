using System.Net.Mime;
using Bogus.DataSets;
using System.Net.Http;
using Microsoft.Extensions.Http;
using Microsoft.EntityFrameworkCore;

namespace PajoPhone.Models;

public class ApplicationDbContext : DbContext
{
    private readonly HttpClient _httpClient = new HttpClient();
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<FieldsKey> FieldsKeys{ get; set; }
    public DbSet<FieldsValue> FieldsValues { get; set; }

    public void SeedData()
    {
        if (Products.Count() < 50)
        {
            var categoryFaker = new Bogus.Faker<Category>()
                .RuleFor(u => u.Name, f => f.Commerce.Department());
            var categories = categoryFaker.Generate(10);
            Categories.AddRange(categories);
            var productFaker = new Bogus.Faker<Product>()
                .RuleFor(u => u.Name, f => f.Name.FirstName())
                .RuleFor(u => u.Color, f => f.Commerce.Color())
                .RuleFor(u => u.Price, f => double.Parse(f.Commerce.Price()))
                .RuleFor(u => u.Description, f => f.Lorem.Sentence())
                .RuleFor(u => u.Image, f =>
                {
                    string url = "https://picsum.photos/640/480/";
                    var response = _httpClient.GetByteArrayAsync(url).Result;
                    return response;
                })
                .RuleFor(u => u.Category, f =>
                {
                    Random rand = new Random();
                    return categories[rand.Next(0, categories.Count)];
                });
            var products = productFaker.Generate(50);
            Products.AddRange(products);
            SaveChanges();
        }
    }
}
