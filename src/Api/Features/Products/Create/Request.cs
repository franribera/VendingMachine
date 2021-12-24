using Api.Domain.Entities;
using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Products.Create;

public class CreateProductRequest : IRequest<CreateProductResponse>
{
    public long UserId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
}

public class CreateProductResponse
{
    public long Id { get; }
    public string Name { get; }
    public int Quantity { get; }
    public int Price { get; }

    public CreateProductResponse(long id, string name, int quantity, int price)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Price = price;
    }
}

public class CreateProductRequestHandler : IRequestHandler<CreateProductRequest, CreateProductResponse>
{
    private readonly VendingMachineDbContext _dbContext;

    public CreateProductRequestHandler(VendingMachineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var sameNameProduct = await _dbContext.Products.SingleOrDefaultAsync(p => p.Name == request.Name, cancellationToken);

        if (sameNameProduct == null)
        {
            var product = new Product(request.Name, request.UserId, request.Quantity, request.Price);

            await _dbContext.Products.AddAsync(product, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateProductResponse(product.Id, product.Name, product.Quantity, product.Price.Amount);
        }

        throw new InvalidOperationException($"Product name {request.Name} already in use.");
    }
}