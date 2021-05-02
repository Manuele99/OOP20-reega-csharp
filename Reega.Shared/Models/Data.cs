using System.Collections.Generic;
using System.Linq;

namespace Reega.Shared.Models
{
    public class Data
    {
        public int ContractId { get; }

        public DataType Type { get; }

        public IDictionary<long, double> DataValuesByTimestamp { get; }

        public Data(int contractId, DataType dataType) : this(contractId, dataType, new Dictionary<long, double>())
        {
            this.ContractId = contractId;
            this.Type = dataType;
        }

        public Data(int contractId, DataType dataType, IDictionary<long, double> data)
        {
            this.ContractId = contractId;
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
