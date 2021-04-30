using System.Collections.Generic;
using System.Linq;

namespace Reega.Shared.Models
{
    class Data
    {
        public int ContractID { get; }

        public DataType Type { get; }

        public IDictionary<long, double> DataValuesByTimestamp { get; }

        public Data(int contractID, DataType dataType) : this(contractID,dataType, new Dictionary<long, double>() )
        {
            this.ContractID = contractID;
            this.Type = dataType;
        }

        public Data(int contractID, DataType dataType, IDictionary<long, double> data)
        {
            this.ContractID = contractID;
            this.Type = dataType;
            this.DataValuesByTimestamp = data;
        }

        public void AddRecord(long timestamp, double value) => 
            this.DataValuesByTimestamp.Add(timestamp, value);

        public void AddRecords(IDictionary<long, double> values)
        {
            foreach (KeyValuePair<long, double> pair in values)
                this.DataValuesByTimestamp.Add(pair.Key, pair.Value);
        }

    }
}
