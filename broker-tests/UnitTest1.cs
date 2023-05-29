using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace YourNamespace.Tests
{
    [TestClass]
    public class BrokerTests
    {
        [TestMethod]
        public void PublishMessage_ShouldNotifySubscribers()
        {
            // Arrange
            var broker = new EventBroker();
            var publisher = new Publisher(broker);
            var subscriber1 = new Mock<ISubscriber>();
            var subscriber2 = new Mock<ISubscriber>();

            broker.Subscribe("event1", subscriber1.Object);

            broker.Subscribe("event2", subscriber2.Object);

            // Act
            publisher.PublishMessage("event1", "Test Message 1");
            publisher.PublishMessage("event2", "Test Message 2");
            publisher.PublishMessage("event2", "Test Message 3");
            publisher.PublishMessage("event3", "Test Message 4");

            // Assert
            subscriber1.Verify(s => s.HandleMessage(It.IsAny<EventBroker>(), It.IsAny<EventDataEventArgs>()), Times.Exactly(1));
            subscriber2.Verify(s => s.HandleMessage(It.IsAny<EventBroker>(), It.IsAny<EventDataEventArgs>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Unsubscribe_ShouldStopNotifyingSubscribedSubscriber()
        {
            // Arrange
            var broker = new EventBroker();
            var publisher = new Publisher(broker);
            var subscriber = new Mock<ISubscriber>();

            broker.Subscribe("messageEvent", subscriber.Object);

            // Act
            publisher.PublishMessage("messageEvent", "Test Message 1");
            broker.Unsubscribe("messageEvent");
            publisher.PublishMessage("messageEvent", "Test Message 2");

            // Assert
            subscriber.Verify(s => s.HandleMessage(It.IsAny<EventBroker>(), It.IsAny<EventDataEventArgs>()), Times.Exactly(1));
        }
    }
}
