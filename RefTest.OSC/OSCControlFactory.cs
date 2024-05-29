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
        private static IOSCControl _instance;

        public static IOSCControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("Instance is not set. Call SetInstance first.");
                }
                return _instance;
            }
        }


        public static void SetInstance(OSCControlType type)
        {
            switch (type)
            {
                case OSCControlType.Original:
                    _instance = OSCControl.Instance;
                    break;
                case OSCControlType.Mock:
                    _instance = OSCControlMock.Instance;
                    break;
                default:
                    throw new ArgumentException("Invalid OSCControlType");
            }
        }
    }
}
