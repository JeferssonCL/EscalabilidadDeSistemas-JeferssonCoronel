using Backend.Business.Dtos;
using Backend.Data.Models;

namespace Backend.Business.Services.Interfaces;

public interface IProductService
{
    public Task<InfoDto<List<Product>>> GetProductsAsync();
    public Task<InfoDto<Product?>> GetProductByIdAsync(int id);
    public Task<Product> AddProductAsync(Product product);
    public Task<Product> UpdateProductAsync(Product product);
    public Task<bool> DeleteProductAsync(int id);
    public Task<string> ClearProductCache();
}