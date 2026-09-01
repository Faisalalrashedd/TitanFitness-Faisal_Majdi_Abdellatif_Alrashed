using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TitanFitness.Application.Memberships.Commands.ChangeMembershipPlan;

namespace TitanFitness.API.Controllers
{
    [ApiController]
    [Route("api/memberships")]
    public class ChangeMembershipPlanController : ControllerBase
    {
        private readonly ChangeMembershipPlanCommandHandler _handler;
        private readonly IValidator<ChangeMembershipPlanCommand> _validator;

        public ChangeMembershipPlanController(
            ChangeMembershipPlanCommandHandler handler,
            IValidator<ChangeMembershipPlanCommand> validator)
        {
            _handler = handler;
            _validator = validator;
        }

        [HttpPost("{id}/change-plan")]
        public async Task<IActionResult> ChangePlan(
            int id,
            ChangeMembershipPlanCommand command)
        {
            command.MembershipId = id;

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
                MembershipId = result.MembershipId,
                Message = "Membership plan changed successfully"
            });
        }
    }
}
