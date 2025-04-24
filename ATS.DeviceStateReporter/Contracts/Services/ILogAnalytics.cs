using ATS.DeviceStateReporter.Models;

namespace ATS.DeviceStateReporter.Contracts.Services
{
    public interface ILogAnalytics
    {
        void Analyze(List<StatePeriod> periods);
        TimeSpan GetTotalRunningTime();
        TimeSpan GetTotalDowntime();
        double GetAvailabilityPercentage();
        List<(int AlarmCode, TimeSpan Duration)> GetTopAlarms(int topN);
    }
}
