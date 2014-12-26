using MongoDB.Bson.Serialization.Attributes;

namespace AngularJSAuthentication.Data.Infrastructure
{
    public interface IEntity<TKey>
    {
        [BsonId]
        TKey Id { get; set; }
    }

    public interface IEntity : IEntity<string>
    {

    }
}
