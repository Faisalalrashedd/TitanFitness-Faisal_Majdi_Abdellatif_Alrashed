using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.Branches.Commands.CreateBranch
{
    public class CreateBranchCommandHandler
    {
        private readonly IGenericRepository<Branch> _branchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBranchCommandHandler(
            IGenericRepository<Branch> branchRepository,
            IUnitOfWork unitOfWork)
        {
            _branchRepository = branchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateBranchCommand command)
        {
            var branch = new Branch
            {
                BranchName = command.BranchName,
                Address = command.Address,
                OpeningTime = command.OpeningTime,
                ClosingTime = command.ClosingTime
            };

            await _branchRepository.AddAsync(branch);
            await _unitOfWork.SaveChangesAsync();

            return branch.BranchId;
        }
    }
}
