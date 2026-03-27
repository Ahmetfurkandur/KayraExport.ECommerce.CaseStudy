using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ProductService.Application.Interfaces.Repositories.Products;
using ProductService.Domain.Entities;
using ProductService.Domain.Events;

namespace ProductService.Application.Features.Products.Commands.CreateProductCommand
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IProductCommandRepository commandRepository;
        private readonly ILogger<CreateProductCommandHandler> logger;
        private readonly IDistributedCache cache;
        private readonly IPublishEndpoint publishEndpoint;


        public CreateProductCommandHandler(IProductCommandRepository commandRepository, ILogger<CreateProductCommandHandler> logger, IDistributedCache cache, IPublishEndpoint publishEndpoint)
        {
            this.commandRepository = commandRepository;
            this.logger = logger;
            this.cache = cache;
            this.publishEndpoint = publishEndpoint;
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
                logger.LogInformation("Product {Id} added successfully", product.Id);
                //Task'ta belirtilen açıklamaya göre yalnızca event fırlatıyorum, Consumer eklemiyorum. Diğer Mikro servisin Consumer'ı çalıştığı anda consume eder.
                await publishEndpoint.Publish(new ProductCreatedEvent(
                    product.Id, product.Name, product.Price, DateTime.UtcNow
                    ));
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
