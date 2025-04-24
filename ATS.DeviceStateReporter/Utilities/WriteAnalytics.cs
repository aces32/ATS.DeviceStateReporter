using ATS.DeviceStateReporter.Contracts.Services;
using ATS.DeviceStateReporter.Models;

namespace ATS.DeviceStateReporter.Utilities
{
    public static class WriteAnalytics
    {
        public static void WriteAnalyticsToConsole(this LogAnalyticsReport analyticsReport)
        {
            Console.WriteLine($"Total Running Time: {analyticsReport.TotalRunningTime}");
            Console.WriteLine($"Total Downtime:     {analyticsReport.TotalDowntime}");
            Console.WriteLine($"Availability:       {analyticsReport.AvailabilityPercentage:F2}%");

            Console.WriteLine("\nTop 5 Alarms by Duration:");
            foreach (var (alarm, duration) in analyticsReport.TopAlarms)
            {
                Console.WriteLine($"Alarm {alarm}: {duration}");
            }
            Console.ReadLine();
        }
    }
}
