using MediatR;

namespace ProductService.Application.Features.Products.Commands.UpdateProductCommand
{
    public class UpdateProductCommandRequest : IRequest<UpdateProductCommandResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ProductCode { get; set; }
        public int CategoryId { get; set; }
    }
}
