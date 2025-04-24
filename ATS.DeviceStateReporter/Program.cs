using ATS.DeviceStateReporter;
using ATS.DeviceStateReporter.Contracts.Infrastructure;
using ATS.DeviceStateReporter.Contracts.Services;
using ATS.DeviceStateReporter.Contracts.Services.Analytics;
using ATS.DeviceStateReporter.Infrastructure;
using ATS.DeviceStateReporter.Services;
using ATS.DeviceStateReporter.Services.Analytics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddSingleton<IStateCsvParser, StateCsvParser>();
        services.AddSingleton<IStateReportProcessor, StateReportProcessor>();
        services.AddSingleton<ILogAnalytics, LogAnalytics>();
        services.AddSingleton<IAlarmTracker, AlarmTracker>();
        services.AddSingleton<IMetricsCalculator, MetricsCalculator>();
        services.AddSingleton<App>(); 
    });

var app = builder.Build().Services.GetRequiredService<App>();
app.Run(args);
