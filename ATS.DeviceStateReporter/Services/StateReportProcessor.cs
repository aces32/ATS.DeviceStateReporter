using ATS.DeviceStateReporter.Contracts.Services;
using ATS.DeviceStateReporter.Models;

namespace ATS.DeviceStateReporter.Services
{
    public class StateReportProcessor : IStateReportProcessor
    {
        public List<StatePeriod> GeneratePeriods(List<StateReportEntry> entries)
        {
            var periods = new List<StatePeriod>();
            for (int i = 0; i < entries.Count - 1; i++)
            {
                periods.Add(new StatePeriod
                {
                    AlarmCode = entries[i].AlarmCode,
                    Start = entries[i].Timestamp,
                    End = entries[i + 1].Timestamp,
                    State = entries[i].State,
                });
            }

            // Handling last entry with assumed end = last.Timestamp.AddMinutes(1)
            if (entries.Count > 0)
            {
                var last = entries[^1];
                periods.Add(new StatePeriod
                {
                    State = last.State,
                    Start = last.Timestamp,
                    End = last.Timestamp.AddMinutes(1),
                    AlarmCode = last.AlarmCode
                });
            }
            return periods;
        }
    }
}
