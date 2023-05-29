namespace YourNamespace
{
    public interface ISubscriber
    {
        public void HandleMessage(object? sender, EventDataEventArgs e);
    }
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

        public void Subscribe(string eventType, ISubscriber handler)
        {
            _subscribers[eventType] = handler.HandleMessage;
        }

        public void Unsubscribe(string eventType)
        {
            if (_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] = null;
            }
        }

        public void Publish(string eventType, string message)
        {
            if (!_subscribers.ContainsKey(eventType))
            {
                return;
            }
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

        public void PublishMessage(string eventName, string message)
        {
            _broker.Publish(eventName, message);
        }
    }

    // Subscriber class
    public class Subscriber : ISubscriber
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
            broker.Subscribe("messageEvent", subscriber);

            // Publish a message
            publisher.PublishMessage("messageEvent", "Hello, world!");

            // Unsubscribe the subscriber from the "messageEvent"
            broker.Unsubscribe("messageEvent");

            // Publish another message
            publisher.PublishMessage("messageEvent", "Goodbye!");

            Console.ReadLine();
        }
    }
}
