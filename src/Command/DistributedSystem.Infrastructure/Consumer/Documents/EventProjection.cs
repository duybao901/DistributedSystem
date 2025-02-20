using DistributedSystem.Infrastructure.Consumer.Abstractions;
using DistributedSystem.Infrastructure.Consumer.Attributes;
using DistributedSystem.Infrastructure.Consumer.Constants;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DistributedSystem.Infrastructure.Consumer.Documents;

[BsonCollection(TableNames.Event)]
public class EventProjection : Document
{
    [BsonRepresentation(BsonType.String)]
    public Guid EventId { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
}
