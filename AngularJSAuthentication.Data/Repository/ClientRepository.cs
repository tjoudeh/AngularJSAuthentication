using System;
using System.Threading.Tasks;
using AngularJSAuthentication.Data.Entities;
using AngularJSAuthentication.Data.Infrastructure;
using AngularJSAuthentication.Data.Interface;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AngularJSAuthentication.Data.Repository
{
    public class ClientRepository 
        : MongoRepository<Client, string>, IClientRepository
    {
        private bool _disposed;

        private readonly MongoRepository<Client> _repository;

        public  ClientRepository(MongoUrl mongoUrl)
        {
            _repository = new MongoRepository<Client>(); 
        }

        public ClientRepository(string connectionString)
            : base(connectionString)
        {
            _repository = new MongoRepository<Client>(connectionString);

            var pack = new ConventionPack();
            pack.Add(new CamelCaseElementNameConvention());
            pack.Add(new IgnoreIfNullConvention(true));

            ConventionRegistry.Register("camel case", pack, t => true);
        }

        public Task<Client> FindClient(string clientId)
        {
            ThrowIfDisposed();
            var result = collection.FindOneAs<Client>((Query.EQ("_id", clientId)));
            return Task.FromResult(result);
        }

        public Task SaveClient(Client client)
        {
            ThrowIfDisposed();
            if (client == null)
                throw new ArgumentNullException("client");

            collection.Insert(client);
            return Task.FromResult(true);
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
