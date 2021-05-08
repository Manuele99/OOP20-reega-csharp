using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Reega.Shared.Models;

namespace Reega.Pola
{
    public class JsonExporter : IReegaExporter
    {
        private readonly IList<Data> _data;

        public JsonExporter(IList<Data> data) =>
            _data = data;

        ///<inheritdoc/>
        public void Export(string file)
        {
            StreamWriter writer = File.CreateText(file);

            List<ContractEntry> metrics = _data
                .GroupBy(v => v.ContractId)
                .Select(contract => new ContractEntry(
                    contract.Key,
                    contract.Select(value => new DataEntry(
                        value.Type.Name,
                        value.DataValuesByTimestamp)
                    ).ToList()
                )).ToList();

            writer.Write(JsonConvert.SerializeObject(metrics, Formatting.Indented));

            writer.Flush();
            writer.Close();
        }

        private class DataEntry
        {
            [JsonProperty("type")] private string _type;
            [JsonProperty("values")] private IDictionary<string, double> _values;

            public DataEntry(string type, IDictionary<long, double> values)
            {
                _type = type;
                _values = values.ToDictionary(
                    k => Utils.UnixTimeToDateTime(k.Key).ToString("yyyy-MM-dd HH:mm:ss"),
                    v => v.Value);
            }
        }

        private class ContractEntry
        {
            [JsonProperty("contract_id")] private int _contractId;
            [JsonProperty("values")] private List<DataEntry> _values;

            public ContractEntry(int contractId, List<DataEntry> values)
            {
                _contractId = contractId;
                _values = values;
            }
        }
    }
}
