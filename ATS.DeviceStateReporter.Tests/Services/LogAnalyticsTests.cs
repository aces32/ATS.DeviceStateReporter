using ATS.DeviceStateReporter.Models;
using ATS.DeviceStateReporter.Services;
using ATS.DeviceStateReporter.Utilities.Enums;

namespace ATS.DeviceStateReporter.Tests.Services
{
    public class LogAnalyticsTests
    {
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
            var analytics = new LogAnalytics();
            analytics.Analyze(GetSamplePeriods());

            Assert.Equal(TimeSpan.FromMinutes(6), analytics.GetTotalRunningTime());
        }

        [Fact]
        public void Analyze_CorrectlyCalculatesTotalDowntime()
        {
            var analytics = new LogAnalytics();
            analytics.Analyze(GetSamplePeriods());

            Assert.Equal(TimeSpan.FromMinutes(4), analytics.GetTotalDowntime());
        }

        [Fact]
        public void Analyze_CorrectlyCalculatesAvailabilityPercentage()
        {
            var analytics = new LogAnalytics();
            analytics.Analyze(GetSamplePeriods());

            var percent = analytics.GetAvailabilityPercentage();
            Assert.InRange(percent, 60.0, 64.0);
        }

        [Fact]
        public void Analyze_ReturnsTopAlarmsByTotalDuration()
        {
            var analytics = new LogAnalytics();
            analytics.Analyze(GetSamplePeriods());

            var top = analytics.GetTopAlarms(2);

            Assert.Equal(2, top.Count);
            Assert.Equal(101, top[0].AlarmCode);
            Assert.Equal(TimeSpan.FromMinutes(2), top[0].Duration);
        }
    }
}
