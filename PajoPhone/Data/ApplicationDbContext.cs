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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<FieldsKey>().HasQueryFilter(f => f.DeletedAt==null);
        modelBuilder.Entity<FieldsValue>().HasQueryFilter(f => f.DeletedAt==null);
        modelBuilder.Entity<FieldsKey>().HasIndex(x => new {x.CategoryId, x.Key}).HasFilter("[isDisabled] = 0").IsUnique(true);
        modelBuilder.Entity<FieldsKey>().HasIndex(x =>  x.CategoryId);
        modelBuilder.Entity<FieldsKey>().HasOne(x =>x.Category).WithMany(x=> x.FieldsKeys).HasForeignKey(x=> x.CategoryId).IsRequired(true);
    }

   public void SeedData()
{
    if (Products.Count() < 50)
    {
        var categoryFaker = new Bogus.Faker<Category>()
            .RuleFor(u => u.Name, f => f.Commerce.Department());
        var categories = categoryFaker.Generate(10);
        Categories.AddRange(categories);

        var fieldsKeyFaker = new Bogus.Faker<FieldsKey>()
            .RuleFor(u => u.Key, f => f.Lorem.Word())
            .RuleFor(u => u.Category, f =>
            {
                Random rand = new Random();
                return categories[rand.Next(0, categories.Count)];
            });

        var fieldsKeys = fieldsKeyFaker.Generate(20); 
        FieldsKeys.AddRange(fieldsKeys);

        
        var productFaker = new Bogus.Faker<Product>()
            .RuleFor(u => u.Name, f => f.Commerce.ProductName())
            .RuleFor(u => u.Color, f => f.Commerce.Color())
            .RuleFor(u => u.Price, f => double.Parse(f.Commerce.Price()))
            .RuleFor(u => u.Description, f => f.Commerce.ProductDescription())
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

        var products = productFaker.Generate(100);
        Products.AddRange(products);
        foreach (var product in products)
        {
            foreach (var fieldKey in product.Category.FieldsKeys)
            {
                var fieldValue = new FieldsValue
                {
                    StringValue = "String Value",
                    IntValue = 0, 
                    FieldKey = fieldKey,
                    Product = product
                };
                FieldsValues.Add(fieldValue);
            }
        }
        SaveChanges();
    }
}

}
