using Backend.Data.Models;
using MediatR;

namespace Backend.Business.QueryCommands.Products.Commands.Commands;

public class CreateProductCommand(Product product) : IRequest<Product>
{
    public Product Product { get; set; } = product;
}