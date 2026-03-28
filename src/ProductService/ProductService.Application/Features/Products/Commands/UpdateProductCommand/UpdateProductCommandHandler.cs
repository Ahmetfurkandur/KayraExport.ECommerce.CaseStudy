using ErrorHandling;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ProductService.Application.Interfaces.Repositories.Products;
using ProductService.Domain.Events;

namespace ProductService.Application.Features.Products.Commands.UpdateProductCommand
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        private readonly IProductCommandRepository commandRepository;
        private readonly IProductQueryRepository queryRepository; //change tracker ile update yapmak için IProductQueryRepository'yi inject ediyoruz
        private readonly ILogger<UpdateProductCommandHandler> logger;
        private readonly IDistributedCache cache;
        private readonly IPublishEndpoint publishEndpoint;
        public UpdateProductCommandHandler(IProductCommandRepository commandRepository, IProductQueryRepository queryRepository, ILogger<UpdateProductCommandHandler> logger, IDistributedCache cache, IPublishEndpoint publishEndpoint)
        {
            this.commandRepository = commandRepository;
            this.queryRepository = queryRepository;
            this.logger = logger;
            this.cache = cache;
            this.publishEndpoint = publishEndpoint;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await queryRepository.GetByIdAsync(request.Id);

            //product null ise
            ErrorBuilder.Create(404)
                    .WithTitle("Ürün Bulunamadı")
                    .WithDescription("Güncellemekte olduğunuz ürün sistemde bulunamadı.")
                    .ThrowIfNull(product);


            //Change Tracker ile güncelleme işlemi
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;
            product.ProductCode = request.ProductCode;
            product.CategoryId = request.CategoryId;

            var result = await commandRepository.SaveAsync(cancellationToken);

            if (result > 0)
            {
                logger.LogInformation("Product {Id} added successfully", product);
                //Task'ta belirtilen açıklamaya göre yalnızca event fırlatıyorum, Consumer eklemiyorum. Diğer Mikro servisin Consumer'ı çalıştığı anda consume eder.
                await publishEndpoint.Publish(new ProductUpdatedEvent(
                    product.Id, product.Name, product.Price, DateTime.UtcNow
                    ));
            }

            await cache.RemoveAsync("products:all", cancellationToken);

            return new()
            {
                Id = request.Id,
                UpdatedAt = (DateTime)product.UpdatedDate!
            };
        }
    }
}
