using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reega.Salomone.DI
{
    public class ServiceProvider
    {
        private readonly ServiceCollection _svcCollection;


        internal ServiceProvider(ServiceCollection svcCollection)
        {
            this._svcCollection = svcCollection;
        }

        /// <summary>
        /// Get a service or null if no service has been found.
        /// </summary>
        /// <typeparam name="T">Type of the service</typeparam>
        /// <returns>The service if the service exists, null otherwise</returns>
#nullable enable
        public T? GetService<T>()
        {
            return this._svcCollection.GetService<T>();
        }

        /// <summary>
        /// Get a service.
        /// </summary>
        /// <typeparam name="T">Type of the service</typeparam>
        /// <returns>An instance of the service</returns>
#nullable disable
        public T GetRequiredService<T>()
        {
            return this._svcCollection.GetService<T>() ??
                   throw new ArgumentException($"No service for type: {typeof(T)}");
        }
    }
}
