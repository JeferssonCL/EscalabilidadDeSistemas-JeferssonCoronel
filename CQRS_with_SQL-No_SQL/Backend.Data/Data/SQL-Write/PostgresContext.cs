using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Data.SQL_Write;

public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgresContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}