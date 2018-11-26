namespace SaatchiDataCapture.FunctionApp
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using StructureMap.Graph;

    /// <summary>
    /// Custom, host-specific implementation of
    /// <see cref="StructureMap.Registry" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Registry : StructureMap.Registry
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Registry" /> class.
        /// </summary>
        public Registry()
        {
            this.Scan(DoScan);
        }

        private static void DoScan(IAssemblyScanner assemblyScanner)
        {
            // Always create concrete instances based on usual DI naming
            // convention
            // i.e. Search for class name "Concrete" when "IConcrete" is
            //      requested.
            assemblyScanner.WithDefaultConventions();

            string assemblyLocation = Assembly.GetExecutingAssembly().Location;

            FileInfo fileInfo = new FileInfo(assemblyLocation);

            string path = fileInfo.Directory.FullName;

            // Scan all assemblies, including the one executing.
            assemblyScanner.AssembliesFromPath(path);
        }
    }
}