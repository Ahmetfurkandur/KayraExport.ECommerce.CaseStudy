using Asp.Versioning;
using LogService.Application.Features.Events.Queries.GetEventByIdQuery;
using LogService.Application.Features.Events.Queries.ListAllEventsQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogService.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize("admin")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IMediator mediator;

        public EventsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListAllEvents([FromQuery] ListAllEventsQueryRequest request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventById([FromRoute] string id)
        {
            return Ok(await mediator.Send(new GetEventByIdQueryRequest(id)));
        }
    }
}
