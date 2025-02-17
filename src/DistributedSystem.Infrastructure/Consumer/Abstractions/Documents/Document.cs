using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DistributedSystem.Infrastructure.Consumer.Abstractions;

public abstract class Document : IDocument
{
    public ObjectId Id { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid DocumentId { get; set; } // Id cua SourceMessage: ProductID, CustomerID, OrderID

    public DateTimeOffset CreatedOnUtc => Id.CreationTime;

    public DateTimeOffset? ModifiedOnUtc { get; set; }
}