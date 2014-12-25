using System;
using System.Threading.Tasks;
using AngularJSAuthentication.Data.Entities;
using AngularJSAuthentication.Data.Models;
using Microsoft.AspNet.Identity;


namespace AngularJSAuthentication.API.Data
{
    public interface IAuthRepository : IDisposable
    {
        Task<IdentityResult> RegisterUser(UserModel userModel);

        Task<IUser> FindUser(string userName, string password);
        
        Client FindClient(string clientId);
        
        Task<bool> AddRefreshToken(RefreshToken token);

        Task<bool> RemoveRefreshToken(string refreshTokenId);

        Task<IUser> FindAsync(UserLoginInfo loginInfo);

        Task<IdentityResult> CreateAsync(IUser user);

        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);

        Task<RefreshToken> FindRefreshToken(string refreshTokenId);

    }
}