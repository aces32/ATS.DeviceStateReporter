using ATS.DeviceStateReporter.Utilities.Enums;

namespace ATS.DeviceStateReporter.Models
{
    public class StatePeriod
    {
        public MachineState State { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public int? AlarmCode { get; set; }

        public TimeSpan Duration => End - Start;
    }
}
