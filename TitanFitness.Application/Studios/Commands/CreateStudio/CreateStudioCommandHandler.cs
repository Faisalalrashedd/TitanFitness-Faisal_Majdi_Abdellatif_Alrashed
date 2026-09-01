using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.Studios.Commands.CreateStudio
{
    public class CreateStudioCommandHandler
    {
        private readonly IGenericRepository<Studio> _studioRepository;
        private readonly IGenericRepository<Branch> _branchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateStudioCommandHandler(
            IGenericRepository<Studio> studioRepository,
            IGenericRepository<Branch> branchRepository,
            IUnitOfWork unitOfWork)
        {
            _studioRepository = studioRepository;
            _branchRepository = branchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int?> Handle(CreateStudioCommand command)
        {
            var branch = await _branchRepository.GetByIdAsync(command.BranchId);

            if (branch == null)
            {
                return null;
            }

            var studio = new Studio
            {
                StudioName = command.StudioName,
                BranchId = command.BranchId,
                Capacity = command.Capacity
            };

            await _studioRepository.AddAsync(studio);
            await _unitOfWork.SaveChangesAsync();

            return studio.StudioId;
        }
    }
}
