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
