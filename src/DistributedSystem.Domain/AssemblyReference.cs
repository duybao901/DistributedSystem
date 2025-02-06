using System.Reflection;

namespace DistributedSystem.Domain;
public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}