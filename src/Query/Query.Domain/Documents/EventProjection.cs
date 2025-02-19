using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Query.Domain.Abstractions.Documents;
using Query.Domain.Attributes;
using Query.Infrastructure.Consumer.Constants;

namespace Query.Infrastructure.Consumer.Documents;

[BsonCollection(TableNames.Event)]
public class EventProjection : Document
{
    [BsonRepresentation(BsonType.String)]
    public Guid EventId { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
}
