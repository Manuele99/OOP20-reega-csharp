using System;
using System.Collections.Generic;
using Reega.Shared.Models;

namespace Reega.Pola
{
    public static class ReegaExporterFactory
    {
        /// <summary>
        /// Export given data to the specified file
        /// </summary>
        /// <param name="format">format of the exported data</param>
        /// <param name="file">absolute file path</param>
        /// <param name="data">raw data</param>
        /// <exception cref="ArgumentException"></exception>
        public static void Export(ExportFormat format, string file, IList<Data> data)
        {
            IList<Data> dataToBeExported = data ?? new List<Data>();
            IReegaExporter exporter = format switch
            {
                ExportFormat.JSON => new JsonExporter(dataToBeExported),
                ExportFormat.CSV => new CsvExporter(dataToBeExported),
                _ => throw new ArgumentException(null, nameof(format))
            };
            exporter.Export(file);
        }
    }
}
