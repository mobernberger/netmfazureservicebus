netmfazureservicebus
====================

A .NET Micro Framework library for communicating with Azure Service (Queues, Topics and Event Hubs)

#### Credits
---

I have gotten the SAS Token Generator from: http://blog.devmobile.co.nz/2014/11/30/azure-event-hub-updates-from-a-netmf-device/ so great thanks for the great work.

#### Requirements
---
* .NET Micro Framework 4.3
* Visual Studio
* An Azure Subscription to create a Service Bus Namespace


#### Get Started:
---

* Create a new Service Bus Namespace inside the Azure Portal
* Take note of your Shared Access Key which will be automatically generate when creating a new Service Bus Namespace.

After these two steps your are ready to start.

### Example Usage of the library:
---

At first you have to create a new Instance of the ServiceBusAccount Class with your Service Bus Namespace, your Shared Access Key Name and your Shared Access Key.
```c#
var account = new ServiceBusAccount(
                "test-ns-1",
                "RootManageSharedAccessKey",
                "ieDIqTtF24fgxu4nRMFHauo4U7+NMqweoyOmzrMVQ1=");
```

After this you could work with one of the three Service Bus Items (Queues, Topics and Event Hubs)
I am only showing Queues here in the readme, but the other two are nearly identical and you could it out in the Test Application.

----
We are now creating a new Instance of the EventHubClient:

```c#
var queueClient = new QueueClient(account);
```
#### Create a new queue
You could now create a new queue directly from your device. The method returns a bool to check if the creation process was successfull.
```c#
if (queueClient.CreateQueue("test-queue1"))
{
    Debug.Print("Queue successfully created");
}
```

#### Send a message to the new queue
We will now dispatch a message into the newly created queue.
```c#
queueClient.BrokerProperties.Label = "Testlabel";
queueClient.BrokerProperties.CorrelationId = "NewId";
queueClient.BrokerProperties.ForcePersistence = true;
queueClient.BrokerProperties.TimeToLive = new TimeSpan(0, 3, 22);
queueClient.CustomProperties.Add("Customer", "MS");
queueClient.CustomProperties.Add("Importance", "High");
if (queueClient.SendMessage("Hello from netmf", "test-queue1"))
{
  Debug.Print("Message to Queue successfully sent");
}
```
There are two collections inside the QueueClient. One of this is Brokerproperties which is a nearly identical Implementation of the BrokeredProperties Class inside the ServiceBusClient (http://msdn.microsoft.com/en-us/library/microsoft.servicebus.messaging.brokeredmessage_properties.aspx) and a HashTable called "CustomProperties" where you could any custom Properties you want to send inside the message. After you have added your properties you could now add an inside body for the message via the first parameter and send it to the queue.

#### Delete the new queue
After all our Tests are finished we will now delete the queue.
```c#
if (queueClient.DeleteQueue("test-queue1"))
{
  Debug.Print("Queue successfully deleted");
}
```

After all your tests are finished you are ready to go and implement this into your application. I am happy to see your implemntations and hear about that. You could contact me on Twitter: @mobernberger 
