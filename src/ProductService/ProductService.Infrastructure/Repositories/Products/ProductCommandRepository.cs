using ProductService.Application.Interfaces.Repositories.Products;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Contexts;

namespace ProductService.Infrastructure.Repositories.Products
{
    internal class ProductCommandRepository : CommandRepository<Product>, IProductCommandRepository
    {
        public ProductCommandRepository(ProductDbContext context) : base(context)
        { }
    }
}
