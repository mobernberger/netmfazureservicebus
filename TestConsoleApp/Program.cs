using System;
using Microsoft.SPOT;
using netmfazureservicebus.Account;
using netmfazureservicebus.EventHubs;
using netmfazureservicebus.Queues;
using netmfazureservicebus.Topics;

namespace TestConsoleApp
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("Program started");

            var account = new ServiceBusAccount(
                "<The name of your Service Bus Namespace>",
                "<Your SharedAccessKeyName from the Service Bus>",
                "<Your RootManageSharedAccessKey from the Service Bus>");

            //Please check your Date and Time configuration or sync it before you run the Client on a real device

            RunQueueTests(account);

            RunTopicTests(account);

            RunEventHubTests(account);
        }

        private static void RunEventHubTests(ServiceBusAccount account)
        {
            //Start Event Hub Tests
            var eventHubClient = new EventHubClient(account);
            const string eventHubName = "test-hub1-netmf";

            //Create a new Event Hub
            if (eventHubClient.CreateEventHub(eventHubName, 5, 12))
            {
                Debug.Print("Event Hub successfully created");
            }

            //Prepare a message and send it to the newly created Event Hub
            eventHubClient.BrokerProperties.Label = "Testlabel";
            eventHubClient.BrokerProperties.ForcePersistence = true;
            eventHubClient.BrokerProperties.TimeToLive = new TimeSpan(0, 2, 8);
            eventHubClient.CustomProperties.Add("Customer", "MS");
            eventHubClient.CustomProperties.Add("Importance", "High");
            if (eventHubClient.SendMessage("Hello from netmf", eventHubName))
            {
                Debug.Print("Message to Event Hub successfully sent");
            }

            //Delete the newly created Test Event Hub
            if (eventHubClient.DeleteEventHub(eventHubName))
            {
                Debug.Print("Event Hub successfully deleted");
            }
        }

        private static void RunTopicTests(ServiceBusAccount account)
        {
            //Start Topic Tests
            var topicClient = new TopicClient(account);
            const string topicName = "test-topic1-netmf";

            //Create a new Topic
            if (topicClient.CreateTopic(topicName))
            {
                Debug.Print("Topic successfully created");
            }

            //Prepare a message and send it to the newly created topic
            topicClient.BrokerProperties.Label = "Testlabel";
            topicClient.BrokerProperties.ForcePersistence = true;
            topicClient.BrokerProperties.TimeToLive = new TimeSpan(0, 2, 8);
            topicClient.CustomProperties.Add("Customer", "MS");
            topicClient.CustomProperties.Add("Importance", "High");
            if (topicClient.SendMessage("Hello from netmf", topicName))
            {
                Debug.Print("Message to Topic successfully sent");
            }

            //Delete the newly created Test Topic
            if (topicClient.DeleteTopic(topicName))
            {
                Debug.Print("Topic successfully deleted");
            }
        }

        private static void RunQueueTests(ServiceBusAccount account)
        {
            //Start Queue Tests
            var queueClient = new QueueClient(account);
            const string queueName = "test-queue1-netmf";

            //Create a new Queue
            if (queueClient.CreateQueue(queueName))
            {
                Debug.Print("Queue successfully created");
            }

            //Prepare a message and send it to the newly created queue
            queueClient.BrokerProperties.Label = "Testlabel";
            queueClient.BrokerProperties.CorrelationId = "NewId";
            queueClient.BrokerProperties.ForcePersistence = true;
            queueClient.BrokerProperties.TimeToLive = new TimeSpan(0, 3, 22);
            queueClient.CustomProperties.Add("Customer", "MS");
            queueClient.CustomProperties.Add("Importance", "High");
            if (queueClient.SendMessage("Hello from netmf", queueName))
            {
                Debug.Print("Message to Queue successfully sent");
            }

            //Delete the newly created Test Queue
            if (queueClient.DeleteQueue(queueName))
            {
                Debug.Print("Queue successfully deleted");
            }
        }
    }
}
