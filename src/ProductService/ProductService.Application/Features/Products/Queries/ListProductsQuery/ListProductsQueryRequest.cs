using MediatR;
using ProductService.Application.DTOs.ListProductsQuery;

namespace ProductService.Application.Features.Products.Queries.ListProductsQuery
{
    public class ListProductsQueryRequest : IRequest<ListProductsQueryResponse>
    {
    }
}
