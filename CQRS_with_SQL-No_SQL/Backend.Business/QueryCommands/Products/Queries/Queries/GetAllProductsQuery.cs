using Backend.Data.Models;
using MediatR;

namespace Backend.Business.QueryCommands.Products.Queries.Queries;

public class GetAllProductsQuery : IRequest<List<ProductReadModel>>;