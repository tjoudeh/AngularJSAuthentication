using AngularJSAuthentication.Data.Entities;
using AngularJSAuthentication.Data.Infrastructure;
using AngularJSAuthentication.Data.Interface;

namespace AngularJSAuthentication.Data.Repository
{
    public class ClientRepoistory : MongoRepository<Client, string>, IClientRepoistory
    {
        public Client FindClient(string clientId)
        {
            throw new System.NotImplementedException();
        }






    }
}
