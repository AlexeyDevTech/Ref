

using RefTest.OSC;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RefTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var import = new OSCImport();
            try
            {
                var devs = SearchDevices();
                var con = Connect();
                Console.WriteLine(con);
                if (con)
                {
                    ushort ver = GetVersion();
                    Console.WriteLine(ver);
                    Console.ReadKey();

                    //ushort len = 128;
                    //var arr = new ushort[len];

                    //var suc = OSCImport.dsoHTReadCalibrationData(0, arr, len);
                }
            }
            catch (AccessViolationException)
            {
                Debug.Write("error");
            }
            Console.ReadKey();
        }

        private static ushort GetVersion()
        {
            return OSCImport.dsoGetFPGAVersion(0);
        }

        private static bool Connect()
        {
            var con = OSCImport.dsoHTDeviceConnect(0);
            if (con == 1) return true;
            else return false;
        }

        private static short[] SearchDevices()
        {
            short[] devices = new short[32];
            IntPtr pDevInfo = Marshal.AllocHGlobal(devices.Length * sizeof(short));

            Marshal.Copy(devices, 0, pDevInfo, devices.Length);
            ushort dev = OSCImport.dsoHTSearchDevice(pDevInfo);
            Marshal.Copy(pDevInfo, devices, 0, devices.Length);
            Marshal.FreeHGlobal(pDevInfo);
            return devices;
        }
    }
}
