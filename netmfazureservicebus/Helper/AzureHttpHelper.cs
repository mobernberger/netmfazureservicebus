using System.Collections;
using System.Net;

namespace netmfazureservicebus.Helper
{
    internal abstract class AzureHttpHelper
    {
        internal static HttpWebResponse SendMessage(string uri, string method, string sasToken, string brokerProperties,
            long contentLength, byte[] buffer, Hashtable customProperties = null)

        {
            var request = (HttpWebRequest) WebRequest.Create(uri);

            request.Method = method;
            request.Headers.Add("Authorization", sasToken);
            request.Headers.Add("ContentType", "application/atom+xml;type=entry;charset=utf-8");
            request.Headers.Add("BrokerProperties", brokerProperties);
            if (customProperties != null)
            {
                foreach (DictionaryEntry customProperty in customProperties)
                {
                    request.Headers.Add(customProperty.Key.ToString(), customProperty.Value.ToString());
                }
            }
            request.ContentLength = contentLength;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(buffer, 0, buffer.Length);
            }
            return (HttpWebResponse) request.GetResponse();
        }

        internal static HttpWebResponse CreateEntity(string uri, string method, string sasToken, long contentLength,
            byte[] buffer)
        {
            var request = (HttpWebRequest) WebRequest.Create(uri + "?timeout=60&api-version=2014-01");

            request.Method = method;
            request.Headers.Add("Authorization", sasToken);
            request.Headers.Add("ContentType", "application/atom+xml;type=entry;charset=utf-8");
            request.ContentLength = contentLength;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(buffer, 0, buffer.Length);
            }
            return (HttpWebResponse)request.GetResponse();
        }

        internal static HttpWebResponse DeleteEntity(string uri, string method, string sasToken)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri + "?timeout=60&api-version=2014-01");

            request.Method = method;
            request.Headers.Add("Authorization", sasToken);
            return (HttpWebResponse)request.GetResponse();
        }
    }
}
