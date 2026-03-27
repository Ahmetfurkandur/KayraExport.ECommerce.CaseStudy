using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ProductService.Application.Interfaces.Repositories.Products;

namespace ProductService.Application.Features.Products.Commands.UpdateProductCommand
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        private readonly IProductCommandRepository commandRepository;
        private readonly IProductQueryRepository queryRepository; //change tracker ile update yapmak için IProductQueryRepository'yi inject ediyoruz
        private readonly ILogger<UpdateProductCommandHandler> logger;
        private readonly IDistributedCache cache;

        public UpdateProductCommandHandler(IProductCommandRepository commandRepository, IProductQueryRepository queryRepository, ILogger<UpdateProductCommandHandler> logger, IDistributedCache cache)
        {
            this.commandRepository = commandRepository;
            this.queryRepository = queryRepository;
            this.logger = logger;
            this.cache = cache;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await queryRepository.GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new KeyNotFoundException("Güncellenecek ürün sistemde bulunamadı.");
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;
            product.ProductCode = request.ProductCode;
            product.CategoryId = request.CategoryId;

            var result = await commandRepository.SaveAsync(cancellationToken);

            if (result > 0)
            {
                logger.LogInformation("Product {Id} added successfully", product); //TODO: event fırlat
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
