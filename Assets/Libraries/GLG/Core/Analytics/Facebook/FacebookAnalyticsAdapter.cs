#if Facebook
using Facebook.Unity;
using System.Collections.Generic;

namespace GLG.Analytics
{
    public class FacebookAnalyticsAdapter : IAnalyticsProvider
    {
        public AnalyticsProvider AnalyticsProviderName => AnalyticsProvider.Facebook;
        public bool IsReady => FB.IsInitialized && _isReady;

        private bool _isReady = false;
        private List<AnalyticsMessage> _pendingMessages = new List<AnalyticsMessage>();

        public bool Flush()
        {
            return _isReady;
        }
        public void Initialize()
        {
            FB.Init(InitializedHandler);
        }
        public bool Track(string id)
        {
            if (!_isReady) return false;
            FB.LogAppEvent(id, null, null);
            return true;
        }
        public bool Track(string id, string name, object value)
        {
            if (!_isReady) return false;
            FB.LogAppEvent(id, null, new Dictionary<string, object>() { { id, value } });
            return true;
        }
        public bool Track(string id, params (string name, object value)[] values)
        {
            if (!_isReady) return false;
            Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (var value in values)
            {
                data.Add(value.name, value.value);
            }
            FB.LogAppEvent(id, null, data);
            return true;
        }
        public bool Track(string id, Dictionary<string, object> values)
        {
            if (!_isReady) return false;
            FB.LogAppEvent(id, null, values);
            return true;
        }
        public bool TrackError(string context, string errorMessage)
        {
            if (!_isReady) return false;
            Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "context", context },
            { "errorMessage", errorMessage }
        };
            FB.LogAppEvent("ERROR", null, data);
            return true;
        }
        public void EnqueueMessage(AnalyticsMessage message)
        {
            _pendingMessages.Add(message);
        }
        public void SendMessagesInQueue()
        {
            if (!_isReady) return;
            AnalyticsMessage currentMessage;
            for (int i = _pendingMessages.Count - 1; i >= 0; i--)
            {
                currentMessage = _pendingMessages[i];
                if (currentMessage.data == null)
                {
                    Track(currentMessage.id);
                }
                else
                {
                    Track(currentMessage.id, currentMessage.data);
                }
            }
            _pendingMessages.Clear();
        }

        private void InitializedHandler()
        {
            _isReady = true;
        }
    }
}
#endif
