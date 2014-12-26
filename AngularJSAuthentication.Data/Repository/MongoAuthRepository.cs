﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AngularJSAuthentication.API.Data;
using AngularJSAuthentication.Data.Entities;
using AngularJSAuthentication.Data.Interface;
using AngularJSAuthentication.Data.Models;
using Microsoft.AspNet.Identity;
using MongoDB.Bson;

namespace AngularJSAuthentication.Data.Repository
{
    public class MongoAuthRepository : IAuthRepository
    {
        private readonly IClientRepository clientRepository;

        private readonly IRefreshTokenRepository refreshTokenRepository;

        private readonly IUserRepository<User> userRepository;

        public bool _disposed;

        public MongoAuthRepository(IClientRepository clientRepository, IRefreshTokenRepository refreshTokenRepository, IUserRepository<User> userRepository)
        {
            this.clientRepository = clientRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.userRepository = userRepository;

        }

        public Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> FindUser(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Client FindClient(string clientId)
        {
            ThrowIfDisposed();
            if (clientId == null)
                throw new ArgumentNullException("clientId");

            var client = clientRepository.FindClient(clientId).Result;
            return client;
        }

        public Task<bool> AddRefreshToken(RefreshToken token)
        {
            ThrowIfDisposed();
            if (token == null)
                throw new ArgumentNullException("token");

            var refreshToken = refreshTokenRepository.AddRefreshToken(token);
            return refreshToken;
        }


        public Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> FindAsync(UserLoginInfo loginInfo)
        {
            throw new NotImplementedException();
            
            //ThrowIfDisposed();
            //if (loginInfo == null)
            //    throw new ArgumentNullException("loginInfo");

            //Debug.Write(loginInfo.ToJson());


            //var user =  userRepository.FindAsync(loginInfo);
            //return user;


            // return Task.FromResult(user);
        }











        public Task<IdentityResult> CreateAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            throw new NotImplementedException();
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            throw new NotImplementedException();
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        public void Dispose()
        {
            _disposed = true;
        }


    }
}
