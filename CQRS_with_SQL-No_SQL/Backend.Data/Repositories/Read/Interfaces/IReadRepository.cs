namespace Backend.Data.Repositories.Read.Interfaces;

public interface IReadRepository<T> where T : class
{
    public Task<List<T>> GetAllAsync();
    public Task<T?> GetByIdAsync(int id);
}