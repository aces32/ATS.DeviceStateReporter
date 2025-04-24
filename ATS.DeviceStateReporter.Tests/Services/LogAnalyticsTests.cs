using ATS.DeviceStateReporter.Contracts.Services;
using ATS.DeviceStateReporter.Models;
using ATS.DeviceStateReporter.Services;
using ATS.DeviceStateReporter.Services.Analytics;
using ATS.DeviceStateReporter.Utilities.Enums;

namespace ATS.DeviceStateReporter.Tests.Services
{
    public class LogAnalyticsTests
    {
        private ILogAnalytics CreateAnalytics()
        {
            return new LogAnalytics(new AlarmTracker(), new MetricsCalculator());
        }

        private List<StatePeriod> GetSamplePeriods()
        {
            var now = DateTimeOffset.UtcNow;
            return new List<StatePeriod>
            {
                new() { State = MachineState.Running, Start = now.AddMinutes(-10), End = now.AddMinutes(-8) },
                new() { State = MachineState.Faulted, Start = now.AddMinutes(-8), End = now.AddMinutes(-6), AlarmCode = 101 },
                new() { State = MachineState.Running, Start = now.AddMinutes(-6), End = now.AddMinutes(-3) },
                new() { State = MachineState.Faulted, Start = now.AddMinutes(-3), End = now.AddMinutes(-1), AlarmCode = 102 },
                new() { State = MachineState.Running, Start = now.AddMinutes(-1), End = now },
            };
        }

        [Fact]
        public void Analyze_CorrectlyCalculatesTotalRunningTime()
        {
            var analytics = CreateAnalytics();
            var report = analytics.Analyze(GetSamplePeriods());

            Assert.Equal(TimeSpan.FromMinutes(6), report.TotalRunningTime);
        }

        [Fact]
        public void Analyze_CorrectlyCalculatesTotalDowntime()
        {
            var analytics = CreateAnalytics();
            var report = analytics.Analyze(GetSamplePeriods());

            Assert.Equal(TimeSpan.FromMinutes(4), report.TotalDowntime);
        }

        [Fact]
        public void Analyze_CorrectlyCalculatesAvailabilityPercentage()
        {
            var analytics = CreateAnalytics();
            var report = analytics.Analyze(GetSamplePeriods());

            var percent = report.AvailabilityPercentage;
            Assert.InRange(percent, 60.0, 64.0);
        }

        [Fact]
        public void Analyze_ReturnsTopAlarmsByTotalDuration()
        {
            var analytics = CreateAnalytics();
            var report = analytics.Analyze(GetSamplePeriods());

            var top = report.TopAlarms;

            Assert.Equal(2, top.Count);
            Assert.Equal(101, top[0].AlarmCode);
            Assert.Equal(TimeSpan.FromMinutes(2), top[0].Duration);
        }
    }
}
