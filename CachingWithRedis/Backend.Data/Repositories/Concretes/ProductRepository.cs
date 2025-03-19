using Backend.Data.Models;
using Backend.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repositories.Concretes;

public class ProductRepository(DbContext context) : IProductRepository
{
    private readonly DbSet<Product> _dbSet = context.Set<Product>();

    public async Task<List<Product>> GetProductsAsync()
    {
        return await _dbSet.ToListAsync();
    }
    
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        return entity;
    }
    
    public async Task<Product> AddProductAsync(Product product)
    {
        _dbSet.Add(product);
        await context.SaveChangesAsync();
        return product;
    }
    
    public async Task<Product> UpdateProductAsync(Product product)
    {
        _dbSet.Update(product);
        await context.SaveChangesAsync();
        return product;
    }
    
    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _dbSet.FindAsync(id);
        if (product == null) return false;
        _dbSet.Remove(product);
        await context.SaveChangesAsync();
        return true;
    }
}
