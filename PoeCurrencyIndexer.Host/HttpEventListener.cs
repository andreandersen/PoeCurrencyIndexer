using System;
using System.Diagnostics.Tracing;
using System.Text;

using Microsoft.Extensions.Logging;

namespace PoeCurrencyIndexer.Host
{
    internal sealed class HttpEventListener : EventListener
    {
        private readonly ILogger _logger;
        
        public HttpEventListener(ILogger logger)
        {
            _logger = logger;
        }

        // Constant necessary for attaching ActivityId to the events.
        public const EventKeywords TasksFlowActivityIds = (EventKeywords)0x80;

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            // List of event source names provided by networking in .NET 5.
            if (eventSource.Name.StartsWith("System.Net") ||
                eventSource.Name == "System.Net.Http" ||
                eventSource.Name == "System.Net.Sockets" ||
                eventSource.Name == "System.Net.Security" ||
                eventSource.Name == "System.Net.NameResolution")
            {
                EnableEvents(eventSource, EventLevel.LogAlways);
            }
            // Turn on ActivityId.
            else if (eventSource.Name == "System.Threading.Tasks.TplEventSource")
            {
                // Attach ActivityId to the events.
                EnableEvents(eventSource, EventLevel.LogAlways, TasksFlowActivityIds);
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData) => 
            _logger.LogDebug("HTTP {EventSource} > {EventName}",
                eventData.EventSource.Name, eventData.EventName);
    }
}