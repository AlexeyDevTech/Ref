

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
