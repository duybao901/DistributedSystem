using System.Reflection;

namespace AuthorizationApi;

public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
