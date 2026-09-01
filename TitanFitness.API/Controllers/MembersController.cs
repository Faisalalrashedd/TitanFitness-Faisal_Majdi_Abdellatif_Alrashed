using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TitanFitness.Application.Member.Queries.GetMembersByID;
using TitanFitness.Application.Members.Commands.CreateMember;
using TitanFitness.Application.Members.Commands.UpdateMember;
using TitanFitness.Application.Members.Queries.GetMemberById;
using TitanFitness.Application.Members.Queries.GetMembers;

namespace TitanFitness.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly CreateMemberCommandHandler _createMemberHandler;
        private readonly UpdateMemberCommandHandler _updateMemberHandler;
        private readonly GetMembersQueryHandler _getMembersHandler;
        private readonly GetMemberByIdQueryHandler _getMemberByIdHandler;
        private readonly IValidator<CreateMemberCommand> _createMemberValidator;
        private readonly IValidator<UpdateMemberCommand> _updateMemberValidator;

        public MembersController(
            CreateMemberCommandHandler createMemberHandler,
            UpdateMemberCommandHandler updateMemberHandler,
            GetMembersQueryHandler getMembersHandler,
            GetMemberByIdQueryHandler getMemberByIdHandler,
            IValidator<CreateMemberCommand> createMemberValidator,
            IValidator<UpdateMemberCommand> updateMemberValidator)
        {
            _createMemberHandler = createMemberHandler;
            _updateMemberHandler = updateMemberHandler;
            _getMembersHandler = getMembersHandler;
            _getMemberByIdHandler = getMemberByIdHandler;
            _createMemberValidator = createMemberValidator;
            _updateMemberValidator = updateMemberValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetMembers()
        {
            var members =
                await _getMembersHandler.Handle(new GetMembersQuery());

            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberById(int id)
        {
            var member =
                await _getMemberByIdHandler.Handle(new GetMemberByIdQuery
                {
                    MemberId = id
                });

            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(
            CreateMemberCommand command)
        {
            var validationResult =
                await _createMemberValidator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var memberId =
                await _createMemberHandler.Handle(command);

            if (memberId == null)
            {
                return BadRequest(new
                {
                    Message = "Membership number already exists"
                });
            }

            return Ok(new
            {
                MemberId = memberId,
                Message = "Member created successfully"
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(
            int id,
            UpdateMemberCommand command)
        {
            command.MemberId = id;

            var validationResult =
                await _updateMemberValidator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var updated =
                await _updateMemberHandler.Handle(command);

            if (!updated)
            {
                return NotFound();
            }

            return Ok(new
            {
                Message = "Member updated successfully"
            });
        }
    }
}