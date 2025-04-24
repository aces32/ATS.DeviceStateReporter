using ATS.DeviceStateReporter.Contracts.Services.Analytics;

namespace ATS.DeviceStateReporter.Services.Analytics
{
    public class AlarmTracker : IAlarmTracker
    {
        public readonly Dictionary<int, TimeSpan> _alarmDurations = [];
        public void Clear()  => _alarmDurations.Clear();

        public List<(int AlarmCode, TimeSpan Duration)> GetTopAlarms(int topN)
        {
            return [.. _alarmDurations
                .OrderByDescending(x => x.Value)
                .Take(topN)
                .Select(x => (x.Key, x.Value))];
        }

        public void Track(int? alarmCode, TimeSpan duration)
        {
            if (!alarmCode.HasValue) return;

            if (_alarmDurations.ContainsKey(alarmCode.Value))
                _alarmDurations[alarmCode.Value] += duration;
            else 
                _alarmDurations[alarmCode.Value] = duration;       
        }
    }
}
