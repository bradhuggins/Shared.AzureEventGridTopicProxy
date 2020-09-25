#region Using Statements
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.AzureEventGridTopicProxy;
using Shared.AzureEventGridTopicProxy.Tests.Models;
using System;
using System.Threading.Tasks;
#endregion

namespace Shared.AzureEventGridTopicProxyTests
{
    [TestClass]
    public class ServiceTests
    {
        string _topicEndpoint = "[ENTER TOPIC ENDPOINT (CONNECTION STRING) HERE]";
        string _topicKey = "[ENTER TOPIC KEY HERE]";
        string _eventType = "Shared.AzureEventGridTopicProxy.Tests";
        string _subject = "Unit Test Subject";

        private SampleObject BuildSampleObject()
        {
            return new SampleObject()
            {
                Id =Guid.NewGuid(),
                TimeStamp = DateTime.UtcNow,
                FirstName = "John",
                LastName = "Doe"
            };
        }

        [TestMethod]
        public void HasError_True_Test()
        {
            // Arrange
            Service target = new Service();
            target.ErrorMessage = "error";

            // Act
            var actual = target.HasError;

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void HasError_False_Test()
        {
            // Arrange
            Service target = new Service();

            // Act
            var actual = target.HasError;

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ConnectionString_Error_Test()
        {
            // Arrange
            Service target = new Service();
            bool hasError = false;
            // Act
            try
            {
                var connectionString = target.ConnectionString;
            }
            catch (Exception ex)
            {
                hasError = true;
            }

            // Assert
            Assert.IsTrue(hasError);
        }

        [TestMethod]
        public void Key_Error_Test()
        {
            // Arrange
            Service target = new Service();

            // Act
            string error = null;
            try
            {
                var actual = target.Key;
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            // Assert
            Assert.IsNotNull(error);
        }

        [TestMethod]
        public void Publish_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _topicEndpoint;
            target.Key = _topicKey;

            var eventData = this.BuildSampleObject();

            // Act
            target.Publish(_eventType, _subject, eventData);

            // Assert
            Assert.IsFalse(target.HasError);
        }

        [TestMethod]
        public void Publish_Error_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = "https://this.will.cause.error";
            target.Key = "bad";

            var eventData = this.BuildSampleObject();

            // Act
            target.Publish(_eventType, _subject, eventData);

            // Assert
            Assert.IsTrue(target.HasError);
        }

        [TestMethod]
        public async Task PublishAsync_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _topicEndpoint;
            target.Key = _topicKey;

            var eventData = this.BuildSampleObject();

            // Act
            await target.PublishAsync(_eventType, _subject, eventData);

            // Assert
            Assert.IsFalse(target.HasError);
        }

        [TestMethod]
        public async Task PublishAsync_Error_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = "https://this.will.cause.error";
            target.Key = "bad";

            var eventData = this.BuildSampleObject();

            // Act
            await target.PublishAsync(_eventType, _subject, eventData);

            // Assert
            Assert.IsTrue(target.HasError);
        }


    }

}
