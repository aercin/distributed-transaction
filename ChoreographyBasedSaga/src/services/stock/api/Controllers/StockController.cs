using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StockController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        
        [HttpGet("Stocks")]
        public async Task<IActionResult> GetStocks()
        {
            return Ok(await this._mediator.Send(new application.GetStocks.Query()));
        }
    }
}
