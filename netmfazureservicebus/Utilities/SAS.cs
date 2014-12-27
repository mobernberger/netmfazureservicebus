using System;
using System.Text;

namespace netmfazureservicebus.Utilities
{
    public static class SAS
    {
        public static string CreateSasToken(string uri, string keyName, string key)
        {
            // Set token lifetime to 20 minutes. When supplying a device with a token, you might want to use a longer expiration time.
            var tokenExpirationTime = GetExpiry(20 * 60);

            var stringToSign = HttpUtility.UrlEncode(uri) + "\n" + tokenExpirationTime;
            var hmac = SHA.computeHMAC_SHA256(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(stringToSign));

            var signature = Convert.ToBase64String(hmac);
            signature = Base64NetMf42ToRfc4648(signature);

            var token = "SharedAccessSignature sr=" + HttpUtility.UrlEncode(uri) + "&sig=" + HttpUtility.UrlEncode(signature) + "&se=" + tokenExpirationTime.ToString() + "&skn=" + keyName;
            return token;
        }

        private static uint GetExpiry(uint tokenLifetimeInSeconds)
        {
            const long ticksPerSecond = 1000000000 / 100; // 1 tick = 100 nano seconds

            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var diff = DateTime.Now.ToUniversalTime() - origin;
            return ((uint)(diff.Ticks / ticksPerSecond)) + tokenLifetimeInSeconds;
        }

        private static string Base64NetMf42ToRfc4648(string base64NetMf)
        {
            var base64Rfc = string.Empty;
            for (var i = 0; i < base64NetMf.Length; i++)
            {
                if (base64NetMf[i] == '!')
                {
                    base64Rfc += '+';
                }
                else if (base64NetMf[i] == '*')
                {
                    base64Rfc += '/';
                }
                else
                {
                    base64Rfc += base64NetMf[i];
                }
            }
            return base64Rfc;
        }

    }
}
