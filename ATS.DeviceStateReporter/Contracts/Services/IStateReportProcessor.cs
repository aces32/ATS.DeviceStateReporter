using ATS.DeviceStateReporter.Models;

namespace ATS.DeviceStateReporter.Contracts.Services
{
    public interface IStateReportProcessor
    {
        List<StatePeriod> GeneratePeriods(List<StateReportEntry> entries);
    }
}
