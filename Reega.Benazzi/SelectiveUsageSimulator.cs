using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reega.Shared.Models;

namespace Reega.Benazzi
{
    /// <summary>
    /// Container for IGenerators initialised depending on the specified DataType
    /// </summary>
    internal class SelectiveUsageSimulator : IUsageSimulator
    {
        private readonly Dictionary<DataType, IGenerator> _generators;

        /// <summary>
        /// Initializes Based on the gives services list
        /// </summary>
        /// <param name="services"> The services this UsageSimulator will generate data for </param>
        public SelectiveUsageSimulator(List<DataType> services)
        {
            this._generators = new();
            foreach (DataType service in services.ToHashSet())
            {
                this._generators.Add(service, GaussianGeneratorFactory.GetGaussianGenerator(service));
            }
        }

        /// <inheritdoc/>
        public Dictionary<DataType, double> GetSelectedUsage(List<DataType> services)
        {
            Dictionary<DataType, double> dict = new();
            foreach (DataType service in services)
            {
                if (this._generators.ContainsKey(service))
                {
                    dict.Add(service, this._generators[service].NextValue());
                }
            }
            return dict;
        }

        /// <inheritdoc/>
        public double? GetUsage(DataType service)
        {
            if (this._generators.ContainsKey(service))
            {
                return this._generators[service].NextValue();
            }
            return null;
        }

        /// <inheritdoc/>
        public Dictionary<DataType, double> GetWastesUsage()
        {
            return this.GetSelectedUsage(new List<DataType>() { DataType.PAPER, DataType.PLASTIC, DataType.GLASS, DataType.MIXED });
        }

        /// <inheritdoc/>
        public Dictionary<DataType, double> ServicesUsage()
        {
            return this.GetSelectedUsage(new List<DataType>() { DataType.ELECTRICITY, DataType.GAS, DataType.WATER });
        }
    }
}
