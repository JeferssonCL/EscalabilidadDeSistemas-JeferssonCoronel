using Backend.Data.Data.SQL_Read;
using Backend.Data.Models;
using Backend.Data.Repositories.Read.Interfaces;
using MongoDB.Driver;

namespace Backend.Data.Repositories.Read.Concretes;

public class ProductReadRepository(MongoContext mongoContext) : IReadRepository<ProductReadModel>
{
    private readonly IMongoCollection<ProductReadModel> _collection = mongoContext.GetCollection<ProductReadModel>("Products");

    public async Task<List<ProductReadModel>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<ProductReadModel?> GetByIdAsync(int id)
    {
        return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }
}
