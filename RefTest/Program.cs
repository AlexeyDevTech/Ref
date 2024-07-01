using RefTest.OSC;
using RefTest.OSC.Helpers;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;

namespace RefTest
{
    internal class Program
    {
        public static SerialPort Port;
        static async Task Main(string[] args)
        {
            try
            {
                #region OSC Control check
                //OSCControlFactory.SetInstance(OSCControlType.Mock);
                //OSCControlFactory.Instance.ConnectStateChange += async state => 
                //{
                //    if (state)
                //    {  
                //        var r = await OSCControlFactory.Instance.Init();  //это запускает OSC
                //        if (r)
                //        {
                //            OSCControlFactory.Instance.Stop();

                //            OSCControlFactory.Instance.Start();  //это запускает считывание данных
                //        }
                //    }
                //    else 
                //        OSCControlFactory.Instance.Stop();
                //};
                //OSCControlFactory.Instance.DataReceived += data =>
                //{
                //    Console.WriteLine($"//мы получили данные: {data.Length} элементов");
                //};

                //OSCControlFactory.Instance.Connect();
                #endregion

                var control = new ARMControl();
                
                
                var sw = new Stopwatch();
                sw.Start();
                var t2 = SerialPortFinder.FindDeviceAsync("R120#", "R120_OK", 115200);
                //var t1 = SerialPortFinder.FindDeviceAsync("#LAB?", "AngstremLabController", 9600);
                //var t3 = SerialPortFinder.FindDeviceAsync("#LAB?", "Power Selector", 115200);

                //await Task.WhenAll(t1, t2, t3);


                //if (await t1 != "Device not found.")
                //    await Console.Out.WriteLineAsync($"success MEA");
                //else await Console.Out.WriteLineAsync($"fail MEA");
                if (await t2 != "Device not found.")
                    await Console.Out.WriteLineAsync($"success Ref");
                else await Console.Out.WriteLineAsync($"fail Ref");
                //if (await t3 != "Device not found.")
                //    await Console.Out.WriteLineAsync($"success PS");
                //else await Console.Out.WriteLineAsync($"fail PS");
                //var f = await control.Connect();
                //if (f)
                //{
                //    await Console.Out.WriteLineAsync($"success: {control.Port.PortName}");

                //    await Task.Delay(2000);
                //    if (await control.SetChannel(1)) await Console.Out.WriteLineAsync("set 1 OK");
                //    //await Task.Delay(20);
                //    if (await control.SetChannel(2)) await Console.Out.WriteLineAsync("set 2 OK");
                //    //await Task.Delay(20);
                //    if (await control.SetChannel(3)) await Console.Out.WriteLineAsync("set 3 OK");
                //    //await Task.Delay(20);
                //    if (await control.SetChannel(4)) await Console.Out.WriteLineAsync("set 4 OK");
                //    //await Task.Delay(20);
                //    if (await control.SetChannel(5)) await Console.Out.WriteLineAsync("set 5 OK");
                //    //await Task.Delay(20);
                //    if (await control.SetChannel(6)) await Console.Out.WriteLineAsync("set 6 OK");
                //    //await Task.Delay(20);
                //    if (await control.SetChannel(5)) await Console.Out.WriteLineAsync("set 5 OK");
                //    //await Task.Delay(20);
                //    if (await control.SetChannel(4)) await Console.Out.WriteLineAsync("set 4 OK");
                //    //await Task.Delay(20);
                //    if (await control.SetChannel(3)) await Console.Out.WriteLineAsync("set 3 OK");
                //    //await Task.Delay(20);
                //    if (await control.SetChannel(2)) await Console.Out.WriteLineAsync("set 2 OK");
                //    //await Task.Delay(20);
                //    if (await control.SetChannel(1)) await Console.Out.WriteLineAsync("set 1 OK");
                //}
                await Console.Out.WriteLineAsync($"time: {sw.Elapsed}");
                sw.Stop();

            }
            catch (AccessViolationException)
            {
                Debug.Write("error");
            }
            //Console.ReadKey();
        }

        public static async Task<bool> Connect(string request, string responce, int baudrate = 9600)
        {
            //var finder = new SerialPortFinder();
            var portName = await SerialPortFinder.FindDeviceAsync(request, responce, baudrate);
            if (!portName.Contains("Device not found."))
            {
                try
                {
                    Port = new SerialPort(portName, baudrate);
                    Port.Open();
                    await Task.Delay(50);
                    if (Port.IsOpen)
                        return true;
                    else return false;

                }
                catch (Exception) { return false; }
            }
            else return false;
        }


    }
}
