using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.Common.Security;

namespace Ambev.DeveloperEvaluation.ORM.Seeds;

public static class DefaultSeeder
{
    public static async Task SeedAsync(DefaultContext context)
    {
        if (!context.Users.Any())
        {
            var passwordHasher = new BCryptPasswordHasher();

            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Customer",
                    Email = "customer@ambev.com",
                    Password = passwordHasher.HashPassword("Customer123!"),
                    Role = UserRole.Customer,
                    Status = UserStatus.Active,
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Admin",
                    Email = "admin@ambev.com",
                    Password = passwordHasher.HashPassword("Admin123!"),
                    Role = UserRole.Admin,
                    Status = UserStatus.Active,
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Manager",
                    Email = "manager@ambev.com",
                    Password = passwordHasher.HashPassword("Manager123!"),
                    Role = UserRole.Manager,
                    Status = UserStatus.Active,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }

        if (!context.Products.Any())
        {
            await SeedProductsAsync(context);
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(DefaultContext context)
    {
        var products = new[]
        {
            new Product 
            { 
                Name = "Brahma",
                Description = "Cerveja Brahma 350ml",
                Price = 3.50m,
                Category = ProductCategory.Beer,
                Status = ProductStatus.Active,
                StockQuantity = 100,
                CreatedAt = DateTime.UtcNow
            },
            new Product 
            { 
                Name = "Skol",
                Description = "Cerveja Skol 350ml",
                Price = 3.50m,
                Category = ProductCategory.Beer,
                Status = ProductStatus.Active,
                StockQuantity = 100,
                CreatedAt = DateTime.UtcNow
            },
            new Product 
            { 
                Name = "Antarctica",
                Description = "Cerveja Antarctica 350ml",
                Price = 3.50m,
                Category = ProductCategory.Beer,
                Status = ProductStatus.Active,
                StockQuantity = 100,
                CreatedAt = DateTime.UtcNow
            },
            new Product 
            { 
                Name = "Guaraná Antarctica",
                Description = "Guaraná Antarctica 350ml",
                Price = 3.00m,
                Category = ProductCategory.Water,
                Status = ProductStatus.Active,
                StockQuantity = 100,
                CreatedAt = DateTime.UtcNow
            },
            new Product 
            { 
                Name = "H2OH!",
                Description = "H2OH! Limão 500ml",
                Price = 3.00m,
                Category = ProductCategory.Water,
                Status = ProductStatus.Active,
                StockQuantity = 100,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Products.AddRangeAsync(products);
    }
} 