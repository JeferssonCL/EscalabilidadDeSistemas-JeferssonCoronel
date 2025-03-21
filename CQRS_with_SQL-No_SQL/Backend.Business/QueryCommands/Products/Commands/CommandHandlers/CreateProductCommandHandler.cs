using Backend.Business.QueryCommands.Products.Commands.Commands;
using Backend.Data.Models;
using Backend.Data.Repositories.Write.Interfaces;
using MediatR;

namespace Backend.Business.QueryCommands.Products.Commands.CommandHandlers;

public class CreateProductCommandHandler(IWriteRepository<Product> repository) : 
    IRequestHandler<CreateProductCommand, Product>
{
    public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        return await repository.AddAsync(request.Product);
    }
}