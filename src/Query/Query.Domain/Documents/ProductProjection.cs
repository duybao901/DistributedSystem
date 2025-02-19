using Query.Domain.Abstractions.Documents;
using Query.Domain.Attributes;
using Query.Infrastructure.Consumer.Constants;

namespace Query.Domain.Documents;

[BsonCollection(TableNames.Product)]
public class ProductProjection : Document
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
}
