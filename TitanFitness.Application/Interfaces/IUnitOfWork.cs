namespace TitanFitness.Application.Interfaces
{
    // saves all database changes together
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}