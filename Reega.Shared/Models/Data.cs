using System.Collections.Generic;

namespace Reega.Shared.Models
{
    class Data
    {
        private readonly int contractID;
        private readonly DataType type;
        private readonly IDictionary<long, double> dataValuesByTimestamp;

        public Data(int contractID, DataType dataType) : this(contractID,dataType, new Dictionary<long, double> )
        {
            this.contractID = contractID;
            this.type = dataType;
        }

        public Data(int contractID, DataType dataType, IDictionary<long, double> data)
        {
            this.contractID = contractID;
            this.type = dataType;
            this.dataValuesByTimestamp = data;
        }

        public void addRecord(long timestamp, double value) 
        {
            this.dataValuesByTimestamp.Add(timestamp, value);
        }

        public void AddRecords(IDictionary<long, double> values)
        {
            foreach (KeyValuePair<long, double> pair in values)
                this.dataValuesByTimestamp.Add(pair.Key, pair.Value);
        }

        public DataType Type => type;

        public int ContractID => contractID;

        public IDictionary<long, double> DataValuesByTimestamp => dataValuesByTimestamp;
    }
}
