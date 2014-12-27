using System;
using System.Collections;
using System.Net;
using System.Text;
using netmfazureservicebus.Account;
using netmfazureservicebus.Helper;
using netmfazureservicebus.SBProperties;
using netmfazureservicebus.Utilities;

namespace netmfazureservicebus.Topics
{
    public class TopicClient
    {
        #region fields
        private readonly ServiceBusAccount _account;
        private readonly string _baseAddress = "";
        private string _topicAddress;
        private HttpWebResponse _response;
        #endregion

        public readonly Hashtable CustomProperties;
        public readonly BrokerProperties BrokerProperties;

        public TopicClient(ServiceBusAccount account)
        {
            _account = account;
            CustomProperties = new Hashtable();
            BrokerProperties = new BrokerProperties();
            _baseAddress = "https://" + _account.NameSpace + ".servicebus.windows.net/";
        }

        public bool SendMessage(string message, string topicName)
        {
            _topicAddress = _baseAddress + topicName + "/messages?timeout=60";

            var sasToken = SAS.CreateSasToken(_baseAddress, _account.SharedAccessKeyName, _account.SharedAccessKey);

            var buffer = Encoding.UTF8.GetBytes(message);

            if (CustomProperties.Count > 0)
            {
                _response = AzureHttpHelper.SendMessage(_topicAddress, "POST", sasToken, BrokerProperties.ConverToJson(),
                    buffer.Length, buffer, CustomProperties);
            }
            else
            {
                _response = AzureHttpHelper.SendMessage(_topicAddress, "POST", sasToken, BrokerProperties.ConverToJson(),
                    buffer.Length, buffer);
            }

            return _response.StatusCode == HttpStatusCode.Created;
        }

        public bool CreateTopic(string topicName, int maxSize = 1024)
        {
            if (maxSize != 1024 && maxSize != 2048 && maxSize != 3072 && maxSize != 4096 && maxSize != 5120)
            {
                throw new ArgumentOutOfRangeException("maxSize",
                    "Only these values are allowed for maxSize: 1024, 2048, 3072, 4096, 5120");
            }

            _topicAddress = _baseAddress + topicName;

            var sasToken = SAS.CreateSasToken(_baseAddress, _account.SharedAccessKeyName, _account.SharedAccessKey);

            var buffer =
                Encoding.UTF8.GetBytes(
                    "<entry xmlns='http://www.w3.org/2005/Atom'>" +
                    "<content type='application/xml'> " +
                    "<TopicDescription xmlns=\"http://schemas.microsoft.com/netservices/2010/10/servicebus/connect\">" +
                    "<MaxSizeInMegaBytes>" + maxSize + "</MaxSizeInMegaBytes>" +
                    "</TopicDescription>" +
                    "</content>" +
                    "</entry>");

            _response = AzureHttpHelper.CreateEntity(_topicAddress, "PUT", sasToken, buffer.Length, buffer);

            return _response.StatusCode == HttpStatusCode.Created;
        }

        public bool DeleteTopic(string queueName)
        {
            _topicAddress = _baseAddress + queueName;

            var sasToken = SAS.CreateSasToken(_baseAddress, _account.SharedAccessKeyName, _account.SharedAccessKey);

            _response = AzureHttpHelper.DeleteEntity(_topicAddress, "DELETE", sasToken);

            return _response.StatusCode == HttpStatusCode.OK;
        }
    }
}
