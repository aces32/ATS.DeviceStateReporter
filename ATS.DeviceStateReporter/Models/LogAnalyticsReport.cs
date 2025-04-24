namespace ATS.DeviceStateReporter.Models
{
    public class LogAnalyticsReport
    {
        public TimeSpan TotalRunningTime { get; set; }
        public TimeSpan TotalDowntime { get; set; }
        public double AvailabilityPercentage { get; set; }
        public List<(int AlarmCode, TimeSpan Duration)> TopAlarms { get; set; } = new();
    }

}
