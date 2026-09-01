using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TitanFitness.Application.Freezes.Commands.CreateFreeze;

namespace TitanFitness.API.Controllers
{
    [ApiController]
    [Route("api/memberships/{membershipId}/freezes")]
    public class FreezesController : ControllerBase
    {
        private readonly CreateFreezeCommandHandler _handler;
        private readonly IValidator<CreateFreezeCommand> _validator;

        public FreezesController(
            CreateFreezeCommandHandler handler,
            IValidator<CreateFreezeCommand> validator)
        {
            _handler = handler;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFreeze(
            int membershipId,
            CreateFreezeCommand command)
        {
            command.MembershipId = membershipId;

            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _handler.Handle(command);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    Message = result.Error
                });
            }

            return Ok(new
            {
                FreezeId = result.FreezeId,
                Message = "Membership frozen successfully"
            });
        }
    }
}
