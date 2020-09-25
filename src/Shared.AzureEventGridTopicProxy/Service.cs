#region Using Statements
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace Shared.AzureEventGridTopicProxy
{
    public class Service: IService
    {
        public string ErrorMessage { get; set; }

        public bool HasError
        {
            get { return !string.IsNullOrEmpty(this.ErrorMessage); }
        }

        private string _topicEndpoint;
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_topicEndpoint))
                {
                    throw new System.Exception("Error: ConnectionString (Topic Endpoint) not set!");
                }
                return _topicEndpoint;
            }
            set { _topicEndpoint = value; }
        }

        private string _topicKey;
        public string Key
        {
            get
            {
                if (string.IsNullOrEmpty(_topicKey))
                {
                    throw new System.Exception("Error: Topic Key not set!");
                }
                return _topicKey;
            }
            set { _topicKey = value; }
        }

        private string TopicHostName
        {
            get
            {
                return new Uri(this.ConnectionString).Host;
            }
        }

        private List<EventGridEvent> BuildEventsList(string eventType, string subject, List<object> eventData)
        {
            List<EventGridEvent> toReturn = new List<EventGridEvent>();
            DateTime now = DateTime.UtcNow;
            foreach(var item in eventData)
            {
                toReturn.Add(
                    new EventGridEvent()
                    {
                        Id = Guid.NewGuid().ToString(),
                        EventType = eventType,
                        Data = item,
                        EventTime = now,
                        Subject = subject,
                        DataVersion = "2.0"
                    });
            }
            return toReturn;
        }

        public void Publish(string eventType, string subject, object eventData)
        {
            this.Publish(eventType, subject, new List<object>() { eventData });
        }

        public void Publish(string eventType, string subject, List<object> eventData)
        {
            try
            {
                List<EventGridEvent> eventsList = this.BuildEventsList(eventType, subject, eventData );
                TopicCredentials topicCredentials = new TopicCredentials(this.Key);
                EventGridClient client = new EventGridClient(topicCredentials);
                client.PublishEventsAsync(this.TopicHostName, eventsList).GetAwaiter().GetResult();
                client = null;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
        }

        public async Task PublishAsync(string eventType, string subject, object eventData)
        {
            await this.PublishAsync(eventType, subject, new List<object>() { eventData });
        }

        public async Task PublishAsync(string eventType, string subject, List<object> eventData)
        {
            try
            {
                List<EventGridEvent> eventsList = this.BuildEventsList(eventType, subject,  eventData );
                TopicCredentials topicCredentials = new TopicCredentials(this.Key);
                EventGridClient client = new EventGridClient(topicCredentials);
                await client.PublishEventsAsync(this.TopicHostName, eventsList);
                client = null;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
        }
    }
}
