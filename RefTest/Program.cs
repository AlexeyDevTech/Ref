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
                OSCControlFactory.SetInstance(OSCControlType.Mock);
                OSCControlFactory.Instance.ConnectStateChange += state =>
                {
                    if (state)
                    {
                        Console.WriteLine("+++произошло событие: устройство подключено!");
                    }
                    else 
                    { 
                        Console.WriteLine("---произошло событие: соединение разорвано!");
                    }
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
