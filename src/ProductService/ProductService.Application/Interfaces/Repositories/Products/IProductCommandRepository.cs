using ProductService.Domain.Entities;

namespace ProductService.Application.Interfaces.Repositories.Products
{
    public interface IProductCommandRepository : ICommandRepository<Product>
    {
    }
}
