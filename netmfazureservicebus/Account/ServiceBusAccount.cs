using System;
using Microsoft.SPOT;

namespace netmfazureservicebus.Account
{
    public class ServiceBusAccount
    {
        public string NameSpace { get; private set; }
        public string SharedAccessKeyName { get; private set; }
        public string SharedAccessKey { get; private set; }

        public ServiceBusAccount(string nameSpace, string sharedAccessKeyName, string sharedAccessKey)
        {
            NameSpace = nameSpace;
            SharedAccessKeyName = sharedAccessKeyName;
            SharedAccessKey = sharedAccessKey;
        }
    }
}
