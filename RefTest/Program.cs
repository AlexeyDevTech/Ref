

using RefTest.OSC;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace RefTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var inst = OSCControlFactory.Get(OSCControlType.Mock);
                var res = inst.Init();
                await Console.Out.WriteLineAsync(res.ToString());
                if (res)
                {

                    Console.WriteLine(inst.GetHTriggerLevel());
                    Stopwatch sw = new Stopwatch();
                    inst.Start();
                    sw.Start();
                    inst.DataReceived += data =>
                    {
                        Console.WriteLine($"Data received in {sw.Elapsed}");
                        if(sw.Elapsed > TimeSpan.FromSeconds(5))
                        {
                            inst.Stop();
                            Console.WriteLine("Task paused... Wait 3 sec");
                            Thread.Sleep(3000);
                            sw.Restart();
                            inst.Start();
                        }
                    };




                        //Stopwatch sw = new Stopwatch();
                        //await Task.Factory.StartNew(() =>
                        //{
                        //    sw.Start();
                        //    int conter = 100;
                        //    while (conter > 0)
                        //    {
                        //        OSCImport.dsoHTStartCollectData(0, 5);

                        //        while ((OSCImport.dsoHTGetState(0) & 0x02) == 0)
                        //        {
                        //            Task.Delay(10);
                        //        }
                        //        ushort[] ch1 = new ushort[65536];
                        //        ushort[] ch2 = new ushort[65536];
                        //        ushort[] ch3 = new ushort[65536];
                        //        ushort[] ch4 = new ushort[65536];
                        //        var res = OSCImport.dsoHTGetData(0, ch1, ch2, ch3, ch4, ref OSCControl.Instance.stControl);
                        //        conter--;
                        //        Console.WriteLine($"iterations lost: {conter}, elapsed: {sw.Elapsed}");
                        //    }
                        //    sw.Stop();
                        //    Console.WriteLine(sw.Elapsed);
                        //});
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

        
    }
}
