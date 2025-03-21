using Backend.Data.Data.SQL_Write;
using Backend.Data.Models;
using Backend.Data.Repositories.Write.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repositories.Write.Concretes;

public class ProductWriteRepository(PostgresContext context) : IWriteRepository<Product>
{
    private readonly DbSet<Product> _dbSet = context.Set<Product>();

    public async Task<Product> AddAsync(Product product)
    {
        _dbSet.Add(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        _dbSet.Update(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var product = await _dbSet.FindAsync(id);
        if (product == null) return false;
        _dbSet.Remove(product);
        await context.SaveChangesAsync();
        return true;
    }
}