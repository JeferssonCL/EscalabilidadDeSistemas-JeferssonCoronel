using Backend.Business.QueryCommands.Products.Queries.Queries;
using Backend.Data.Models;
using Backend.Data.Repositories.Read.Interfaces;
using MediatR;

namespace Backend.Business.QueryCommands.Products.Queries.QueryHandlers;

public class GetProductByIdQueryHandler(IReadRepository<ProductReadModel> repository) : 
    IRequestHandler<GetProductByIdQuery, ProductReadModel?>
{
    public async Task<ProductReadModel?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(request.Id);
    }
}