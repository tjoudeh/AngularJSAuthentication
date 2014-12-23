using System;
using System.Threading.Tasks;
using AngularJSAuthentication.API.Models;
using AngularJSAuthentication.Data.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AngularJSAuthentication.API.Data
{
    public interface IAuthRepository : IDisposable
    {
        Task<IdentityResult> RegisterUser(UserModel userModel);
        
        Task<IdentityUser> FindUser(string userName, string password);
        
        Client FindClient(string clientId);
        Task<bool> AddRefreshToken(RefreshToken token);

        Task<bool> RemoveRefreshToken(string refreshTokenId);

        Task<IdentityUser> FindAsync(UserLoginInfo loginInfo);

        Task<IdentityResult> CreateAsync(IdentityUser user);

        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);


    }
}