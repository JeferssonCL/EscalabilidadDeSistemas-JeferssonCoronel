using Backend.Business.QueryCommands.Products.Queries.Queries;
using Backend.Data.Models;
using Backend.Data.Repositories.Read.Interfaces;
using MediatR;

namespace Backend.Business.QueryCommands.Products.Queries.QueryHandlers;

public class GetAllProductsQueryHandler(IReadRepository<ProductReadModel> repository) : 
    IRequestHandler<GetAllProductsQuery, List<ProductReadModel>>
{
    public async Task<List<ProductReadModel>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync();
    }
}