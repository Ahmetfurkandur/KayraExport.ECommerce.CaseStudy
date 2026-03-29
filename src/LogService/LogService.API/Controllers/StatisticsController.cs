using Asp.Versioning;
using LogService.Application.Features.Statistics.Queries.GetStatisticByIdQuery;
using LogService.Application.Features.Statistics.Queries.ListAllStatisticsQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogService.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize("admin")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IMediator mediator;

        public StatisticsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListAllStatistics([FromQuery] ListAllStatisticsQueryRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpGet("/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStatisticById([FromRoute] string id)
        {
            return Ok(await mediator.Send(new GetStatisticByIdQueryRequest(id)));
        }
    }
}
