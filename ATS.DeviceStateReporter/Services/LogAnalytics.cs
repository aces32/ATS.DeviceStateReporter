using ATS.DeviceStateReporter.Contracts.Services;
using ATS.DeviceStateReporter.Contracts.Services.Analytics;
using ATS.DeviceStateReporter.Models;
using ATS.DeviceStateReporter.Services.Analytics;
using ATS.DeviceStateReporter.Utilities.Enums;

namespace ATS.DeviceStateReporter.Services
{
    public class LogAnalytics : ILogAnalytics
    {
        private readonly IAlarmTracker _alarmTracker;
        private readonly IMetricsCalculator _metricsCalculator;

        public LogAnalytics(IAlarmTracker alarmTracker,
            IMetricsCalculator metricsCalculator)
        {
            _alarmTracker = alarmTracker;
            _metricsCalculator = metricsCalculator;
        }

        public LogAnalyticsReport Analyze(List<StatePeriod> periods, int topNAlarms = 5)
        {
            _alarmTracker.Clear();
            _metricsCalculator.Reset();
            foreach (var period in periods)
            {
                _metricsCalculator.Process(period);
                if (period.State == MachineState.Faulted)
                    _alarmTracker.Track(period.AlarmCode, period.Duration);

            }

            return new LogAnalyticsReport
            {
                TotalRunningTime = _metricsCalculator.TotalRunningTime,
                TotalDowntime = _metricsCalculator.TotalDowntime,
                AvailabilityPercentage = _metricsCalculator.GetAvailabilityPercentage(),
                TopAlarms = _alarmTracker.GetTopAlarms(topNAlarms)
            };
        }

    }
}
