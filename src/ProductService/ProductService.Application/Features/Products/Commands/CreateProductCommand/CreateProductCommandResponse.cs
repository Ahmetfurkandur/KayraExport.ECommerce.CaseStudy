namespace ProductService.Application.Features.Products.Commands.CreateProductCommand
{
    public class CreateProductCommandResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}