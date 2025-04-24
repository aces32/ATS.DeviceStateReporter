using ATS.DeviceStateReporter.Contracts.Services;
using ATS.DeviceStateReporter.Models;
using ATS.DeviceStateReporter.Utilities.Enums;

namespace ATS.DeviceStateReporter.Services
{
    public class LogAnalytics : ILogAnalytics
    {
        private TimeSpan _runningTime = TimeSpan.Zero;
        private TimeSpan _downTime = TimeSpan.Zero;
        private readonly Dictionary<int, TimeSpan> _alarmDurations = [];
        public void Analyze(List<StatePeriod> periods)
        {
            _runningTime = TimeSpan.Zero;
            _downTime = TimeSpan.Zero;
            _alarmDurations.Clear();
            foreach (var period in periods)
            {
                var duration = period.Duration;
                if (period.State == MachineState.Running)
                {
                    _runningTime += duration;
                }
                else if (period.State == MachineState.Faulted)
                {
                    _downTime += duration;

                    if (period.AlarmCode.HasValue)
                    {
                        if (_alarmDurations.ContainsKey(period.AlarmCode.Value))
                            _alarmDurations[period.AlarmCode.Value] += duration;
                        else
                            _alarmDurations[period.AlarmCode.Value] = duration;
                    }
                }
            }
        }

        public double GetAvailabilityPercentage()
        {
            var total = _runningTime + _downTime;
            return total.TotalSeconds == 0 ? 0 : (_runningTime.TotalSeconds / total.TotalSeconds) * 100;
        }

        public List<(int AlarmCode, TimeSpan Duration)> GetTopAlarms(int topN)
        {
            return [.. _alarmDurations
                .OrderByDescending(x => x.Value)
                .Take(topN)
                .Select(x => (x.Key, x.Value))];
        }

        public TimeSpan GetTotalRunningTime() => _runningTime;
        public TimeSpan GetTotalDowntime() => _downTime;
    }
}
