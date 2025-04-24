using ATS.DeviceStateReporter.Models;

namespace ATS.DeviceStateReporter.Contracts.Services
{
    public interface ILogAnalytics
    {
        LogAnalyticsReport Analyze(List<StatePeriod> periods, int topNAlarms = 5);
    }
}
