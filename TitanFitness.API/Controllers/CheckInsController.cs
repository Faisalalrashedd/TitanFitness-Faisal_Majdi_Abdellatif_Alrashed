using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TitanFitness.Application.CheckIns.Commands.CreateCheckIn;

namespace TitanFitness.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckInsController : ControllerBase
    {
        private readonly CreateCheckInCommandHandler _handler;
        private readonly IValidator<CreateCheckInCommand> _validator;

        public CheckInsController(
            CreateCheckInCommandHandler handler,
            IValidator<CreateCheckInCommand> validator)
        {
            _handler = handler;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckIn(
            CreateCheckInCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _handler.Handle(command);

            if (result == null)
            {
                return BadRequest(new
                {
                    Message = "Member or branch not found"
                });
            }

            return Ok(result);
        }
    }
}
