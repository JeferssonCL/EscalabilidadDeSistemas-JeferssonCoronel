using MediatR;

namespace Backend.Business.QueryCommands.Products.Commands.Commands;

public class DeleteProductCommand(int id) : IRequest<bool>
{
    public int Id { get; set; } = id;
}