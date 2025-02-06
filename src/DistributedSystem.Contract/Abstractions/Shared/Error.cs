namespace DistributedSystem.Contract.Abtractions.Shared;

public class Error : IEquatable<Error>
{
    public string Message { get; }
    public string Code { get; }
    public Error(string message, string code)
    {
        Message = message;
        Code = code;
    }

    public static readonly Error None = new Error(string.Empty, string.Empty);
    public static readonly Error NullValue = new Error("Error.NullValue", "The specified value is null.");

    // chuyển một đối tượng của kiểu Error thành một kiểu string
    public static implicit operator string(Error error) => error.Code;

    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    // (a ==b ) will call == operator above
    public static bool operator !=(Error? a, Error? b) => !(a == b);

    public bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Code == other.Code && Message == other.Message;
    }

    public override bool Equals(object? obj) => obj is Error error && Equals(error);

    public override int GetHashCode() => HashCode.Combine(Code, Message);

    public override string ToString() => Code;
}