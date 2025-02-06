using System.Reflection;

namespace DistributedSystem.API;

public static class AssemblyReference
{
    public static readonly Assembly assembly = typeof(AssemblyReference).Assembly;
}
