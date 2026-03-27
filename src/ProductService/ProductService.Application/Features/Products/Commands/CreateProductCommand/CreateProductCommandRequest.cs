using MediatR;

namespace ProductService.Application.Features.Products.Commands.CreateProductCommand
{
    public record CreateProductCommandRequest : IRequest<CreateProductCommandResponse>
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public int StockQuantity { get; init; }
        public string ProductCode { get; init; }
        public int CategoryId { get; init; }
    }
}
