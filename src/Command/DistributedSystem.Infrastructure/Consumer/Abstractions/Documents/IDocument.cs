using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DistributedSystem.Infrastructure.Consumer.Abstractions;

public interface IDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    ObjectId Id { get; set; }

    DateTimeOffset CreatedOnUtc { get; }

    DateTimeOffset? ModifiedOnUtc { get; }
}