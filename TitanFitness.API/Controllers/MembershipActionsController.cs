using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TitanFitness.Application.GuestPasses.Commands.IssueGuestPass;
using TitanFitness.Application.GuestPasses.Commands.UseGuestPass;
using TitanFitness.Application.GuestPasses.Queries.GetGuestPasses;
using TitanFitness.Application.Memberships.Commands.CancelMembership;
using TitanFitness.Application.Memberships.Commands.RenewMembership;

namespace TitanFitness.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class MembershipActionsController : ControllerBase
    {
        private readonly RenewMembershipCommandHandler _renewHandler;
        private readonly CancelMembershipCommandHandler _cancelHandler;
        private readonly IssueGuestPassCommandHandler _issuePassHandler;
        private readonly UseGuestPassCommandHandler _usePassHandler;
        private readonly GetGuestPassesQueryHandler _getPassesHandler;
        private readonly IValidator<RenewMembershipCommand> _renewValidator;
        private readonly IValidator<IssueGuestPassCommand> _issuePassValidator;

        public MembershipActionsController(
            RenewMembershipCommandHandler renewHandler,
            CancelMembershipCommandHandler cancelHandler,
            IssueGuestPassCommandHandler issuePassHandler,
            UseGuestPassCommandHandler usePassHandler,
            GetGuestPassesQueryHandler getPassesHandler,
            IValidator<RenewMembershipCommand> renewValidator,
            IValidator<IssueGuestPassCommand> issuePassValidator)
        {
            _renewHandler = renewHandler;
            _cancelHandler = cancelHandler;
            _issuePassHandler = issuePassHandler;
            _usePassHandler = usePassHandler;
            _getPassesHandler = getPassesHandler;
            _renewValidator = renewValidator;
            _issuePassValidator = issuePassValidator;
        }

        [HttpPost("memberships/{id}/renew")]
        public async Task<IActionResult> RenewMembership(
            int id,
            RenewMembershipCommand command)
        {
            command.MembershipId = id;

            var validationResult = await _renewValidator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _renewHandler.Handle(command);

            if (!result.Success)
            {
                return BadRequest(new { Message = result.Error });
            }

            return Ok(new
            {
                MembershipId = result.MembershipId,
                Message = "Membership renewed successfully"
            });
        }

        [HttpPost("memberships/{id}/cancel")]
        public async Task<IActionResult> CancelMembership(int id)
        {
            var cancelled = await _cancelHandler.Handle(
                new CancelMembershipCommand
                {
                    MembershipId = id
                });

            if (!cancelled)
            {
                return NotFound();
            }

            return Ok(new
            {
                Message = "Membership cancelled successfully"
            });
        }

        [HttpGet("memberships/{membershipId}/guest-passes")]
        public async Task<IActionResult> GetGuestPasses(int membershipId)
        {
            var passes = await _getPassesHandler.Handle(
                new GetGuestPassesQuery
                {
                    MembershipId = membershipId
                });

            return Ok(passes);
        }

        [HttpPost("memberships/{membershipId}/guest-passes")]
        public async Task<IActionResult> IssueGuestPass(
            int membershipId,
            IssueGuestPassCommand command)
        {
            command.MembershipId = membershipId;

            var validationResult =
                await _issuePassValidator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _issuePassHandler.Handle(command);

            if (!result.Success)
            {
                return BadRequest(new { Message = result.Error });
            }

            return Ok(new
            {
                GuestPassId = result.GuestPassId,
                Message = "Guest pass issued successfully"
            });
        }

        [HttpPost("guest-passes/{id}/use")]
        public async Task<IActionResult> UseGuestPass(int id)
        {
            var used = await _usePassHandler.Handle(
                new UseGuestPassCommand
                {
                    GuestPassId = id
                });

            if (!used)
            {
                return BadRequest(new
                {
                    Message = "Guest pass not found or already used"
                });
            }

            return Ok(new
            {
                Message = "Guest pass used successfully"
            });
        }
    }
}
