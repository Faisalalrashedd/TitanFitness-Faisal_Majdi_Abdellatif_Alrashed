using TitanFitness.Application.Interfaces;
using TitanFitness.Application.Studios.Dtos;
using TitanFitness.Domain.Entities;

namespace TitanFitness.Application.Studios.Queries.GetStudiosByBranch
{
    public class GetStudiosByBranchQueryHandler
    {
        private readonly IGenericRepository<Studio> _studioRepository;

        public GetStudiosByBranchQueryHandler(IGenericRepository<Studio> studioRepository)
        {
            _studioRepository = studioRepository;
        }

        public async Task<List<StudioDto>> Handle(GetStudiosByBranchQuery query)
        {
            var studios = await _studioRepository.FindAsync(
                x => x.BranchId == query.BranchId);

            return studios.Select(studio => new StudioDto
            {
                StudioId = studio.StudioId,
                StudioName = studio.StudioName,
                BranchId = studio.BranchId,
                Capacity = studio.Capacity
            }).ToList();
        }
    }
}
