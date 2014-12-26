using System.Collections.Generic;
using System.Threading.Tasks;
using AngularJSAuthentication.Data.Entities;

namespace AngularJSAuthentication.Data.Interface
{
    public interface IRefreshTokenRepository
    {
        Task<bool> AddRefreshToken(RefreshToken token);

        Task<bool> RemoveRefreshToken(string refreshTokenId);

        Task<bool> RemoveRefreshToken(RefreshToken refreshToken);

        Task<RefreshToken> FindRefreshToken(string refreshTokenId);

        List<RefreshToken> GetAllRefreshTokens();
    }
}
