using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefTest.OSC
{
    public enum OSCControlType
    {
        Original,
        Mock,
        // Add other implementations if needed
    }

    public static class OSCControlFactory
    {
        public static IOSCControl Get(OSCControlType type)
        {
            switch (type)
            {
                case OSCControlType.Original:
                    return OSCControl.Instance;
                case OSCControlType.Mock:
                    return OSCControlMock.Instance;
                default:
                    throw new ArgumentException("Invalid OSCControlType");
            }
        }
    }
}
