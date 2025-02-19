using System.Reflection;

namespace DistributedSystem.Persistence;
public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}