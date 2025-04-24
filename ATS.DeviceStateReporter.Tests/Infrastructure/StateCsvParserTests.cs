using ATS.DeviceStateReporter.Infrastructure;
using ATS.DeviceStateReporter.Utilities.Enums;
using CsvHelper;
using System.Globalization;

namespace ATS.DeviceStateReporter.Tests.Infrastructure
{
    public class StateCsvParserTests
    {
        private const string CsvContent = @"state,timestamp,alarmcode
                                            running,2025-04-01T08:00:00Z,
                                            faulted,2025-04-01T09:00:00Z,101
                                            running,2025-04-01T10:00:00Z,
                                            faulted,2025-04-01T11:00:00Z,102";

        [Fact]
        public void Parse_ValidCsv_ReturnsCorrectEntries()
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, CsvContent);

            var parser = new StateCsvParser();
            var entries = parser.Parse(tempFile);

            Assert.Equal(4, entries.Count);
            Assert.Equal(MachineState.Running, entries[0].State);
            Assert.Equal(DateTimeOffset.Parse("2025-04-01T08:00:00Z", null, DateTimeStyles.AdjustToUniversal), entries[0].Timestamp);
            Assert.Null(entries[0].AlarmCode);

            Assert.Equal(MachineState.Faulted, entries[1].State);
            Assert.Equal(101, entries[1].AlarmCode);

            File.Delete(tempFile);
        }

        [Fact]
        public void Parse_InvalidState_ThrowsArgumentException()
        {
            var invalidCsv = @"state,timestamp,alarmcode
                                paused,2023-04-01T08:00:00Z,201";
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, invalidCsv);

            var parser = new StateCsvParser();
            var ex = Assert.Throws<ReaderException>(() => parser.Parse(tempFile));
            Assert.IsType<ArgumentException>(ex.InnerException);

            File.Delete(tempFile);
        }

        [Fact]
        public void Parse_EmptyFile_ReturnsEmptyList()
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "state,timestamp,alarmcode\n");

            var parser = new StateCsvParser();
            var entries = parser.Parse(tempFile);

            Assert.Empty(entries);

            File.Delete(tempFile);
        }
    }
}
