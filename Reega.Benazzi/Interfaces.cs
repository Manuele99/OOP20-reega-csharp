using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reega.Shared.Models;

namespace Reega.Benazzi
{
    public interface IGenerator
    {
        /// <summary>
        /// Generates a random value.
        /// </summary>
        /// <returns> a new generated value. </returns>
        double NextValue();
    }

    public interface IUsageSimulator
    {
        /// <summary>
        /// Generates the usage for water, electric energy and gas, excluding the ones NOT specified at construction. returns
        /// an empty map if no services are specified.
        /// </summary>
        /// <returns> map of utilization values, key is the type of service. </returns>
        Dictionary<DataType, double> ServicesUsage();

        /// <summary>
        /// Generates the values for paper, plastic, glass and mixed wastes, excluding the ones NOT specified at
        /// construction.returns an empty map if no services are specified.
        /// </summary>
        /// <returns> map of utilization values, key is the type of service. </returns>
        Dictionary<DataType, double> GetWastesUsage();

        /// <summary>
        /// Generates the values for selected services excluding the ones NOT specified at construction. returns an empty map
        /// if no services are specified or selected.
        /// </summary>
        /// <param name="services"> list of DataType used for generating the data </param>
        /// <returns> map of utilization values, key is the type of service. </returns>
        Dictionary<DataType, double> GetSelectedUsage(List<DataType> services);

        /// <summary>
        /// Generates the usage for the specified service; if the service does not belong to the ones specifies ate
        /// construction the method returns an Optional null.
        /// </summary>
        /// <param name="service"> type of service of which the usage will be generated. </param>
        /// <returns> A nullable containing the value</returns>
        Nullable<Double> GetUsage(DataType service);
    }

    public interface IDataFiller
    {
        /// <summary>
        /// Fills the given database with utilization data for every given contract starting from the latest time stamp in
        /// the database to the current time stamp for every service of every contract, following these cadences: - for
        /// water, electric energy and gas: new data every hour. - for plastic, glass, paper and mixed wastes: new data every
        /// day.
        /// </summary>
        void Fill();
    }
}
