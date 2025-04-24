using ATS.DeviceStateReporter.Models;
using ATS.DeviceStateReporter.Services;
using ATS.DeviceStateReporter.Utilities.Enums;

namespace ATS.DeviceStateReporter.Tests.Services
{
    public class StateReportProcessorTests
    {

        [Fact]
        public void GeneratePeriods_ShouldPreserveOrderAndTimestamps()
        {
            var now = DateTime.UtcNow;
            var entries = new List<StateReportEntry>
            {
                new() { State = MachineState.Running, Timestamp = now.AddMinutes(-5) },
                new() { State = MachineState.Faulted, Timestamp = now.AddMinutes(-3) },
            };

            var processor = new StateReportProcessor();
            var periods = processor.GeneratePeriods(entries);

            Assert.Equal(entries[0].Timestamp, periods[0].Start);
            Assert.Equal(entries[1].Timestamp, periods[0].End);
        }

        [Fact]
        public void GeneratePeriods_WithNoEntries_ReturnsEmptyList()
        {
            var entries = new List<StateReportEntry>();

            var processor = new StateReportProcessor();
            var periods = processor.GeneratePeriods(entries);

            Assert.Empty(periods);
        }

        [Fact]
        public void GeneratePeriods_WithMultipleEntries_ReturnsCorrectPeriodCount()
        {
            var entries = new List<StateReportEntry>
            {
                new() { State = MachineState.Running, Timestamp = DateTime.UtcNow.AddMinutes(-4) },
                new() { State = MachineState.Faulted, Timestamp = DateTime.UtcNow.AddMinutes(-3) },
                new() { State = MachineState.Running, Timestamp = DateTime.UtcNow.AddMinutes(-2) },
            };

            var processor = new StateReportProcessor();
            var periods = processor.GeneratePeriods(entries);

            Assert.Equal(3, periods.Count);
        }

        [Fact]
        public void GeneratePeriods_WithSingleEntry_ShouldReturnOpenEndedPeriod()
        {
            var timestamp = DateTime.UtcNow.AddMinutes(-10);
            var entry = new StateReportEntry
            {
                State = MachineState.Running,
                Timestamp = timestamp,
                AlarmCode = null
            };

            var processor = new StateReportProcessor();
            var periods = processor.GeneratePeriods(new List<StateReportEntry>{ entry });

            Assert.Single(periods);
            Assert.Equal(MachineState.Running, periods[0].State);
            Assert.Equal(timestamp, periods[0].Start);
            Assert.True(periods[0].End > timestamp);
        }
    }
}
