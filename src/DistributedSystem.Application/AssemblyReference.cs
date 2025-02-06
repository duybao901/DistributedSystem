using System.Reflection;

namespace DistributedSystem.Application;
public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}