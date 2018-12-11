namespace MarketingDataCapture.FunctionApp.Infrastructure
{
    using System.Diagnostics.CodeAnalysis;
    using StructureMap.Graph;

    /// <summary>
    /// Custom, host-specific implementation of
    /// <see cref="StructureMap.Registry" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Registry : StructureMap.Registry
    {
        private string executingAssemblyLocation;

        /// <summary>
        /// Initialises a new instance of the <see cref="Registry" /> class.
        /// </summary>
        /// <param name="executingAssemblyLocation">
        /// The location of the executing assembly (i.e. the bin directory).
        /// </param>
        public Registry(string executingAssemblyLocation)
        {
            this.executingAssemblyLocation = executingAssemblyLocation;

            this.Scan(this.DoScan);
        }

        private void DoScan(IAssemblyScanner assemblyScanner)
        {
            // Always create concrete instances based on usual DI naming
            // convention
            // i.e. Search for class name "Concrete" when "IConcrete" is
            //      requested.
            assemblyScanner.WithDefaultConventions();

            // Scan all assemblies, including the one executing.
            assemblyScanner.AssembliesFromPath(this.executingAssemblyLocation);
        }
    }
}