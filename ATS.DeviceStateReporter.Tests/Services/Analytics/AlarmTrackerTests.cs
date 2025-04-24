using ATS.DeviceStateReporter.Services.Analytics;

namespace ATS.DeviceStateReporter.Tests.Services.Analytics
{
    public class AlarmTrackerTests
    {

        [Fact]
        public void Track_AddsDurationForEachAlarmCode()
        {
            var tracker = new AlarmTracker();

            tracker.Track(101, TimeSpan.FromMinutes(2));
            tracker.Track(102, TimeSpan.FromMinutes(3));
            tracker.Track(101, TimeSpan.FromMinutes(4));

            var top = tracker.GetTopAlarms(2);

            Assert.Equal(2, top.Count);
            Assert.Equal(101, top[0].AlarmCode);
            Assert.Equal(TimeSpan.FromMinutes(6), top[0].Duration);
            Assert.Equal(102, top[1].AlarmCode);
            Assert.Equal(TimeSpan.FromMinutes(3), top[1].Duration);
        }

        [Fact]
        public void Track_DoesNotTrackTrackIfAlarmIsNull()
        {
            var tracker = new AlarmTracker();
            tracker.Track(null, TimeSpan.FromMinutes(5));

            var top = tracker.GetTopAlarms(1);
            Assert.Empty(top);
        }

        [Fact]
        public void Clear_RemovesAllTrackedData()
        {
            var tracker = new AlarmTracker();
            tracker.Track(101, TimeSpan.FromMinutes(5));

            tracker.Clear();

            var top = tracker.GetTopAlarms(1);
            Assert.Empty(top);
        }
    }
}
