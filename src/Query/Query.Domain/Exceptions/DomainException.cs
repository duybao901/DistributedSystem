namespace Query.Domain.Exceptions;

public abstract class DomainException : Exception
{
    public string Title { get; set; }
    protected DomainException(string title, string message) : base(message)
    {
        Title = title;
    }
}
