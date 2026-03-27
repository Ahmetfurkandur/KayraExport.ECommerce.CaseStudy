using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Features.Products.Commands.CreateProductCommand;
using ProductService.Application.Features.Products.Commands.UpdateProductCommand;
using ProductService.Application.Features.Products.Queries.ListProductsQuery;

namespace ProductService.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// CQRS gereği birçok ayrı sınıfı merkezi bir noktadan yönetmemizi sağlayan Meditor pattern kullanılır. 
        /// Bu pattern'i hazır kütüphane olan MediatR kütüphanesi ile kullanmak hem geliştirme süresini kısaltır hem de Source Generators altyapısı sayesinde yüksek performans sağlar. 
        /// </summary>
        private readonly IMediator mediator;

        public ProductsController(IMediator mediator) //Dependency Injection Pattern kullanımı. Gevşek bağımlılık için kullanılır
        {
            this.mediator = mediator;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "manager")] //Ürünleri yalnızca admin veya manager rolüne sahip olanlar ekleyebilir
        public async Task<IActionResult> CreateProduct(CreateProductCommandRequest createProductCommandRequest)
        {
            return Ok(await mediator.Send(createProductCommandRequest));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await mediator.Send(new ListProductsQueryRequest()));
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "manager")] //Ürünleri yalnızca admin veya manager rolüne sahip olanlar güncelleyebilir
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductCommandRequest updateProductCommandRequest)
        {
            updateProductCommandRequest.Id = id;
            return Ok(await mediator.Send(updateProductCommandRequest));
        }
    }
}
