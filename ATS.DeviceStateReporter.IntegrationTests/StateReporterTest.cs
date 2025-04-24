using ATS.DeviceStateReporter.Contracts.Services;
using ATS.DeviceStateReporter.Infrastructure;
using ATS.DeviceStateReporter.Services;
using ATS.DeviceStateReporter.Services.Analytics;

namespace ATS.DeviceStateReporter.IntegrationTests
{
    public class StateReporterTest
    {
        private const string CsvContent = @"state,timestamp,alarmcode
                                            running,2023-04-01T08:00:00Z,
                                            faulted,2023-04-01T09:00:00Z,101
                                            running,2023-04-01T10:00:00Z,
                                            faulted,2023-04-01T11:00:00Z,102
                                            running,2023-04-01T12:00:00Z,";

        private ILogAnalytics CreateAnalytics()
        {
            return new LogAnalytics(new AlarmTracker(), new MetricsCalculator());
        }

        [Fact]
        public void FullPipeline_ShouldCalculateCorrectValues()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, CsvContent);

            var parser = new StateCsvParser();
            var processor = new StateReportProcessor();
            var analytics = CreateAnalytics();

            // Act
            var entries = parser.Parse(tempFile);
            var periods = processor.GeneratePeriods(entries);
            var report = analytics.Analyze(periods);

            // Assert
            Assert.Equal(5, entries.Count);
            Assert.True(report.TotalRunningTime.TotalMinutes > 0);
            Assert.True(report.TotalDowntime.TotalMinutes > 0);
            Assert.InRange(report.AvailabilityPercentage, 50, 100);

            var topAlarms = report.TopAlarms;
            Assert.Contains(topAlarms, a => a.AlarmCode == 101);
            Assert.Contains(topAlarms, a => a.AlarmCode == 102);

            File.Delete(tempFile);
        }
    }
}
