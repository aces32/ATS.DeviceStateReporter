using ATS.DeviceStateReporter.Contracts.Services.Analytics;
using ATS.DeviceStateReporter.Models;
using ATS.DeviceStateReporter.Utilities.Enums;

namespace ATS.DeviceStateReporter.Services.Analytics
{
    public class MetricsCalculator : IMetricsCalculator
    {
        public TimeSpan TotalRunningTime { get; private set; } = TimeSpan.Zero;
        public TimeSpan TotalDowntime { get; private set; } = TimeSpan.Zero;

        public double GetAvailabilityPercentage()
        {
            var total = TotalRunningTime + TotalDowntime;
            return total.TotalSeconds == 0 ? 0 : (TotalRunningTime.TotalSeconds / total.TotalSeconds) * 100;
        }

        public void Process(StatePeriod period)
        {
            var duration = period.Duration;

            if (period.State == MachineState.Running)
                TotalRunningTime += duration;
            else if (period.State == MachineState.Faulted)
                TotalDowntime += duration;
        }

        public void Reset()
        {
            TotalDowntime = TimeSpan.Zero;
            TotalRunningTime = TimeSpan.Zero;
        }
    }
}
