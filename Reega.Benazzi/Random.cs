using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reega.Benazzi
{
    internal class RandomWithGaussian : System.Random
    {
        private double _nextNextGaussian;
        private bool _hasNextGaussian = false;

        /// <summary>
        /// c# implementation of the <see href="https://hg.openjdk.java.net/jdk8/jdk8/jdk/file/tip/src/share/classes/java/util/Random.java">OpenJDK</see> method "NextGaussian" wich uses 
        /// the polar form of the <see href="https://en.wikipedia.org/wiki/Box%E2%80%93Muller_transform"> Box-Muller transform </see>
        /// </summary>
        /// <returns> a pseudorandom Gaussian distributed value with mean 0 and standard deviation 1 </returns>
        public double NextGaussian()
        {
            if (this._hasNextGaussian) 
            {
                this._hasNextGaussian = false;
                return this._nextNextGaussian;
            }
            else
            {
                double n1, n2, p;
                do
                {
                    n1 = (2 * base.NextDouble()) - 1;
                    n2 = (2 * base.NextDouble()) - 1;
                    p = n1 * n1 + n2 * n2;
                } while (p >= 1 || p == 0);
                double common = Math.Sqrt((-2 * Math.Log(p)) / p);
                this._nextNextGaussian = common * n1;
                this._hasNextGaussian = true;
                return common * n2;
            }
        }
    }
}
