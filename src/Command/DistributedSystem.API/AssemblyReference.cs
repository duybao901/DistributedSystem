using System.Reflection;

namespace DistributedSystem.API;

public class AssemblyReference
{
    public static readonly Assembly assembly = typeof(AssemblyReference).Assembly;
}
