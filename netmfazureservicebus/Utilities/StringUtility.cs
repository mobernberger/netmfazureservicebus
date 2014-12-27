using System;
using Microsoft.SPOT;

namespace netmfazureservicebus.Utilities
{
    public abstract class StringUtility
    {
        public static bool IsNullOrEmpty(string str)
        {
            if (str == null || str == string.Empty)
                return true;

            return false;
        }
    }
}
