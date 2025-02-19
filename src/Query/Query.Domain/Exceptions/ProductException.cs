namespace Query.Domain.Exceptions;

public static class ProductException
{
    public abstract class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid productId) : base($"Product with id {productId} was not found.")
        {
        }
    }
}
