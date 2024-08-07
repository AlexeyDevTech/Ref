﻿using RefTest.OSC;
using RefTest.OSC.Helpers;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;

namespace RefTest
{
    internal class Program
    {
        private static bool old_connectState;
        public static ARMControl control = new ARMControl();
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

                
                control.ConnectStateChange += Control_ConnectStateChange;
                control.Connect();

                var sw = new Stopwatch();
                sw.Start();

                //SerialPortFinder.AddDeviceToSearch("MEA", "#LAB?", "AngstremLabController");
                //SerialPortFinder.AddDeviceToSearch("PS", "#LAB?", "Power Selector");
                //SerialPortFinder.AddDeviceToSearch("Ref", "R120#", "R120_OK", 115200);
                //SerialPortFinder.PortFind += SerialPortFinder_PortFind;
                //SerialPortFinder.PortFindCompleted += SerialPortFinder_PortFindCompleted;

                //var devs = await SerialPortFinder.StartSearch();
                //if (devs != null)
                //{
                //    await Console.Out.WriteLineAsync($"result:\n\t");
                //    foreach (var dev in devs)
                //    {
                //        await Console.Out.WriteAsync($"device: {dev.Name}\n\t");
                //        await Console.Out.WriteAsync($"PortName: {dev.PortName}\n\t");
                //        await Console.Out.WriteAsync($"BaudRate: {dev.BaudRate}\n\t");
                //        await Console.Out.WriteLineAsync();

                //    }
                //}
                //else await Console.Out.WriteLineAsync("devices not found");

                //var t2 = SerialPortFinder.FindDeviceAsync("R120#", "R120_OK", 115200);
                ////var t1 = SerialPortFinder.FindDeviceAsync("#LAB?", "AngstremLabController", 9600);
                ////var t3 = SerialPortFinder.FindDeviceAsync("#LAB?", "Power Selector", 115200);

                ////await Task.WhenAll(t1, t2, t3);


                ////if (await t1 != "Device not found.")
                ////    await Console.Out.WriteLineAsync($"success MEA");
                ////else await Console.Out.WriteLineAsync($"fail MEA");
                //if (await t2 != "Device not found.")
                //    await Console.Out.WriteLineAsync($"success Ref");
                //else await Console.Out.WriteLineAsync($"fail Ref");
                ////if (await t3 != "Device not found.")
                ////    await Console.Out.WriteLineAsync($"success PS");
                ////else await Console.Out.WriteLineAsync($"fail PS");
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



                Console.ReadKey();
                await Console.Out.WriteLineAsync($"time: {sw.Elapsed}");

                sw.Stop();

            }
            catch (AccessViolationException)
            {
                Debug.Write("error");
            }
            //Console.ReadKey();
        }

        private static async void Control_ConnectStateChange(bool state, int faultCounter)
        {
            if (state)
            {
                await Console.Out.WriteLineAsync($"ARM Connected!");
            } else
            {
                if (old_connectState) control.Connect();
            }
            old_connectState = state;


        }

        private static async void SerialPortFinder_PortFindCompleted(int countFindedDevices, bool isError)
        {
            await Console.Out.WriteLineAsync($"port search is completed. devices founded: {countFindedDevices} Error = {isError}");
        }

        private static async void SerialPortFinder_PortFind(string device, string PortName)
        {
            await Console.Out.WriteLineAsync($"++++Device {device} has been found in {PortName}.+++++");
        }

        //public static async Task<bool> Connect(string request, string responce, int baudrate = 9600)
        //{
        //    //var finder = new SerialPortFinder();
        //    var portName = await SerialPortFinder.FindDeviceAsync(request, responce, baudrate);
        //    if (!portName.Contains("Device not found."))
        //    {
        //        try
        //        {
        //            Port = new SerialPort(portName, baudrate);
        //            Port.Open();
        //            await Task.Delay(50);
        //            if (Port.IsOpen)
        //                return true;
        //            else return false;

        //        }
        //        catch (Exception) { return false; }
        //    }
        //    else return false;
        //}


    }
}
