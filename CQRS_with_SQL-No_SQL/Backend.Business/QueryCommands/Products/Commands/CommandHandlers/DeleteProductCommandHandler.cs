using Backend.Business.QueryCommands.Products.Commands.Commands;
using Backend.Data.Models;
using Backend.Data.Repositories.Write.Interfaces;
using MediatR;

namespace Backend.Business.QueryCommands.Products.Commands.CommandHandlers;

public class DeleteProductCommandHandler(IWriteRepository<Product> repository ) : 
    IRequestHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        return await repository.DeleteByIdAsync(request.Id);
    }
}