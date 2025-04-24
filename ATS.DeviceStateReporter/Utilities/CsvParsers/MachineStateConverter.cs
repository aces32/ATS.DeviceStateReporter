using ATS.DeviceStateReporter.Utilities.Enums;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;

namespace ATS.DeviceStateReporter.Utilities.CsvParsers
{
    public class MachineStateConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            return text?.Trim().ToLower() switch
            {
                "running" => MachineState.Running,
                "faulted" => MachineState.Faulted,
                _ => throw new ArgumentException($"Invalid machine state: {text}")
            };
        }
    }
}
