using RefTest.OSC;
using System.Diagnostics;
using System.Text;

namespace RefTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                OSCControlFactory.SetInstance(OSCControlType.Original);
                OSCControlFactory.Instance.ConnectStateChange += async state => 
                {
                    if (state)
                    {
                        Console.WriteLine("+++произошло событие: устройство подключено!");
                        var r = await OSCControlFactory.Instance.Init();
                        if (r)
                        {
                            OSCControlFactory.Instance.Start();
                        }
                    }
                    else 
                    { 
                        Console.WriteLine("---произошло событие: соединение разорвано!");
                        OSCControlFactory.Instance.Stop();
                    }
                };
                OSCControlFactory.Instance.DataReceived += data =>
                {
                    Console.WriteLine($"//мы получили данные: {data.Length} элементов");
                };

                OSCControlFactory.Instance.Connect();
               
                
            }
            catch (AccessViolationException)
            {
                Debug.Write("error");
            }
            Console.ReadKey();
        }

        
    }
}
