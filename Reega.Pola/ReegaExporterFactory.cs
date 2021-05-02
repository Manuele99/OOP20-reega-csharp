using System;
using System.Collections.Generic;
using Reega.Shared.Models;

namespace Reega.Pola
{
    public static class ReegaExporterFactory
    {
        static void Export(ExportFormat format, string file, List<Data> data)
        {
            List<Data> dataToBeExported = data ?? new List<Data>();
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