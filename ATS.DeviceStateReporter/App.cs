using ATS.DeviceStateReporter.Contracts.Infrastructure;
using ATS.DeviceStateReporter.Contracts.Services;
using ATS.DeviceStateReporter.Utilities;
using CsvHelper;

namespace ATS.DeviceStateReporter
{
    public class App
    {
        private readonly IStateCsvParser _stateCsvParser;
        private readonly IStateReportProcessor _stateReportProcessor;
        private readonly ILogAnalytics _logAnalytics;

        public App(IStateCsvParser stateCsvParser,
            IStateReportProcessor stateReportProcessor, ILogAnalytics logAnalytics)
        {
            _stateCsvParser = stateCsvParser;
            _stateReportProcessor = stateReportProcessor;
            _logAnalytics = logAnalytics;
        }

        public void Run(string[] args)
        {
            try
            {
                var filePath = args.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                {
                    Console.WriteLine("Please provide a CSV file path.");
                    Console.ReadLine();
                    return;
                }

                var entries = _stateCsvParser.Parse(filePath);
                var periods = _stateReportProcessor.GeneratePeriods(entries);
                var reports = _logAnalytics.Analyze(periods);
                reports.WriteAnalyticsToConsole();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.Message}");
                Console.ReadLine();
            }
            catch (CsvHelperException ex)
            {
                Console.WriteLine($"CSV format issue: {ex.Message}");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                Console.ReadLine();
            }
        }

    }
}
