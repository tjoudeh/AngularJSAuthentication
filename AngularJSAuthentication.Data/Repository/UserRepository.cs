using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AngularJSAuthentication.Data.Entities;
using AngularJSAuthentication.Data.Infrastructure;
using AngularJSAuthentication.Data.Interface;
using Microsoft.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AngularJSAuthentication.Data.Repository
{
    public class UserRepository<TUser> : MongoRepository<User>, IUserLoginStore<TUser>, IUserClaimStore<TUser>, IUserRoleStore<TUser>,
       IUserPasswordStore<TUser>, IUserSecurityStampStore<TUser>, IUserStore<TUser>, IUserRepository<TUser>
       where TUser : User
    {

        public bool _disposed;

        private readonly MongoRepository<TUser> _repository;

        public UserRepository(MongoUrl mongoUrl)
        {
            _repository = new MongoRepository<TUser>(); 
        }

        public UserRepository(string connectionString) : base(connectionString)
        {
            _repository = new MongoRepository<TUser>(connectionString);

            var pack = new ConventionPack();
            pack.Add(new CamelCaseElementNameConvention());
            pack.Add(new IgnoreIfNullConvention(true));

            ConventionRegistry.Register("camel case", pack, t => true);
        }

        public Task CreateAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            collection.Insert(user);
            return Task.FromResult(user);
        }

        public Task UpdateAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            var bsonId = ObjectId.Parse(user.Id);
            collection.Update(Query.EQ("_id", bsonId), MongoDB.Driver.Builders.Update.Replace(user), UpdateFlags.Upsert);
            return Task.FromResult(user);
        }

        public Task DeleteAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            collection.Remove((Query.EQ("_id", ObjectId.Parse(user.Id))));
            return Task.FromResult(true);
        }

        public Task<TUser> FindByIdAsync(string id)
        {
            ThrowIfDisposed();
            var bsonId = ObjectId.Parse(id);
            TUser user = collection.FindOneByIdAs<TUser>(bsonId);
            return Task.FromResult(user);
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            ThrowIfDisposed();

            var user = collection.FindOneAs<TUser>((Query.EQ("userName", userName)));
            return Task.FromResult(user);
        }

        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            if (!user.Logins.Any(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey))
            {
                user.Logins.Add(login);
            }

            return Task.FromResult(true);
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            user.Logins.RemoveAll(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);

            return Task.FromResult(0);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            IList<UserLoginInfo> logins = user.Logins.ToList();
            return Task.FromResult(logins);
        }

        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            TUser user = null;
            user = collection.FindOneAs<TUser>(Query.And(Query.EQ("logins.loginProvider", login.LoginProvider),
                        Query.EQ("logins.providerKey", login.ProviderKey)));

            return Task.FromResult(user);
        }

        public Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            IList<Claim> result = user.Claims.Select(c => new Claim(AngularJSAuthentication.Data.Models.Constants.ClaimTypeScope, c)).ToList();
            return Task.FromResult(result);
        }

        public Task AddClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            if (!user.Claims.Any(x => x.Equals(claim.Value, StringComparison.OrdinalIgnoreCase)))
            {
                user.Claims.Add(claim.Value);
            }

            return Task.FromResult(0);
        }

        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            user.Claims.RemoveAll(x => x.Equals(claim.Value, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(0);
        }

        public Task AddToRoleAsync(TUser user, string role)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            if (!user.Roles.Contains(role, StringComparer.InvariantCultureIgnoreCase))
                user.Roles.Add(role);

            return Task.FromResult(true);
        }

        public Task RemoveFromRoleAsync(TUser user, string role)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            user.Roles.RemoveAll(r => String.Equals(r, role, StringComparison.InvariantCultureIgnoreCase));
            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult<IList<string>>(user.Roles);
        }

        public Task<bool> IsInRoleAsync(TUser user, string role)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.Roles.Contains(role, StringComparer.InvariantCultureIgnoreCase));
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetSecurityStampAsync(TUser user, string passwordHash)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.SecurityStamp);
        }

        public void Dispose()
        {
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }
    }
}
