using ATS.DeviceStateReporter.Utilities.Enums;

namespace ATS.DeviceStateReporter.Models
{
    public class StateReportEntry
    {
        public MachineState State { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public int? AlarmCode { get; set; }
    }
}
