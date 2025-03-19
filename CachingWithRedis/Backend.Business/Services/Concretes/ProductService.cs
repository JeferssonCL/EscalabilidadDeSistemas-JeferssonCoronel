using System.Diagnostics;
using Backend.Business.CacheService.Interfaces;
using Backend.Business.Dtos;
using Backend.Business.Services.Interfaces;
using Backend.Data.Models;
using Backend.Data.Repositories.Interfaces;

namespace Backend.Business.Services.Concretes;

public class ProductService(IProductRepository repository, IRedisCacheService cacheService)
    : IProductService
{
    private const string CacheKey = "Products";

    public async Task<InfoDto<List<Product>>> GetProductsAsync()
    {
        var stopwatch = Stopwatch.StartNew();

        var cachedProducts = await cacheService.GetAsync<List<Product>>(CacheKey);
        if (cachedProducts != null)
        {
            stopwatch.Stop();
            return new InfoDto<List<Product>>
            {
                Source = "Cache",
                Data = cachedProducts,
                TimeToGet = $"{stopwatch.ElapsedMilliseconds} ms."
            };
        }

        var products = await repository.GetProductsAsync();
        await cacheService.SetAsync(CacheKey, products, TimeSpan.FromMinutes(5));

        stopwatch.Stop();
        return new InfoDto<List<Product>>
        {
            Source = "Database",
            Data = products,
            TimeToGet = $"{stopwatch.ElapsedMilliseconds} ms."
        };
    }

    public async Task<InfoDto<Product?>> GetProductByIdAsync(int id)
    {
        var cacheKey = $"Product_{id}";
        var stopwatch = Stopwatch.StartNew();

        var cachedProduct = await cacheService.GetAsync<Product>(cacheKey);
        if (cachedProduct != null)
        {
            stopwatch.Stop();
            return new InfoDto<Product?>
            {
                Source = "Cache",
                Data = cachedProduct,
                TimeToGet = $"{stopwatch.ElapsedMilliseconds} ms."
            };
        }

        var product = await repository.GetProductByIdAsync(id);
        if (product != null)
        {
            await cacheService.SetAsync(cacheKey, product, TimeSpan.FromMinutes(5));
        }

        stopwatch.Stop();
        return new InfoDto<Product?>
        {
            Source = "Database",
            Data = product,
            TimeToGet = $"{stopwatch.ElapsedMilliseconds} ms."
        };
    }

    public async Task<Product> AddProductAsync(Product product)
    {
        var newProduct = await repository.AddProductAsync(product);
        await cacheService.RemoveAsync("Products"); 
        return newProduct;
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        var updatedProduct = await repository.UpdateProductAsync(product);
        await cacheService.RemoveAsync("Products");
        await cacheService.RemoveAsync($"Product_{product.Id}"); 
        return updatedProduct;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var deleted = await repository.DeleteProductAsync(id);
        if (deleted)
        {
            await cacheService.RemoveAsync("Products");
            await cacheService.RemoveAsync($"Product_{id}"); 
        }
        return deleted;
    }
    
    public async Task<string> ClearProductCache()
    {
        await cacheService.RemoveAsync(CacheKey);
        return "Cache removed";
    }
}