using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TitanFitness.Application.Plans.Commands.CreatePlan;
using TitanFitness.Application.Plans.Commands.UpdatePlan;
using TitanFitness.Application.Plans.Queries.GetPlans;
using TitanFitness.Application.Plans.Queries.GetPlanByID;

namespace TitanFitness.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlansController : ControllerBase
    {
        private readonly CreatePlanCommandHandler _createPlanHandler;
        private readonly GetPlansQueryHandler _getPlansHandler;
        private readonly GetPlanByIdQueryHandler _getPlanByIdHandler;
        private readonly UpdatePlanCommandHandler _updatePlanHandler;
        private readonly IValidator<CreatePlanCommand> _createPlanValidator;
        private readonly IValidator<UpdatePlanCommand> _updatePlanValidator;

        public PlansController(
            CreatePlanCommandHandler createPlanHandler,
            GetPlansQueryHandler getPlansHandler,
            GetPlanByIdQueryHandler getPlanByIdHandler,
            UpdatePlanCommandHandler updatePlanHandler,
            IValidator<CreatePlanCommand> createPlanValidator,
            IValidator<UpdatePlanCommand> updatePlanValidator)
        {
            _createPlanHandler = createPlanHandler;
            _getPlansHandler = getPlansHandler;
            _getPlanByIdHandler = getPlanByIdHandler;
            _updatePlanHandler = updatePlanHandler;
            _createPlanValidator = createPlanValidator;
            _updatePlanValidator = updatePlanValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlans()
        {
            var plans = await _getPlansHandler.Handle(new GetPlansQuery());

            return Ok(plans);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlanById(int id)
        {
            var plan = await _getPlanByIdHandler.Handle(new GetPlanByIdQuery
            {
                PlanId = id
            });

            if (plan == null)
            {
                return NotFound();
            }

            return Ok(plan);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlan(CreatePlanCommand command)
        {
            var validationResult =
                await _createPlanValidator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var planId = await _createPlanHandler.Handle(command);

            return Ok(new
            {
                PlanId = planId,
                Message = "Plan created successfully"
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlan(
            int id,
            UpdatePlanCommand command)
        {
            command.PlanId = id;

            var validationResult =
                await _updatePlanValidator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var updated = await _updatePlanHandler.Handle(command);

            if (!updated)
            {
                return NotFound();
            }

            return Ok(new
            {
                Message = "Plan updated successfully"
            });
        }
    }
}