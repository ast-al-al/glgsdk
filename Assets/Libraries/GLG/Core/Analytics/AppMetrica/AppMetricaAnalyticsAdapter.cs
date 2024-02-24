#if AppMetrica
using System.Collections.Generic;

namespace GLG.Analytics
{
    public class AppMetricaAnalyticsAdapter : IAnalyticsProvider
    {
        public AnalyticsProvider AnalyticsProviderName => AnalyticsProvider.AppMetrica;
        public bool IsReady => _isReady;

        private bool _isReady = false;
        private List<AnalyticsMessage> _pendingMessages = new List<AnalyticsMessage>();

        public bool Flush()
        {
            if (!_isReady) return false;
            AppMetrica.Instance.SendEventsBuffer();
            return true;
        }
        public void Initialize()
        {
            AppMetrica.Instance.OnActivation += PluginActivationHandler;
            AppMetrica.Instance.ActivateWithConfiguration(new YandexAppMetricaConfig(""));
            AppMetrica.Instance.RequestTrackingAuthorization(TrackingAuthorizationHandler);
            AppMetrica.Instance.SetLocationTracking(false);
        }
        public bool Track(string id)
        {
            if (!_isReady) return false;
            AppMetrica.Instance.ReportEvent(id);
            return true;
        }
        public bool Track(string id, string name, object value)
        {
            if (!_isReady) return false;
            AppMetrica.Instance.ReportEvent(id, new Dictionary<string, object>() { { name, value } });
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
            AppMetrica.Instance.ReportEvent(id, data);
            return true;
        }
        public bool Track(string id, Dictionary<string, object> values)
        {
            if (!_isReady) return false;
            AppMetrica.Instance.ReportEvent(id, values);
            return true;
        }
        public bool TrackError(string context, string errorMessage)
        {
            if (!_isReady) return false;
            AppMetrica.Instance.ReportError(context, null, new System.Exception(errorMessage));
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

        private void TrackingAuthorizationHandler(YandexAppMetricaRequestTrackingStatus status)
        {

        }
        private void PluginActivationHandler(YandexAppMetricaConfig config)
        {
            _isReady = true;
        }
    }
}
#endif