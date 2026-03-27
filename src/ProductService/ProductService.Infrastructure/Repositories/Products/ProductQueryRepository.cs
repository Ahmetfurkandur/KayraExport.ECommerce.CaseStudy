using ProductService.Application.Interfaces.Repositories.Products;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Contexts;

namespace ProductService.Infrastructure.Repositories.Products
{
    public class ProductQueryRepository : QueryRepository<Product>, IProductQueryRepository
    {
        public ProductQueryRepository(ProductDbContext context) : base(context)
        {
        }
    }
}
