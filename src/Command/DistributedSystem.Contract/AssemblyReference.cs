using System.Reflection;

namespace DistributedSystem.Contract;
public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}