using System.Threading.Tasks;

namespace Core.DomainServices
{
    public interface IUnitOfWork
    {
        int Save();
        Task<int> SaveAsync();
    }
}
