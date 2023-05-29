# Broker

In this example, we have a MessageEventArgs class representing the event arguments for the message event. The Publisher class sends messages to the broker using the PublishMessage method.

The Subscriber class has a HandleMessage method that is invoked when a message is received. Multiple subscribers can be created, and each subscriber can handle the messages in its own way.

The Broker class acts as the central communication hub. It maintains a dictionary of subscribers and their corresponding message handlers. Subscribers can subscribe to the broker using the Subscribe method and unsubscribe using the Unsubscribe method. When a message is published through the broker using the PublishMessage method, the broker distributes the message to all subscribed subscribers.

In the Program class, we create instances of the broker, publisher, and subscribers. We subscribe the subscribers to the broker using the Subscribe method. The publisher then sends messages through the broker using the SendMessage method. We also demonstrate how to unsubscribe a subscriber from the broker using the Unsubscribe method.

When executed, the program demonstrates how the publisher sends messages to the broker, and the broker distributes those messages to the subscribed subscribers, which handle the messages accordingly.

This example illustrates how the Broker pattern helps decouple components and enables loose coupling and flexibility in communication between publishers and subscribers.
