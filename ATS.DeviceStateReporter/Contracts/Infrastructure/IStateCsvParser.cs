using ATS.DeviceStateReporter.Models;

namespace ATS.DeviceStateReporter.Contracts.Infrastructure
{
    public interface IStateCsvParser
    {
        List<StateReportEntry> Parse(string filePath);
    }
}
