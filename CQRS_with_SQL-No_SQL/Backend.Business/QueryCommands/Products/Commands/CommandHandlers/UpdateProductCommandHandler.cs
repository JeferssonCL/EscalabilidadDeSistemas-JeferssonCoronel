using Backend.Business.QueryCommands.Products.Commands.Commands;
using Backend.Data.Models;
using Backend.Data.Repositories.Write.Interfaces;
using MediatR;

namespace Backend.Business.QueryCommands.Products.Commands.CommandHandlers;

public class UpdateProductCommandHandler(IWriteRepository<Product> repository) : 
    IRequestHandler<UpdateProductCommand, Product>
{
    public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        return await repository.UpdateAsync(request.Product);
    }
}
