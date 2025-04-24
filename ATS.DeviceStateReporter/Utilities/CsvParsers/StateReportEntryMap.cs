using ATS.DeviceStateReporter.Models;
using CsvHelper.Configuration;

namespace ATS.DeviceStateReporter.Utilities.CsvParsers
{
    public class StateReportEntryMap : ClassMap<StateReportEntry>
    {
        public StateReportEntryMap()
        {
            Map(m => m.State).Name("state");
            Map(m => m.Timestamp).Name("timestamp");
            Map(m => m.AlarmCode).Name("alarmcode");
        }
    }
}
