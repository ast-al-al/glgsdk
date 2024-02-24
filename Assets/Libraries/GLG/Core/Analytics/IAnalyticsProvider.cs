using System.Collections.Generic;

namespace GLG.Analytics
{
    public interface IAnalyticsProvider
    {
        public AnalyticsProvider AnalyticsProviderName { get; }
        public bool IsReady { get; }
        public void Initialize();
        public bool Track(string id);
        public bool Track(string id, string name, object value);
        public bool Track(string id, params (string name, object value)[] values);
        public bool Track(string id, Dictionary<string, object> values);
        public bool TrackError(string context, string errorMessage);
        public bool Flush();
        public void EnqueueMessage(AnalyticsMessage message);
        public void SendMessagesInQueue();
    }
}