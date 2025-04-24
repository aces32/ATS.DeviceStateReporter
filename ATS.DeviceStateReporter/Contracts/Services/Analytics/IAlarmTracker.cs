namespace ATS.DeviceStateReporter.Contracts.Services.Analytics
{
    public interface IAlarmTracker
    {
        void Track(int? alarmCode, TimeSpan duration);
        void Clear();
        List<(int AlarmCode, TimeSpan Duration)> GetTopAlarms(int topN);
    }
}
