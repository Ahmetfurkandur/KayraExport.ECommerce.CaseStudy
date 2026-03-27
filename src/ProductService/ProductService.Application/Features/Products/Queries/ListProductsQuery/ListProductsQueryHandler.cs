using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProductService.Application.DTOs;
using ProductService.Application.DTOs.ListProductsQuery;
using ProductService.Application.Interfaces.Repositories.Products;
using System.Text.Json;

namespace ProductService.Application.Features.Products.Queries.ListProductsQuery
{
    public class ListProductsQueryHandler : IRequestHandler<ListProductsQueryRequest, ListProductsQueryResponse>
    {
        private readonly IProductQueryRepository productQueryRepository;
        private readonly IDistributedCache cache;
        public string CacheKey { get; set; } = "products:all"; //magic string anti pattern'ini engellemek için cache adını property'e yazıyoruz

        public ListProductsQueryHandler(IProductQueryRepository productQueryRepository, IDistributedCache cache)
        {
            this.productQueryRepository = productQueryRepository;
            this.cache = cache;
        }

        public async Task<ListProductsQueryResponse> Handle(ListProductsQueryRequest request, CancellationToken cancellationToken)
        {

            //cache kontrolü yapıyoruz
            var cached = await cache.GetStringAsync(CacheKey, cancellationToken);
            if (cached is not null)
            {
                return new()
                {
                    Data = JsonSerializer.Deserialize<List<ProductDto>>(cached)!,
                    Size = JsonSerializer.Deserialize<List<ProductDto>>(cached)!.Count()
                };
            }

            //cache'de yoksa db den çekiyoruz
            var products = productQueryRepository.GetAll();
            var dtos = products.Select(p => new ProductDto()
            {
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                ProductCode = p.ProductCode,
                CategoryId = p.CategoryId
            }).ToList();

            //Cache'e yazıyoruz
            await cache.SetStringAsync(CacheKey, JsonSerializer.Serialize(dtos), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), //TODO: test amaçlı süreler uzun tutuldu, prod için makul değerler ayarla
                SlidingExpiration = TimeSpan.FromMinutes(10)
            }, cancellationToken);


            return new ListProductsQueryResponse()
            {
                Data = dtos,
                Size = products.Count()
            };
        }
    }
}
