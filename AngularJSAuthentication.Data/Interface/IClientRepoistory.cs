using AngularJSAuthentication.Data.Entities;

namespace AngularJSAuthentication.Data.Interface
{
    public interface IClientRepoistory
    {
        Client FindClient(string clientId);

    }
}
