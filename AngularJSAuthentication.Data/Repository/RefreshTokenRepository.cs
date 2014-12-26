﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularJSAuthentication.Data.Entities;
using AngularJSAuthentication.Data.Infrastructure;
using AngularJSAuthentication.Data.Interface;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

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
            ThrowIfDisposed();
            if (token == null)
                throw new ArgumentNullException("token");

            collection.Insert(token);
            return Task.FromResult(true);
        }

        public Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            ThrowIfDisposed();
            if (refreshToken == null)
                throw new ArgumentNullException("refreshToken");

            collection.Remove((Query.EQ("_id", ObjectId.Parse(refreshToken.Id))));
            return Task.FromResult(true);
        }

        public Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            ThrowIfDisposed();
            var bsonId = ObjectId.Parse(refreshTokenId);

            var refreshToken = collection.FindOneByIdAs<RefreshToken>(bsonId);
            return Task.FromResult(refreshToken);
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            var refreshTokens = collection.FindAllAs<RefreshToken>().ToList();
            return refreshTokens;
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
