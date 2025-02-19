using DistributedSystem.Domain.Abstractions.Aggregates;
using DistributedSystem.Domain.Abstractions.Entities;

namespace DistributedSystem.Domain.Entities;

public class Product : AggregateRoot<Guid>, IAuditableEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public Product(Guid id, string name, decimal price, string description)
    {
        Id = id;
        Name = name; 
        Price = price; 
        Description = description;
    }

    public static Product CreateProduct(Guid id, string name, decimal price, string description)
    {
        var product = new Product(id, name, price, description);

        product.RaiseDomainEvent(new Contract.Services.V1.Product.DomainEvent.ProductCreated(Guid.NewGuid(), 
            product.Id, 
            product.Name,
            product.Price, 
            product.Description));

        return product;
    }

    public void Update(string name, decimal price, string description)
    {
        Name = name;
        Price = price;
        Description = description;

        // this -> tham chiếu đến đối tượng hiện tại (Product.Update)
        this.RaiseDomainEvent(new Contract.Services.V1.Product.DomainEvent.ProductUpdated(Guid.NewGuid(),
            Id,
            name,
            price,
            description));
    }
    
    public void Delete()
    {
        // this -> tham chiếu đến đối tượng hiện tại (Product.Delete)
        this.RaiseDomainEvent(new Contract.Services.V1.Product.DomainEvent.ProductDeleted(Guid.NewGuid(), Id));
    }
}
