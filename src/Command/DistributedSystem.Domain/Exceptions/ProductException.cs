namespace DistributedSystem.Domain.Exceptions;

public static class ProductException
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid productId) : base($"Product with id {productId} was not found.")
        {
        }
    }
}
