using System.Threading.Tasks;
using AngularJSAuthentication.Data.Entities;

namespace AngularJSAuthentication.Data.Interface
{
    public interface IClientRepository
    {
        Task<Client> FindClient(string clientId);

        Task SaveClient(Client client);
    }
}
