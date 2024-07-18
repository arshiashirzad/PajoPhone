using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PajoPhone.Models
{
    public static class SeedDataProvider
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                SeedData(dbContext);
            }
        }
        private static void SeedData(ApplicationDbContext dbContext)
        {
            if (dbContext.Products.Count() < 50)
            {
                var categories = new List<Category>();
                if (!dbContext.Categories.Any())
                {
                    var categoryFaker = new Faker<Category>()
                        .RuleFor(u => u.Name, f => f.Commerce.Department());
                    categories = categoryFaker.Generate(10);
                    dbContext.Categories.AddRange(categories);
                    dbContext.SaveChanges();
                }
                else
                {
                    categories = dbContext.Categories.ToList();
                }

                var fieldsKeyFaker = new Faker<FieldsKey>()
                    .RuleFor(u => u.Key, f => f.Lorem.Word())
                    .RuleFor(u => u.Category, f =>
                    {
                        Random rand = new Random();
                        return categories[rand.Next(0, categories.Count)];
                    });

                var fieldsKeys = fieldsKeyFaker.Generate(20);
                dbContext.FieldsKeys.AddRange(fieldsKeys);
                var counter = 0;
                var productFaker = new Faker<Product>()
                    .RuleFor(u => u.Name, f => f.Commerce.ProductName())
                    .RuleFor(u => u.Color, f => f.Commerce.Color())
                    .RuleFor(u => u.Price, f => double.Parse(f.Commerce.Price()))
                    .RuleFor(u => u.Description, f => f.Commerce.ProductDescription())
                    .RuleFor(u => u.Image, f =>
                    {
                        byte[] response=[];
                        var isDone = false;
                        string url = "https://picsum.photos/200/100/";
                        while (!isDone)
                        {
                            try
                            {
                                response = _httpClient.GetByteArrayAsync(url).Result;
                                isDone = true;
                            }
                            catch (Exception e)
                            {
                                
                            }
                        }

                        counter++;
                        if (counter % 100 == 0)
                        {
                            Console.WriteLine($"*** product {counter} of 50000");
                        }
                        return response;
                    })
                    .RuleFor(u => u.Category, f =>
                    {
                        Random rand = new Random();
                        return categories[rand.Next(0, categories.Count)];
                    });

                var products = productFaker.Generate(5000);
                dbContext.Products.AddRange(products);

                var fieldValueFaker = new Faker<FieldsValue>()
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
                        dbContext.FieldsValues.Add(fieldValue);
                    }
                }

                dbContext.SaveChanges();
            }
        }
    }
}
