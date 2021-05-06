using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reega.Shared.Models;

namespace Reega.Benazzi
{
    /// <summary>
    /// Static factory to obtain GaussianGenerators based on the needed DataType.
    /// Mean and variance of each GaussianGenerator are based on real usage values of the italian population. 
    /// </summary>
    internal static class GaussianGeneratorFactory
    {
        public static readonly Dictionary<DataType, Tuple<double, double>> _rangesMap = new() {
            { DataType.ELECTRICITY, new Tuple<double, double>(0.3, 0.23)},
            { DataType.GAS , new Tuple<double, double>(0.17, 0.2) },
            { DataType.WATER, new Tuple<double, double>(18.0, 4.0) },
            { DataType.PAPER, new Tuple<double, double>(0.2, 0.1) },
            { DataType.GLASS, new Tuple<double, double>(0.2, 0.09) },
            { DataType.PLASTIC, new Tuple<double, double>(0.35, 0.1) },
            { DataType.MIXED, new Tuple<double, double>(0.5, 0.2) }
        };
        private static readonly RandomWithGaussian _random = new();

        public static GaussianGenerator GetGaussianGenerator(DataType dataType)
        {
            if (_rangesMap.ContainsKey(dataType))
            {
                return new PosGaussianGenerator(_random.NextDouble() * 0.2 + _rangesMap[dataType].Item1,
                    _random.NextDouble() * 0.2 + _rangesMap[dataType].Item2);
            } else
            {
                return new GaussianGenerator(0.0, 0.0);
            }
        }        
    }

    /// <summary>
    /// Internal wrapper to only generate positive values.
    /// </summary>
    internal class PosGaussianGenerator : GaussianGenerator
    {
        public PosGaussianGenerator(double mean, double variance) : base(mean, variance) { }

        public override double NextValue()
        {
            return Math.Abs(base.NextValue());
        }
    }
}
