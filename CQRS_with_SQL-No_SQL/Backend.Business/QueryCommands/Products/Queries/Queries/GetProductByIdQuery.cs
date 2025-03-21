using Backend.Data.Models;
using MediatR;

namespace Backend.Business.QueryCommands.Products.Queries.Queries;

public class GetProductByIdQuery(int id) : IRequest<ProductReadModel?>
{
    public int Id { get; set; } = id;
}