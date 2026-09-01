using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TitanFitness.Application.Sessions.Commands.CreateSession;
using TitanFitness.Application.Sessions.Queries.GetSessions;

namespace TitanFitness.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController : ControllerBase
    {
        private readonly CreateSessionCommandHandler _createHandler;
        private readonly GetSessionsQueryHandler _getHandler;
        private readonly IValidator<CreateSessionCommand> _validator;

        public SessionsController(
            CreateSessionCommandHandler createHandler,
            GetSessionsQueryHandler getHandler,
            IValidator<CreateSessionCommand> validator)
        {
            _createHandler = createHandler;
            _getHandler = getHandler;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetSessions(
            int? branchId,
            DateOnly? date)
        {
            var sessions = await _getHandler.Handle(new GetSessionsQuery
            {
                BranchId = branchId,
                Date = date
            });

            return Ok(sessions);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession(
            CreateSessionCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _createHandler.Handle(command);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    Message = result.Error
                });
            }

            return Ok(new
            {
                SessionId = result.SessionId,
                Message = "Session created successfully"
            });
        }
    }
}
