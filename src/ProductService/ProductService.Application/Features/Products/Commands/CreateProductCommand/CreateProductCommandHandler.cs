using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ProductService.Application.Interfaces.Repositories.Products;
using ProductService.Domain.Entities;

namespace ProductService.Application.Features.Products.Commands.CreateProductCommand
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IProductCommandRepository commandRepository;
        private readonly ILogger<CreateProductCommandHandler> logger;
        private readonly IDistributedCache cache;

        public CreateProductCommandHandler(IProductCommandRepository commandRepository, ILogger<CreateProductCommandHandler> logger, IDistributedCache cache)
        {
            this.commandRepository = commandRepository;
            this.logger = logger;
            this.cache = cache;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await commandRepository.AddAsync(new Product()
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                ProductCode = request.ProductCode,
                CategoryId = request.CategoryId
            });

            var result = await commandRepository.SaveAsync(cancellationToken);

            if (result > 0)
            {
                logger.LogInformation("Product {Id} added successfully", product); // TODO: event fırlat
            }

            await cache.RemoveAsync("products:all", cancellationToken);

            return new()
            {
                Id = product.Id,
                CreatedDate = product.CreatedDate
            };
        }
    }
}
