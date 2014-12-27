using System;
using System.Collections;
using System.Net;
using System.Text;
using netmfazureservicebus.Account;
using netmfazureservicebus.Helper;
using netmfazureservicebus.SBProperties;
using netmfazureservicebus.Utilities;

namespace netmfazureservicebus.Queues
{
    public class QueueClient
    {

        #region fields
        private readonly ServiceBusAccount _account;
        private readonly string _baseAddress = "";
        private string _queueAddress;
        private HttpWebResponse _response;
        #endregion

        public readonly Hashtable CustomProperties;
        public readonly BrokerProperties BrokerProperties;

        public QueueClient(ServiceBusAccount account)
        {
            _account = account;
            CustomProperties = new Hashtable();
            BrokerProperties = new BrokerProperties();
            _baseAddress = "https://" + _account.NameSpace + ".servicebus.windows.net/";
        }

        public bool SendMessage(string message, string queueName)
        {
            _queueAddress = _baseAddress + queueName + "/messages?timeout=60";

            var sasToken = SAS.CreateSasToken(_baseAddress, _account.SharedAccessKeyName, _account.SharedAccessKey);

            var buffer = Encoding.UTF8.GetBytes(message);
            
            if (CustomProperties.Count > 0)
            {
                _response = AzureHttpHelper.SendMessage(_queueAddress, "POST", sasToken, BrokerProperties.ConverToJson(),
                    buffer.Length, buffer, CustomProperties);
            }
            else
            {
                _response = AzureHttpHelper.SendMessage(_queueAddress, "POST", sasToken, BrokerProperties.ConverToJson(),
                    buffer.Length, buffer);
            }

            return _response.StatusCode == HttpStatusCode.Created;
        }

        public bool CreateQueue(string queueName, int maxSize=1024)
        {
            if (maxSize != 1024 && maxSize != 2048 && maxSize != 3072 && maxSize != 4096 && maxSize != 5120)
            {
                throw new ArgumentOutOfRangeException("maxSize",
                    "Only these values are allowed for maxSize: 1024, 2048, 3072, 4096, 5120");
            }

            _queueAddress = _baseAddress + queueName;

            var sasToken = SAS.CreateSasToken(_baseAddress, _account.SharedAccessKeyName, _account.SharedAccessKey);

            var buffer =
                Encoding.UTF8.GetBytes(
                    "<entry xmlns='http://www.w3.org/2005/Atom'>" +
                    "<content type='application/xml'> " +
                    "<QueueDescription xmlns=\"http://schemas.microsoft.com/netservices/2010/10/servicebus/connect\">" +
                    "<MaxSizeInMegaBytes>" + maxSize + "</MaxSizeInMegaBytes>" +
                    "</QueueDescription>" +
                    "</content>" +
                    "</entry>");

            _response = AzureHttpHelper.CreateEntity(_queueAddress, "PUT", sasToken, buffer.Length, buffer);

            return _response.StatusCode == HttpStatusCode.Created;
        }

        public bool DeleteQueue(string queueName)
        {
            _queueAddress = _baseAddress + queueName;

            var sasToken = SAS.CreateSasToken(_baseAddress, _account.SharedAccessKeyName, _account.SharedAccessKey);
            
            _response = AzureHttpHelper.DeleteEntity(_queueAddress, "DELETE", sasToken);

            return _response.StatusCode == HttpStatusCode.OK;
        }
    }
}
