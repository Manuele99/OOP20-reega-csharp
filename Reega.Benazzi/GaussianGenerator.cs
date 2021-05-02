using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reega.Benazzi
{
    internal class GaussianGenerator : IGenerator
    {
        private readonly double _mean;
        private readonly double _variance;
        private readonly RandomWithGaussian _random;

        public GaussianGenerator(double mean, double variance)
        {
            this._mean = mean;
            this._variance = variance;
            this._random = new RandomWithGaussian();

        }

        public virtual double NextValue()
        {
            return this._mean + this._variance * this._random.NextGaussian();
        }
    }

}
