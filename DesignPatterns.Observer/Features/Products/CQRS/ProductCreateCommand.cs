using DesignPatterns.Observer.Shared.Results;
using MediatR;

namespace DesignPatterns.Observer.Features.Products.CQRS;

public sealed record ProductCreateCommand(
    string Name,
    string Description,
    decimal Price) : IRequest<Result<Product>>;