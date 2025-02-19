using System.Reflection;

namespace Query.Infrastructure;
public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}