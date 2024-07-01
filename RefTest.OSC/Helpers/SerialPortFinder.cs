using System.Collections.Concurrent;
using System.IO.Ports;

namespace RefTest.OSC.Helpers
{
    public delegate void PortFindEventHandler(string device, string PortName, bool isEnd);
    public class SerialPortFinder
    {
        public event PortFindEventHandler PortFind;
        private static int maxRetries = 5;
        private static int retryDelay = 200; // in milliseconds
        private static int readTimeout = 100; // in milliseconds
        private static int writeTimeout = 100; // in milliseconds

        private static ConcurrentQueue<DeviceInfo> SearchDevices = new ConcurrentQueue<DeviceInfo>();

        public static async Task<string> FindDeviceAsync(string exceptedRequest, string expectedResponse, int baudRate = 9600)
        {
            string[] portNames = SerialPort.GetPortNames();
            var cts = new CancellationTokenSource();
            var tasks = portNames.Select(portName => Task.Run(() => TryFindDeviceOnPort(portName, exceptedRequest, expectedResponse, baudRate, cts.Token))).ToArray();
            string foundPort = null;
            try
            {
                foundPort = (await Task.WhenAny(tasks)).Result;
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

                        port.WriteLine(exceptedRequest); // send command to check device
                        string response = await Task.Run(() => port.ReadExisting());

                        if (response.Contains(expectedResponse))
                        {
                            Console.WriteLine($"Device found on port {portName}");
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
                finally
                {
                    
                }
            }

            return null;
        }

        private static async Task<string> TryFindDevicesOnPort(string portName, CancellationToken token)
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
                    using (SerialPort port = new SerialPort(portName))
                    {
                        foreach (var dev in SearchDevices)
                        {
                            port.ReadTimeout = readTimeout;
                            port.WriteTimeout = writeTimeout;
                            port.BaudRate = dev.BaudRate;
                            port.Open();
                            await Task.Delay(50); // delay for establishing connection

                            port.WriteLine(dev.Request); // send command to check device
                            string response = await Task.Run(() => port.ReadExisting());

                            if (response.Contains(dev.Response))
                            {
                                Console.WriteLine($"Device found on port {portName}");
                                return portName;
                            }
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
                finally
                {

                }
            }

            return null;
        }


        public static void AddDeviceToSearch(string Name, string Request, string Response)
        {
            SearchDevices.Enqueue(new DeviceInfo { Name = Name, Request = Request, Response = Response });
        }
        public static async Task StartSearch()
        {

        }

    }

    public class DeviceInfo
    {
        public string Name { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public int BaudRate { get; set; }
        public string PortName { get; set; }

    }
}