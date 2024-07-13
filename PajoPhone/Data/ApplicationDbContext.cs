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
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<FieldsKey> FieldsKeys{ get; set; } = null!;
    public DbSet<FieldsValue> FieldsValues { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<FieldsKey>()
            .HasQueryFilter(f => f.DeletedAt==null);
        modelBuilder.Entity<FieldsValue>()
            .HasQueryFilter(f => f.DeletedAt==null);
        modelBuilder.Entity<FieldsKey>()
            .HasIndex(x =>  x.CategoryId);
        modelBuilder.Entity<FieldsKey>()
            .HasOne(x =>x.Category).WithMany(x=> x.FieldsKeys).HasForeignKey(x=> x.CategoryId).IsRequired(true);
    }

   public void SeedData()
{
    if (Products.Count() < 50)
    {
        var categories = new List<Category>();
        if (!Categories.Any())
        {
            var categoryFaker = new Bogus.Faker<Category>()
                .RuleFor(u => u.Name, f => f.Commerce.Department());
             categories = categoryFaker.Generate(10);
            Categories.AddRange(categories);
            SaveChanges();
        }
        else
        {
            categories = Categories.ToList();
        }
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
        var fieldValueFaker = new Bogus.Faker<FieldsValue>()
            .RuleFor(fv => fv.StringValue, f => f.Lorem.Word())
            .RuleFor(fv => fv.IntValue, f => f.Random.Int(0, 100));

        foreach (var product in products)
        {
                var productCategory = categories.First(c => c.Id == product.CategoryId);
                foreach (var fieldKey in productCategory.FieldsKeys)
                {
                    var fieldValue = fieldValueFaker.Generate();
                    fieldValue.FieldKey = fieldKey;
                    fieldValue.Product = product;
                    FieldsValues.Add(fieldValue);
                }
        }
        SaveChanges();
    }
}
}
