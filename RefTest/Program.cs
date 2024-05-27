

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
                    // Console.ReadKey();

                    var f = OSCImport.dsoSetUSBBus(0);
                    if (OSCImport.dsoInitHard(0))
                    {
                        if (OSCImport.dsoHTADCCHModGain(0, 1) != 0)
                        {
                            /*
                             * ZERO_FLAG				       0xFBCF
                             * 
                             * 
                             * 
                             * CAL_LEVEL_LEN           ZEROCALI_LEN +1                         = 577 
                             * ZEROCALI_LEN            (ZEROCALI_PER_CH_LEN*MAX_CH_NUM)        = 576
                             * ZEROCALI_PER_CH_LEN     (ZEROCALI_PER_VOLT_LEN*MAX_VOLTDIV_NUM) = 144
                             * MAX_VOLTDIV_NUM         12
                             * ZEROCALI_PER_VOLT_LEN   12
                             * MAX_CH_NUM              4
                             */
                            ushort[] m_CalLevel = new ushort[577];
                            OSCImport.dsoHTReadCalibrationData(0, m_CalLevel, 577);


                            if (m_CalLevel[m_CalLevel.Length - 1] != 0xFBCF)
                            {
                                //re-calibrate
                                for (int i = 0; i < 577; i++)
                                {
                                    int n_volt = (i % 144) / 12;
                                    if (n_volt == 5 || n_volt == 8 || n_volt == 11)
                                    {
                                        switch ((i % 144) % 12)
                                        {
                                            case 0:
                                                m_CalLevel[i] = 16602;
                                                break;
                                            case 1:
                                                m_CalLevel[i] = 60111;
                                                break;
                                            case 2:
                                                m_CalLevel[i] = 17528;
                                                break;
                                            case 3:
                                                m_CalLevel[i] = 59201;
                                                break;
                                            case 4:
                                                m_CalLevel[i] = 17710;
                                                break;
                                            case 5:
                                                m_CalLevel[i] = 58900;
                                                break;
                                            default:
                                                m_CalLevel[i] = 0;
                                                break;


                                        }
                                    }
                                }
                            }

                        }

                    }



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
