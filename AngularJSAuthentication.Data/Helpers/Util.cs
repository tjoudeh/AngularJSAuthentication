using System;
using System.Configuration;
using AngularJSAuthentication.Data.Attributes;
using AngularJSAuthentication.Data.Infrastructure;
using MongoDB.Driver;

namespace AngularJSAuthentication.Data.Helpers
{
    internal static class Util<U>
    {
        /// <summary>
        /// The default key MongoRepository will look for in the App.config or Web.config file.
        ///  - Changed Default Value 
        /// </summary>
        private const string DefaultConnectionstringName = "MongoServerSettings";

        /// <summary>
        /// Retrieves the default connectionstring from the App.config or Web.config file.
        /// </summary>
        /// <returns>Returns the default connectionstring from the App.config or Web.config file.</returns>
        public static string GetDefaultConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[DefaultConnectionstringName].ConnectionString;
        }

        /// <summary>
        /// Creates and returns a MongoDatabase from the specified url.
        /// </summary>
        /// <param name="url">The url to use to get the database from.</param>
        /// <returns>Returns a MongoDatabase from the specified url.</returns>
        private static MongoDatabase GetDatabaseFromUrl(MongoUrl url)
        {
            var client = new MongoClient(url);
            var server = client.GetServer();
            return server.GetDatabase(url.DatabaseName); // WriteConcern defaulted to Acknowledged
        }

        /// <summary>
        /// Creates and returns a MongoCollection from the specified type and connectionstring.
        /// </summary>
        /// <typeparam name="T">The type to get the collection of.</typeparam>
        /// <param name="connectionString">The connectionstring to use to get the collection from.</param>
        /// <returns>Returns a MongoCollection from the specified type and connectionstring.</returns>
        public static MongoCollection<T> GetCollectionFromConnectionString<T>(string connectionString)
            where T : IEntity<U>
        {
            return Util<U>.GetCollectionFromConnectionString<T>(connectionString, GetCollectionName<T>());
        }

        /// <summary>
        /// Creates and returns a MongoCollection from the specified type and connectionstring.
        /// </summary>
        /// <typeparam name="T">The type to get the collection of.</typeparam>
        /// <param name="connectionString">The connectionstring to use to get the collection from.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        /// <returns>Returns a MongoCollection from the specified type and connectionstring.</returns>
        public static MongoCollection<T> GetCollectionFromConnectionString<T>(string connectionString, string collectionName)
            where T : IEntity<U>
        {
            return Util<U>.GetDatabaseFromUrl(new MongoUrl(connectionString))
                .GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Creates and returns a MongoCollection from the specified type and url.
        /// </summary>
        /// <typeparam name="T">The type to get the collection of.</typeparam>
        /// <param name="url">The url to use to get the collection from.</param>
        /// <returns>Returns a MongoCollection from the specified type and url.</returns>
        public static MongoCollection<T> GetCollectionFromUrl<T>(MongoUrl url)
            where T : IEntity<U>
        {
            return Util<U>.GetCollectionFromUrl<T>(url, GetCollectionName<T>());
        }

        /// <summary>
        /// Creates and returns a MongoCollection from the specified type and url.
        /// </summary>
        /// <typeparam name="T">The type to get the collection of.</typeparam>
        /// <param name="url">The url to use to get the collection from.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        /// <returns>Returns a MongoCollection from the specified type and url.</returns>
        public static MongoCollection<T> GetCollectionFromUrl<T>(MongoUrl url, string collectionName)
            where T : IEntity<U>
        {
            return Util<U>.GetDatabaseFromUrl(url)
                .GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Determines the collectionname for T and assures it is not empty
        /// </summary>
        /// <typeparam name="T">The type to determine the collectionname for.</typeparam>
        /// <returns>Returns the collectionname for T.</returns>
        private static string GetCollectionName<T>() where T : IEntity<U>
        {
            string collectionName;
            if (typeof(T).BaseType.Equals(typeof(object)))
            {
                collectionName = GetCollectioNameFromInterface<T>();
            }
            else
            {
                collectionName = GetCollectionNameFromType(typeof(T));
            }

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("Collection name cannot be empty for this entity");
            }
            return collectionName;
        }

        /// <summary>
        /// Determines the collectionname from the specified type.
        /// </summary>
        /// <typeparam name="T">The type to get the collectionname from.</typeparam>
        /// <returns>Returns the collectionname from the specified type.</returns>
        private static string GetCollectioNameFromInterface<T>()
        {
            string collectionname;

            // Check to see if the object (inherited from Entity) has a CollectionName attribute
            var att = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionName));
            if (att != null)
            {
                // It does! Return the value specified by the CollectionName attribute
                collectionname = ((CollectionName)att).Name;
            }
            else
            {
                collectionname = typeof(T).Name;
            }

            return collectionname;
        }

        /// <summary>
        /// Determines the collectionname from the specified type.
        /// </summary>
        /// <param name="entitytype">The type of the entity to get the collectionname from.</param>
        /// <returns>Returns the collectionname from the specified type.</returns>
        private static string GetCollectionNameFromType(Type entitytype)
        {
            string collectionname;

            // Check to see if the object (inherited from Entity) has a CollectionName attribute
            var att = Attribute.GetCustomAttribute(entitytype, typeof(CollectionName));
            if (att != null)
            {
                // It does! Return the value specified by the CollectionName attribute
                collectionname = ((CollectionName)att).Name;
            }
            else
            {
                // No attribute found, get the basetype
                while (!entitytype.BaseType.Equals(typeof(Entity)))
                {
                    entitytype = entitytype.BaseType;
                }

                collectionname = entitytype.Name;
            }

            return collectionname;
        }
    }
}
