using System;

namespace Shared.AzureEventGridTopicProxy.Tests.Models
{
    public class SampleObject
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
