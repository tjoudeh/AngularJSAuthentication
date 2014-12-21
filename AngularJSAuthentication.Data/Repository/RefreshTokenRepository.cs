using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngularJSAuthentication.Data.Entities;
using AngularJSAuthentication.Data.Infrastructure;
using AngularJSAuthentication.Data.Interface;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace AngularJSAuthentication.Data.Repository
{
    public class RefreshTokenRepository 
        : MongoRepository<RefreshToken, string>, IRefreshTokenRepository
    {
        private bool _disposed;

        private readonly MongoRepository<RefreshToken> _repository;

        public  RefreshTokenRepository(MongoUrl mongoUrl)
        {
            _repository = new MongoRepository<RefreshToken>(); 
        }

        public RefreshTokenRepository(string connectionString)
            : base(connectionString)
        {
            _repository = new MongoRepository<RefreshToken>(connectionString);

            var pack = new ConventionPack();
            pack.Add(new CamelCaseElementNameConvention());
            pack.Add(new IgnoreIfNullConvention(true));

            ConventionRegistry.Register("camel case", pack, t => true);
        }

        public Task<bool> AddRefreshToken(RefreshToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            throw new System.NotImplementedException();
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     Throws if disposed.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException"></exception>
        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

    }
}
