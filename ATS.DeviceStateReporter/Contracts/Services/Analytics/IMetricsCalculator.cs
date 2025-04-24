using ATS.DeviceStateReporter.Models;

namespace ATS.DeviceStateReporter.Contracts.Services.Analytics
{
    public interface IMetricsCalculator
    {
        void Process(StatePeriod period);
        TimeSpan TotalRunningTime { get; }
        TimeSpan TotalDowntime { get; }
        double GetAvailabilityPercentage();
        void Reset();
    }
}
