using ATS.DeviceStateReporter.Contracts.Infrastructure;
using ATS.DeviceStateReporter.Models;
using ATS.DeviceStateReporter.Utilities.CsvParsers;
using ATS.DeviceStateReporter.Utilities.Enums;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace ATS.DeviceStateReporter.Infrastructure
{
    public class StateCsvParser : IStateCsvParser
    {
        public List<StateReportEntry> Parse(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
            });
            csv.Context.TypeConverterCache.AddConverter<MachineState>(new MachineStateConverter());
            csv.Context.RegisterClassMap<StateReportEntryMap>();
            return [.. csv.GetRecords<StateReportEntry>()];
        }
    }

}
