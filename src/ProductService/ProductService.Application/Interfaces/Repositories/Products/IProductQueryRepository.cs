using ProductService.Domain.Entities;

namespace ProductService.Application.Interfaces.Repositories.Products
{
    public interface IProductQueryRepository : IQueryRepository<Product>
    {
    }
}
