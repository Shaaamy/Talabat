﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregation;

namespace Talabat.Repository.Data
{
    public static class StoreDbContextSeed
    {
        public static async Task SeedAsync(StoreDbContext dbContext)
        {
           if(!dbContext.ProductBrands.Any())
            {
                var BrandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData); //convert json string into object 
                //Why?: Converts the JSON data into a format the application can use (e.g., C# objects).
                if (Brands?.Count > 0)
                {
                    foreach (var Brand in Brands)
                        await dbContext.Set<ProductBrand>().AddAsync(Brand);
                    await dbContext.SaveChangesAsync();
                }
            }
           if(!dbContext.ProductTypes.Any())
            {
                var TypeData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypeData);
                if (Types?.Count > 0)
                {
                    foreach (var Type in Types)
                        await dbContext.Set<ProductType>().AddAsync(Type);
                    await dbContext.SaveChangesAsync();
                }
            }
           if(!dbContext.Products.Any())
            {
                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json"); 
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (Products?.Count > 0)
                {
                    foreach (var Product in Products)
                        await dbContext.Set<Product>().AddAsync(Product);
                    await dbContext.SaveChangesAsync();
                }
            }
            if (!dbContext.DeliveryMethods.Any())
            {
                var DeliveryData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);
                if(DeliveryMethods?.Count > 0)
                {
                    foreach(var  DeliveryMethod in DeliveryMethods)
                        await dbContext.Set<DeliveryMethod>().AddAsync(DeliveryMethod);
                    await dbContext.SaveChangesAsync();
                }
            }


        }

        
    }
}
