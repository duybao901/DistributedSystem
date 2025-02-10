using System.Reflection;

namespace DistributedSystem.Persistance;
public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}