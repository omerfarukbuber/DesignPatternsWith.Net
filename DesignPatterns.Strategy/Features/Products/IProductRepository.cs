﻿using DesignPatterns.Strategy.Features.Products;
using DesignPatterns.Strategy.Shared.Results;

namespace DesignPatterns.Strategy.Features.Products;

public interface IProductRepository
{
    Task<Result<List<Product>>> GetAllProductsAsync(CancellationToken cancellationToken = default);
    Task<Result<Product>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<Product>> SaveAsync(CreateProductDto createProduct, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(UpdateProductDto updateProduct, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);


}