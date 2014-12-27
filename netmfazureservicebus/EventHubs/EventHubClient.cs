using System;
using System.Collections;
using System.Net;
using System.Text;
using netmfazureservicebus.Account;
using netmfazureservicebus.Helper;
using netmfazureservicebus.SBProperties;
using netmfazureservicebus.Utilities;

namespace netmfazureservicebus.EventHubs
{
    public class EventHubClient
    {
        #region fields
        private readonly ServiceBusAccount _account;
        private readonly string _baseAddress = "";
        private string _eventHubAddress;
        private HttpWebResponse _response;
        #endregion

        public readonly Hashtable CustomProperties;
        public readonly BrokerProperties BrokerProperties;

        public EventHubClient(ServiceBusAccount account)
        {
            _account = account;
            CustomProperties = new Hashtable();
            BrokerProperties = new BrokerProperties();
            _baseAddress = "https://" + _account.NameSpace + ".servicebus.windows.net/";
        }

        public bool SendMessage(string message, string eventHubName)
        {
            _eventHubAddress = _baseAddress + eventHubName + "/messages?timeout=60";

            var sasToken = SAS.CreateSasToken(_baseAddress, _account.SharedAccessKeyName, _account.SharedAccessKey);

            var buffer = Encoding.UTF8.GetBytes(message);

            if (CustomProperties.Count > 0)
            {
                _response = AzureHttpHelper.SendMessage(_eventHubAddress, "POST", sasToken, BrokerProperties.ConverToJson(),
                    buffer.Length, buffer, CustomProperties);
            }
            else
            {
                _response = AzureHttpHelper.SendMessage(_eventHubAddress, "POST", sasToken, BrokerProperties.ConverToJson(),
                    buffer.Length, buffer);
            }

            return _response.StatusCode == HttpStatusCode.Created;
        }

        public bool CreateEventHub(string hubName, long messageRetentionInDays = 7, int partitionCount = 16)
        {
            if (messageRetentionInDays < 1 && messageRetentionInDays > 7)
            {
                throw new ArgumentOutOfRangeException("partitionCount",
                    "The MessageRetentionInDays must be between 1 and 7");
            }

            if (partitionCount < 8 && partitionCount > 32)
            {
                throw new ArgumentOutOfRangeException("partitionCount",
                    "The PartitionCount must be between 8 and 32");
            }

            _eventHubAddress = _baseAddress + hubName;

            var sasToken = SAS.CreateSasToken(_baseAddress, _account.SharedAccessKeyName, _account.SharedAccessKey);

            var buffer =
                Encoding.UTF8.GetBytes(
                    "<entry xmlns='http://www.w3.org/2005/Atom'>" +
                    "<content type='application/xml'> " +
                    "<EventHubDescription xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.microsoft.com/netservices/2010/10/servicebus/connect\">" +
                    "<MessageRetentionInDays>" + messageRetentionInDays + "</MessageRetentionInDays>" +
                    "<PartitionCount>" + partitionCount + "</PartitionCount>" +
                    "</EventHubDescription>" +
                    "</content>" +
                    "</entry>");

            _response = AzureHttpHelper.CreateEntity(_eventHubAddress, "PUT", sasToken, buffer.Length, buffer);

            return _response.StatusCode == HttpStatusCode.Created;
        }

        public bool DeleteEventHub(string queueName)
        {
            _eventHubAddress = _baseAddress + queueName;

            var sasToken = SAS.CreateSasToken(_baseAddress, _account.SharedAccessKeyName, _account.SharedAccessKey);

            _response = AzureHttpHelper.DeleteEntity(_eventHubAddress, "DELETE", sasToken);

            return _response.StatusCode == HttpStatusCode.OK;
        }
    }
}
