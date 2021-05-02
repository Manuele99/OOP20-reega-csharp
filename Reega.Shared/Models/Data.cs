using System.Collections.Generic;

namespace Reega.Shared.Models
{
    public class Data
    {
        public Data(int contractId, DataType dataType) : this(contractId, dataType, new Dictionary<long, double>())
        {
            ContractId = contractId;
            Type = dataType;
        }

        public Data(int contractId, DataType dataType, IDictionary<long, double> data)
        {
            ContractId = contractId;
            Type = dataType;
            DataValuesByTimestamp = data;
        }

        public int ContractId { get; }

        public DataType Type { get; }

        public IDictionary<long, double> DataValuesByTimestamp { get; }

        public void AddRecord(long timestamp, double value)
        {
            DataValuesByTimestamp.Add(timestamp, value);
        }

        public void AddRecords(IDictionary<long, double> values)
        {
            foreach (var pair in values)
                DataValuesByTimestamp.Add(pair.Key, pair.Value);
        }
    }
}
