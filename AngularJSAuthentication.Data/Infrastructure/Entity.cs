using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AngularJSAuthentication.Data.Infrastructure
{
    [Serializable]
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Entity : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public virtual string Id { get; set; }
    }
}
