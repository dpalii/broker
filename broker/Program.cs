using System;
using System.Collections.Generic;

namespace YourNamespace
{
    // Event arguments for the event published by the broker
    public class EventDataEventArgs : EventArgs
    {
        public string Message { get; set; }

        public EventDataEventArgs(string message)
        {
            Message = message;
        }
    }

    // Broker or event aggregator
    public class EventBroker
    {
        private readonly Dictionary<string, EventHandler<EventDataEventArgs>?> _subscribers = new Dictionary<string, EventHandler<EventDataEventArgs>?>();

        public void Subscribe(string eventType, EventHandler<EventDataEventArgs> handler)
        {
            if (!_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] = null;
            }

            _subscribers[eventType] += handler;
        }

        public void Unsubscribe(string eventType, EventHandler<EventDataEventArgs> handler)
        {
            if (_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] -= handler;
            }
        }

        public void Publish(string eventType, string message)
        {
            EventHandler<EventDataEventArgs>? handler = _subscribers[eventType];
            if (handler != null)
            {
                var args = new EventDataEventArgs(message);
                handler(this, args);
            }
        }
    }

    // Publisher class
    public class Publisher
    {
        private readonly EventBroker _broker;

        public Publisher(EventBroker broker)
        {
            _broker = broker;
        }

        public void PublishMessage(string message)
        {
            _broker.Publish("messageEvent", message);
        }
    }

    // Subscriber class
    public class Subscriber
    {
        public void HandleMessage(object? sender, EventDataEventArgs e)
        {
            Console.WriteLine("Received message: " + e.Message);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of the broker
            var broker = new EventBroker();

            // Create an instance of the publisher and subscriber
            var publisher = new Publisher(broker);
            var subscriber = new Subscriber();

            // Subscribe the subscriber to the "messageEvent"
            broker.Subscribe("messageEvent", subscriber.HandleMessage);

            // Publish a message
            publisher.PublishMessage("Hello, world!");

            // Unsubscribe the subscriber from the "messageEvent"
            broker.Unsubscribe("messageEvent", subscriber.HandleMessage);

            // Publish another message
            publisher.PublishMessage("Goodbye!");

            Console.ReadLine();
        }
    }
}
