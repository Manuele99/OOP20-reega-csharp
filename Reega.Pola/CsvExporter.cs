using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Reega.Shared.Models;
using System.IO;
using Reega.Shared.Extensions;

namespace Reega.Pola
{
    public class CsvExporter : IReegaExporter
    {
        private readonly IList<Data> _data;

        public CsvExporter(IList<Data> data) =>
            _data = data;

        ///<inheritdoc/>
        public void Export(string file)
        {
            StreamWriter writer = File.CreateText(file);
            // header
            writer.WriteLine("timestamp,contract_id,type,value");

            _data.GroupBy(v => v.ContractId)
                .OrderBy(v => v.Key)
                .ForEach(contract =>
                {
                    contract.ForEach(value =>
                    {
                        value.DataValuesByTimestamp
                            .OrderBy(t => t.Key)
                            .Select(record => CsvRow(
                                Utils.UnixTimeToDateTime(record.Key).ToString("yyyy-MM-dd HH:mm:ss"),
                                contract.Key,
                                value.Type.Name,
                                record.Value
                            )).ForEach(row => writer.WriteLine(row));
                    });
                });
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// build csv row given n values
        /// </summary>
        /// <param name="elements">values to join into csv row</param>
        /// <returns>string containing the csv row</returns>
        private static string CsvRow(params object[] elements)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            return string.Join(",", elements.Select(v => v.ToString()));
        }
    }
}
