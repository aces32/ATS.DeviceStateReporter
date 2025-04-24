using ATS.DeviceStateReporter.Models;
using ATS.DeviceStateReporter.Services.Analytics;
using ATS.DeviceStateReporter.Utilities.Enums;

namespace ATS.DeviceStateReporter.Tests.Services.Analytics
{
    public class MetricsCalculatorTests
    {



        [Fact]
        public void Process_AccumulatesRunningAndDowntime()
        {
            var metrics = new MetricsCalculator();
            var now = DateTimeOffset.UtcNow;

            metrics.Process(new StatePeriod { State = MachineState.Running, Start = now.AddMinutes(-1), End = now.AddMinutes(5) });
            metrics.Process(new StatePeriod { State = MachineState.Faulted, Start = now.AddMinutes(-1), End = now.AddMinutes(3) });

            Assert.Equal(TimeSpan.FromMinutes(6), metrics.TotalRunningTime);
            Assert.Equal(TimeSpan.FromMinutes(4), metrics.TotalDowntime);
        }

        [Fact]
        public void GetAvailabilityPercentage_ReturnsCorrectRatio()
        {
            var metrics = new MetricsCalculator();
            var now = DateTimeOffset.UtcNow;

            metrics.Process(new StatePeriod { State = MachineState.Running, Start = now, End = now.AddMinutes(4) });
            metrics.Process(new StatePeriod { State = MachineState.Faulted, Start = now, End = now.AddMinutes(6) });

            var availability = metrics.GetAvailabilityPercentage();

            Assert.InRange(availability, 39.9, 40.1);

        }

        [Fact]
        public void Reset_ClearsAllMetrics()
        {
            var metrics = new MetricsCalculator();

            metrics.Process(new StatePeriod { State = MachineState.Running, Start = DateTimeOffset.Now, End = DateTimeOffset.Now.AddMinutes(10) });
            metrics.Reset();

            Assert.Equal(TimeSpan.Zero, metrics.TotalRunningTime);
            Assert.Equal(TimeSpan.Zero, metrics.TotalDowntime);
        }
    }
}
