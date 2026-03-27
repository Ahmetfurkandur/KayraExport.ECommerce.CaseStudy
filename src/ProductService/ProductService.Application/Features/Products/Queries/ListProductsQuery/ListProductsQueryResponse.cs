namespace ProductService.Application.DTOs.ListProductsQuery
{
    public class ListProductsQueryResponse
    {
        public List<ProductDto> Data { get; set; }
        public int Size { get; set; }
    }
}