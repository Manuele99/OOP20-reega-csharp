using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Reega.Shared.Models;

namespace Reega.Pola
{
    public class JsonExporter : IReegaExporter
    {
        private readonly List<Data> _data;

        public JsonExporter(List<Data> data) =>
            _data = data;

        public void Export(string file)
        {
            StreamWriter writer = File.CreateText(file);

            List<ContractEntry> metrics = _data
                .GroupBy(v => v.ContractID)
                .Select(contract => new ContractEntry(
                    contract.Key,
                    contract.Select(value => new DataEntry(
                        Enum.GetName(value.Type),
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
            [JsonProperty("values")] private IDictionary<long, double> _values;

            public DataEntry(string type, IDictionary<long, double> values)
            {
                _type = type;
                _values = values;
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
