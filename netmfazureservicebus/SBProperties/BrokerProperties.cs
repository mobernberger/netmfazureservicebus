using System;
using netmfazureservicebus.Utilities;

namespace netmfazureservicebus.SBProperties
{
    public class BrokerProperties
    {
        public string CorrelationId;

        public string SessionId;

        public string MessageId;

        public string Label;
        
        public string ReplyTo;
        
        public string To;

        public string ReplyToSessionId;

        public long EnqueuedSequenceNumber;

        public string ViaPartitionKey;
        
        public bool ForcePersistence;
        
        public string PartitionKey;

        public TimeSpan TimeToLive;

        public string ConverToJson()
        {
            var json = @"{";

            if (!StringUtility.IsNullOrEmpty(CorrelationId))
            {
                json += "\"CorrelationId\":\"" + CorrelationId + "\",";
            }
            if (!StringUtility.IsNullOrEmpty(SessionId))
            {
                json += "\"SessionId\":\"" + SessionId + "\",";
            }
            if (!StringUtility.IsNullOrEmpty(MessageId))
            {
                json += "\"MessageId\":\"" + MessageId + "\",";
            }
            if (!StringUtility.IsNullOrEmpty(Label))
            {
                json += "\"Label\":\"" + Label + "\",";
            }
            if (!StringUtility.IsNullOrEmpty(ReplyTo))
            {
                json += "\"ReplyTo\":\"" + ReplyTo + "\",";
            }
            if (!StringUtility.IsNullOrEmpty(To))
            {
                json += "\"To\":\"" + To + "\",";
            }
            if (!StringUtility.IsNullOrEmpty(ReplyToSessionId))
            {
                json += "\"ReplyToSessionId\":\"" + ReplyToSessionId + "\",";
            }
            if (EnqueuedSequenceNumber > 0)
            {
                json += "\"EnqueuedSequenceNumber\":" + EnqueuedSequenceNumber + ",";
            }
            if (!StringUtility.IsNullOrEmpty(ViaPartitionKey))
            {
                json += "\"ViaPartitionKey\":\"" + ViaPartitionKey + "\",";
            }
            if (ForcePersistence)
            {
                json += "\"ForcePersistence\":" + "true" + ",";
            }
            if (!StringUtility.IsNullOrEmpty(PartitionKey))
            {
                json += "\"PartitionKey\":\"" + PartitionKey + "\",";
            }
            if (TimeToLive > new TimeSpan(0,0,0))
            {
                var seconds = 0;
                if (TimeToLive.Hours > 0)
                {
                    seconds += TimeToLive.Hours * 3600;
                }
                if (TimeToLive.Minutes > 0)
                {
                    seconds += TimeToLive.Minutes * 60;
                }
                if (TimeToLive.Seconds > 0)
                {
                    seconds += TimeToLive.Seconds;
                }

                json += "\"TimeToLive\":" + seconds + ",";
            }

            json += @"}";
            return json;
        }
    }
}
