using TitanFitness.Application.Branches.Dtos;
using TitanFitness.Application.Interfaces;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.Branches.Queries.GetBranches
{
    public class GetBranchesQueryHandler
    {
        private readonly IGenericRepository<Branch> _branchRepository;

        public GetBranchesQueryHandler(IGenericRepository<Branch> branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<List<BranchDto>> Handle(GetBranchesQuery query)
        {
            var branches = await _branchRepository.GetAllAsync();

            return branches.Select(branch => new BranchDto
            {
                BranchId = branch.BranchId,
                BranchName = branch.BranchName,
                Address = branch.Address,
                OpeningTime = branch.OpeningTime,
                ClosingTime = branch.ClosingTime
            }).ToList();
        }
    }
}
