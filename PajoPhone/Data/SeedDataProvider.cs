using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PajoPhone.Models
{
    public static class SeedDataProvider
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                using var httpClient = new HttpClient();
                await SeedDataAsync(dbContext, serviceProvider, httpClient);
            }
        }

public static async Task SeedDataAsync(
    ApplicationDbContext dbContext, 
    IServiceProvider serviceProvider,
    HttpClient httpClient)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = { "Admin", "Customer" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    var adminEmail = "admin@example.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(adminUser, "Admin@123");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    var customerEmail = "customer@example.com";
    var customerUser = await userManager.FindByEmailAsync(customerEmail);
    if (customerUser == null)
    {
        customerUser = new IdentityUser
        {
            UserName = customerEmail,
            Email = customerEmail,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(customerUser, "Customer@123");
        await userManager.AddToRoleAsync(customerUser, "Customer");
    }

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
            .RuleFor(u => u.Category, f => categories[new Random().Next(categories.Count)]);

        var fieldsKeys = fieldsKeyFaker.Generate(20);
        dbContext.FieldsKeys.AddRange(fieldsKeys);

        var productFaker = new Faker<Product>()
            .RuleFor(u => u.Name, f => f.Commerce.ProductName())
            .RuleFor(u => u.Color, f => f.Commerce.Color())
            .RuleFor(u => u.Price, f => double.Parse(f.Commerce.Price()))
            .RuleFor(u => u.Description, f => f.Commerce.ProductDescription())
            .RuleFor(u => u.Image, f =>
            {
                var url = "https://picsum.photos/200/100/";
                try
                {
                    return httpClient.GetByteArrayAsync(url).Result;
                }
                catch
                {
                    return new byte[0];
                }
            })
            .RuleFor(u => u.Category, f => categories[new Random().Next(categories.Count)]);

        var products = productFaker.Generate(50);
        dbContext.Products.AddRange(products);

        var fieldValueFaker = new Faker<FieldsValue>()
            .RuleFor(fv => fv.StringValue, f => f.Lorem.Word())
            .RuleFor(fv => fv.IntValue, f => f.Random.Int(0, 100));

        foreach (var product in products)
        {
            var productCategory = categories.FirstOrDefault(c => c.Id == product.CategoryId);
            if (productCategory?.FieldsKeys != null)
            {
                foreach (var fieldKey in productCategory.FieldsKeys)
                {
                    var fieldValue = fieldValueFaker.Generate();
                    fieldValue.FieldKey = fieldKey;
                    fieldValue.Product = product;
                    dbContext.FieldsValues.Add(fieldValue);
                }
            }
        }

        dbContext.SaveChanges();
    }
}
    }
}
