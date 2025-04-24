using ATS.DeviceStateReporter.Contracts.Services;

namespace ATS.DeviceStateReporter.Utilities
{
    public static class WriteAnalytics
    {
        public static void WriteAnalyticsToConsole(this ILogAnalytics analytics)
        {
            Console.WriteLine($"Total Running Time: {analytics.GetTotalRunningTime()}");
            Console.WriteLine($"Total Downtime:     {analytics.GetTotalDowntime()}");
            Console.WriteLine($"Availability:       {analytics.GetAvailabilityPercentage():F2}%");

            Console.WriteLine("\nTop 5 Alarms by Duration:");
            foreach (var (alarm, duration) in analytics.GetTopAlarms(5))
            {
                Console.WriteLine($"Alarm {alarm}: {duration}");
            }
            Console.ReadLine();
        }
    }
}
