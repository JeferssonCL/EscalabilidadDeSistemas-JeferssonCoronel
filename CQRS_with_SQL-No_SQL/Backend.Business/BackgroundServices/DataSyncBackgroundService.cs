using Backend.Data.Data.SQL_Read;
using Backend.Data.Data.SQL_Write;
using Backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Backend.Business.BackgroundServices;

public class DataSyncBackgroundService(
    ILogger<BackgroundService> logger,
    IServiceProvider serviceProvider)
    : BackgroundService
{
    private readonly TimeSpan _period = TimeSpan.FromMinutes(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("SQL to MongoDB Synchronization Service starting");
        using var timer = new PeriodicTimer(_period);

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
                await SynchronizeProductsAsync(stoppingToken);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("SQL to MongoDB Synchronization Service stopping");
        }
    }

    private async Task SynchronizeProductsAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting data synchronization from SQL to MongoDB");

        try
        {
            using var scope = serviceProvider.CreateScope();
            var postgresContext = scope.ServiceProvider.GetRequiredService<PostgresContext>();
            var mongoContext = scope.ServiceProvider.GetRequiredService<MongoContext>();

            var sqlProducts = await postgresContext.Set<Product>().ToListAsync(stoppingToken);
            var mongoCollection = mongoContext.GetCollection<Product>("Products");
            var existingMongoProducts = await mongoCollection.Find(_ => true).ToListAsync(stoppingToken);

            foreach (var sqlProduct in sqlProducts)
            {
                var existingMongoProduct = existingMongoProducts.FirstOrDefault(p => p.Id == sqlProduct.Id);

                if (existingMongoProduct == null)
                {
                    var productReadModel = new ProductReadModel
                    {
                        Id = sqlProduct.Id,
                        Name = sqlProduct.Name,
                        Price = sqlProduct.Price,
                        Category = sqlProduct.Price > 100 ? "Luxury" : "Economical"
                    };
                    await mongoCollection.InsertOneAsync(productReadModel, null, stoppingToken);
                    logger.LogInformation("Product {ProductId} inserted into MongoDB", sqlProduct.Id);
                }
                else if (!ProductsAreEqual(sqlProduct, existingMongoProduct))
                {
                    var filter = Builders<Product>.Filter.Eq(p => p.Id, sqlProduct.Id);
                    var replaceOptions = new ReplaceOptions();
                    await mongoCollection.ReplaceOneAsync(filter, sqlProduct, replaceOptions, stoppingToken);
                    logger.LogInformation("Product {ProductId} updated in MongoDB", sqlProduct.Id);
                }
            }

            var sqlProductIds = sqlProducts.Select(p => p.Id).ToHashSet();
            var productsToDelete = existingMongoProducts.Where(p => 
                !sqlProductIds.Contains(p.Id)).ToList();

            foreach (var productToDelete in productsToDelete)
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, productToDelete.Id);
                await mongoCollection.DeleteOneAsync(filter, stoppingToken);
                logger.LogInformation("Product {ProductId} deleted from MongoDB", productToDelete.Id);
            }

            logger.LogInformation("Data synchronization completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during synchronization");
        }
    }

    private bool ProductsAreEqual(Product product1, Product product2)
    {
        return product1.Id == product2.Id &&
               product1.Name == product2.Name &&
               Math.Abs(product1.Price - product2.Price) < 0.01;
    }
}