using System.Reflection;

namespace DistributedSystem.Infrastructure;
public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}