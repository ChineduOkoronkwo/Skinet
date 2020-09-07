using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory) 
        {
            try 
            {
                if (!context.ProductBrands.Any()) 
                {
                    var brandData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                     await context.ProductBrands.AddRangeAsync(brands);                     
                    await context.SaveChangesAsync();                   
                }
                if (!context.ProductTypes.Any()) {
                    var typeData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);
                    await context.ProductTypes.AddRangeAsync(types);
                    await context.SaveChangesAsync();
                }
                if (!context.Products.Any()) {
                    var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    await context.Products.AddRangeAsync(products);
                    await context.SaveChangesAsync();
                }
                if (!context.DeliveryMethods.Any()) {
                    var dmData = File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");
                    var dm = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);
                    await context.DeliveryMethods.AddRangeAsync(dm);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex) 
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex, "An error occured during data seeding");
            }

        }
    }
}