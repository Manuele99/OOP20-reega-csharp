using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reega.Shared.Models;

namespace Reega.Shared.Data
{
    public interface IDataController
    {
        /// <summary>
        /// Push data into the database (implementation specific).
        /// </summary>
        /// <param name="data"> data that needs to be put in the database </param>
        void PutUserData(Reega.Shared.Models.Data data);

        /// <summary>
        /// Get the latest timestamp for the specific contract and metric present in the database.
        /// </summary>
        /// <param name="contractID"> ID of the researched contract </param>
        /// <param name="service"> data type requested </param>
        /// <returns> the latest timestamp (in milliseconds) for the specific contract and DataType </returns>
        long GetLatestData(int contractID, DataType service);

        /// <summary>
        /// Get the data from the first day of the month until today for a given contractID.
        /// </summary>
        /// <param name="contractID"> contractID that needs to get the monthly data </param>
        /// <returns> of data containing data from the first day of the month until today </returns>
        List<Reega.Shared.Models.Data> GetMonthlyData(int contractID);
    }
}
