using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reega.Shared.Models;

namespace Reega.Benazzi
{
    internal class SelectiveUsageSimulator : IUsageSimulator
    {
        private readonly Dictionary<DataType, IGenerator> _generators;

        public SelectiveUsageSimulator(List<DataType> services)
        {
            this._generators = new();
            foreach (DataType service in services)
            {
                this._generators.Add(service, GaussianGeneratorFactory.GetGaussianGenerator(service));
            }
        }

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

        public double? GetUsage(DataType service)
        {
            if (this._generators.ContainsKey(service))
            {
                return this._generators[service].NextValue();
            }
            return null;
        }

        public Dictionary<DataType, double> GetWastesUsage()
        {
            return this.GetSelectedUsage(new List<DataType>() { DataType.PAPER, DataType.PLASTIC, DataType.GLASS, DataType.MIXED });
        }

        public Dictionary<DataType, double> ServicesUsage()
        {
            return this.GetSelectedUsage(new List<DataType>() { DataType.ELECTRICITY, DataType.GAS, DataType.WATER });
        }
    }
}
