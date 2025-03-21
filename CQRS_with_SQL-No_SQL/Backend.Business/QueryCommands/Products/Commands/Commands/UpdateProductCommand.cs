using Backend.Data.Models;
using MediatR;

namespace Backend.Business.QueryCommands.Products.Commands.Commands;

public class UpdateProductCommand(Product product) : IRequest<Product>
{
    public Product Product { get; set; } = product;
}