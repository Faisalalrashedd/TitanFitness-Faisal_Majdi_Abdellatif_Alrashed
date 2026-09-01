using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TitanFitness.Application.Branches.Commands.CreateBranch;
using TitanFitness.Application.Branches.Queries.GetBranches;
using TitanFitness.Application.Studios.Commands.CreateStudio;
using TitanFitness.Application.Studios.Queries.GetStudiosByBranch;

namespace TitanFitness.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchesController : ControllerBase
    {
        private readonly CreateBranchCommandHandler _createBranchHandler;
        private readonly GetBranchesQueryHandler _getBranchesHandler;
        private readonly CreateStudioCommandHandler _createStudioHandler;
        private readonly GetStudiosByBranchQueryHandler _getStudiosHandler;
        private readonly IValidator<CreateBranchCommand> _branchValidator;
        private readonly IValidator<CreateStudioCommand> _studioValidator;

        public BranchesController(
            CreateBranchCommandHandler createBranchHandler,
            GetBranchesQueryHandler getBranchesHandler,
            CreateStudioCommandHandler createStudioHandler,
            GetStudiosByBranchQueryHandler getStudiosHandler,
            IValidator<CreateBranchCommand> branchValidator,
            IValidator<CreateStudioCommand> studioValidator)
        {
            _createBranchHandler = createBranchHandler;
            _getBranchesHandler = getBranchesHandler;
            _createStudioHandler = createStudioHandler;
            _getStudiosHandler = getStudiosHandler;
            _branchValidator = branchValidator;
            _studioValidator = studioValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetBranches()
        {
            var branches = await _getBranchesHandler.Handle(new GetBranchesQuery());
            return Ok(branches);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBranch(CreateBranchCommand command)
        {
            var validationResult = await _branchValidator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var branchId = await _createBranchHandler.Handle(command);

            return Ok(new
            {
                BranchId = branchId,
                Message = "Branch created successfully"
            });
        }

        [HttpGet("{branchId}/studios")]
        public async Task<IActionResult> GetStudios(int branchId)
        {
            var studios = await _getStudiosHandler.Handle(
                new GetStudiosByBranchQuery
                {
                    BranchId = branchId
                });

            return Ok(studios);
        }

        [HttpPost("{branchId}/studios")]
        public async Task<IActionResult> CreateStudio(
            int branchId,
            CreateStudioCommand command)
        {
            command.BranchId = branchId;

            var validationResult = await _studioValidator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var studioId = await _createStudioHandler.Handle(command);

            if (studioId == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                StudioId = studioId,
                Message = "Studio created successfully"
            });
        }
    }
}
