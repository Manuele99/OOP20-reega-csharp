
using System;
using System.Collections.Generic;
using Reega.Shared.Models;

namespace Reega.Pola
{
    public static class ReegaExporterFactory
    {
        static void Export(ExportFormat format, string file, List<Data> data)
        {
            IReegaExporter exporter = format switch
            {
                ExportFormat.JSON => new JsonExporter(data),
                ExportFormat.CSV => new CsvExporter(data),
                _ => throw new ArgumentException(null, nameof(format))
            };
            exporter.Export(file);
        }
    }
}
