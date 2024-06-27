using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RefTest.OSC.Helpers
{
    public class SerialPortFinder
    {
        private static int maxRetries = 3;
        private static int retryDelay = 200; // in milliseconds
        private static int readTimeout = 100; // in milliseconds
        private static int writeTimeout = 100; // in milliseconds



        public static async Task<string> FindDeviceAsync(string exceptedRequest, string expectedResponse, int baudRate = 9600)
        {
            string[] portNames = SerialPort.GetPortNames();
            var cts = new CancellationTokenSource();
            var tasks = portNames.Select(portName => Task.Run(() => TryFindDeviceOnPort(portName, exceptedRequest, expectedResponse, baudRate, cts.Token))).ToArray();
            string foundPort = null;
            try
            {
                var FT = await Task.WhenAny(tasks);
                foundPort = FT.Result;
                await Console.Out.WriteLineAsync($"found task:{FT.Status}");
                if (foundPort != null)
                {
                    cts.Cancel(); // Отмена остальных задач, если устройство найдено
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in task: {ex.Message}");
            }
            await Task.WhenAll(tasks); // Дождаться завершения всех задач, чтобы обработать отмененные задачи корректно
            await Console.Out.WriteLineAsync($"FP: {foundPort}");
            return foundPort ?? "Device not found.";
        }

        private static async Task<string> TryFindDeviceOnPort(string portName, string exceptedRequest, string expectedResponse, int baudRate, CancellationToken token)
        {
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine($"Task for port {portName} was cancelled.");
                    break;
                }
                try
                {
                    using (SerialPort port = new SerialPort(portName, baudRate))
                    {
                        port.ReadTimeout = readTimeout;
                        port.WriteTimeout = writeTimeout;

                        port.Open();
                        await Task.Delay(50); // delay for establishing connection

                        port.Write(exceptedRequest); // send command to check device
                        string response = await Task.Run(() => port.ReadExisting());

                        if (response.Contains(expectedResponse))
                        {
                            Console.WriteLine($"Device found on port {portName}");
                            port.Close();
                            return portName;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempt {attempt} on port {portName} failed: {ex.Message}");
                    if (attempt < maxRetries)
                    {
                        await Task.Delay(retryDelay);
                    }
                }
            }

            return null;
        }
    }
}
