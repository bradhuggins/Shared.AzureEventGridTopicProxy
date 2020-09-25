#region Using Statemets
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace Shared.AzureEventGridTopicProxy
{
    public interface IService
    {
        string ErrorMessage { get; set; }
        bool HasError { get; }
        string ConnectionString { get; set; }
        string Key { get; set; }

        void Publish(string eventType, string subject, object eventData);
        void Publish(string eventType, string subject, List<object> eventData);

        Task PublishAsync(string eventType, string subject, object eventData);
        Task PublishAsync(string eventType, string subject, List<object> eventData);
    }
}
