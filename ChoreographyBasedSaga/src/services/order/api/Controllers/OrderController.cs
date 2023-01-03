using application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost("/Order")]
        public async Task<IActionResult> PlaceOrder(PlaceOrder.Command request)
        {
            return Ok(await this._mediator.Send(request));
        }

        [HttpGet("/Orders")]
        public async Task<IActionResult> GetOrders()
        {
            return Ok(await this._mediator.Send(new GetOrders.Query()));
        }
    }
}
