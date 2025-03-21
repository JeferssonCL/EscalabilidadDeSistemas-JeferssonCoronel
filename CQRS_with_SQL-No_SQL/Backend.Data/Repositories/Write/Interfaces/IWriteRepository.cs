namespace Backend.Data.Repositories.Write.Interfaces;

public interface IWriteRepository<T> where T : class
{
    public Task<T> AddAsync(T entity);
    public Task<T> UpdateAsync(T entity);
    public Task<bool> DeleteByIdAsync(int id);
}