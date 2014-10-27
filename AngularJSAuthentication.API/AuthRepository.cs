using AngularJSAuthentication.API.Entities;
using AngularJSAuthentication.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularJSAuthentication.API
{

    public class AuthRepository : IDisposable
    {
        private AuthContext _ctx;

        private UserManager<IdentityUser> _userManager;

        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public async Task<IdentityUser> FindUserByName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public Client FindClient(string clientId)
        {
            var client = _ctx.Clients.Find(clientId);

            return client;
        }

        public bool AddRefreshToken(RefreshToken token)
        {
            _ctx.RefreshTokens.Add(token);
            return _ctx.SaveChanges() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
           var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

           if (refreshToken != null) {
               return RemoveRefreshToken(refreshToken);
           }

           return false;
        }

        public bool RemoveRefreshToken(params RefreshToken[] refreshTokens)
        {
            foreach (var refreshToken in refreshTokens)
            {
                _ctx.RefreshTokens.Remove(refreshToken);
            }
             return _ctx.SaveChanges() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public IQueryable<RefreshToken> FindRefreshTokens(string clientId, string userName,
            string userAgent,
            DateTime? maxExpiresUtc = null)
        {
            var query = _ctx.RefreshTokens.AsQueryable()
                .Where(t => t.UserName == userName && t.ClientId == clientId);

            if (userAgent != null)
            {
                query = query.Where(t => t.UserAgent == userAgent);
            }
            if (maxExpiresUtc.HasValue)
            {
                query = query.Where(t => t.ExpiresUtc < maxExpiresUtc);
            }

            return query;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
             return  _ctx.RefreshTokens.ToList();
        }

        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            IdentityUser user = await _userManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateAsync(IdentityUser user)
        {
            var result = await _userManager.CreateAsync(user);

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}