using Microsoft.AspNetCore.Mvc;
using TitanFitness.Application.Dashboard.Queries.GetDashboard;

namespace TitanFitness.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly GetDashboardQueryHandler _handler;

        public DashboardController(GetDashboardQueryHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboard =
                await _handler.Handle(new GetDashboardQuery());

            return Ok(dashboard);
        }
    }
}
