using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Reega.Pola;
using Reega.Shared.Models;
using Xunit;

namespace Reega.Tests.Pola
{
    public class ExporterTest
    {
        private static readonly IDictionary<long, double> SampleValues = new Dictionary<long, double>()
        {
            {1614942000000, 5.5},
            {1614942002000, 6.4},
            {1614942004000, 7.3}
        };

        private static readonly IList<Data> SampleData = new List<Data>
        {
            new(1, DataType.ELECTRICITY, SampleValues),
            new(1, DataType.GAS, SampleValues),
            new(1, DataType.GLASS, SampleValues),
            new(1, DataType.MIXED, SampleValues),
            new(1, DataType.PAPER, SampleValues),
            new(1, DataType.PLASTIC, SampleValues),
            new(1, DataType.WATER, SampleValues)
        };

        [Fact]
        public void ExportEmptyData()
        {
            string emptyCsvFile = Path.GetTempFileName();
            ReegaExporterFactory.Export(ExportFormat.CSV, emptyCsvFile, null);
            using StreamReader csvReader = File.OpenText(emptyCsvFile);
            Assert.Equal("timestamp,contract_id,type,value" + Environment.NewLine, csvReader.ReadToEnd());


            string emptyJsonFile = Path.GetTempFileName();
            ReegaExporterFactory.Export(ExportFormat.JSON, emptyJsonFile, null);
            using StreamReader jsonReader = File.OpenText(emptyJsonFile);
            Assert.Equal("[]", jsonReader.ReadToEnd());
        }

        [Fact]
        public void JsonExport()
        {
            string jsonFile = Path.GetTempFileName();
            ReegaExporterFactory.Export(ExportFormat.JSON, jsonFile, SampleData);

            using StreamReader jsonReader = File.OpenText(jsonFile);
            JArray sample = JArray.Parse(GetSampleFile("jsonSample.json"));
            sample.Should().BeEquivalentTo(JArray.Parse(jsonReader.ReadToEnd()));
        }

        [Fact]
        public void CsvExport()
        {
            string csvFile = Path.GetTempFileName();
            ReegaExporterFactory.Export(ExportFormat.CSV, csvFile, SampleData);

            using StreamReader csvReader = File.OpenText(csvFile);
            string sample = GetSampleFile("csvSample.csv");
            Assert.Equal(sample, csvReader.ReadToEnd().Replace("\r\n","\n"));
        }

        private static string GetSampleFile(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using Stream stream = assembly.GetManifestResourceStream("Reega.Tests.Pola." + name);
            using StreamReader reader = new(stream!);
            return reader.ReadToEnd();
        }
    }
}
