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
                OSCControlFactory.Instance.ConnectStateChange += async state => 
                {
                    if (state)
                    {  
                        var r = await OSCControlFactory.Instance.Init();  //это запускает OSC
                        if (r)
                        {
                            OSCControlFactory.Instance.Stop();

                            OSCControlFactory.Instance.Start();  //это запускает считывание данных
                        }
                    }
                    else 
                        OSCControlFactory.Instance.Stop();
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
