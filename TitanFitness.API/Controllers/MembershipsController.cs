using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TitanFitness.Application.Memberships.Commands.PurchaseMembership;
using TitanFitness.Application.Memberships.Queries.GetMembershipsByMember;

namespace TitanFitness.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class MembershipsController : ControllerBase
    {
        private readonly PurchaseMembershipCommandHandler _purchaseHandler;
        private readonly GetMembershipsByMemberQueryHandler _getMembershipsHandler;
        private readonly IValidator<PurchaseMembershipCommand> _purchaseValidator;

        public MembershipsController(
            PurchaseMembershipCommandHandler purchaseHandler,
            GetMembershipsByMemberQueryHandler getMembershipsHandler,
            IValidator<PurchaseMembershipCommand> purchaseValidator)
        {
            _purchaseHandler = purchaseHandler;
            _getMembershipsHandler = getMembershipsHandler;
            _purchaseValidator = purchaseValidator;
        }

        [HttpPost("members/{memberId}/memberships")]
        public async Task<IActionResult> PurchaseMembership(
            int memberId,
            PurchaseMembershipCommand command)
        {
            command.MemberId = memberId;

            var validationResult =
                await _purchaseValidator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _purchaseHandler.Handle(command);

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
                Message = "Membership created successfully"
            });
        }

        [HttpGet("members/{memberId}/memberships")]
        public async Task<IActionResult> GetMemberships(int memberId)
        {
            var memberships =
                await _getMembershipsHandler.Handle(
                    new GetMembershipsByMemberQuery
                    {
                        MemberId = memberId
                    });

            return Ok(memberships);
        }
    }
}
