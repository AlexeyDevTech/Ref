using System.Collections.Concurrent;
using System.IO.Ports;

namespace RefTest.OSC.Helpers
{
    public delegate void PortFindEventHandler(string device, string PortName);
    public delegate void PortFindCompletedEventHandler(int countFindedDevices, bool isError);
    public class SerialPortFinder
    {
        public static event PortFindEventHandler PortFind;
        public static event PortFindCompletedEventHandler PortFindCompleted;
        private static int maxRetries = 5;
        private static int retryDelay = 200; // in milliseconds
        private static int readTimeout = 100; // in milliseconds
        private static int writeTimeout = 100; // in milliseconds

        private static ConcurrentQueue<DeviceInfo> SearchDevices = new ConcurrentQueue<DeviceInfo>();

        public static bool SingleSearchExecuted { get; private set; }
        public static bool MultiSearchExecuted { get; private set; }

        public static async Task<string> FindDeviceAsync(string exceptedRequest, string expectedResponse, int baudRate = 9600)
        {
            if (!MultiSearchExecuted)
            {
                SingleSearchExecuted = true;
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
                finally
                {
                    SingleSearchExecuted = false;
                }
                await Task.WhenAll(tasks); // Дождаться завершения всех задач, чтобы обработать отмененные задачи корректно
                SingleSearchExecuted = false;
                return foundPort ?? "Device not found.";
            } else throw new InvalidOperationException("Данный метод нельзя выполнить сейчас. Выполняется множественный поиск устройств.");
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

        private static async Task<DeviceInfo> TrySearchDevicesOnPort(string portName, CancellationToken token)
        {
            //await Console.Out.WriteLineAsync($"start process to {portName}");
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
                        //await Console.Out.WriteLineAsync($"{portName} -> port created");
                        foreach (var dev in SearchDevices) //для всех ищущихся устройств
                        {
                            if (string.IsNullOrEmpty(dev.PortName)) 
                            {
                                port.ReadTimeout = readTimeout;
                                port.WriteTimeout = writeTimeout;
                                port.BaudRate = dev.BaudRate;
                                if (!port.IsOpen)
                                    port.Open();
                               // await Console.Out.WriteLineAsync($"{portName} -> +port opened");
                                await Task.Delay(50); // delay for establishing connection

                                port.Write(dev.Request); // send command to check device
                                //await Console.Out.WriteLineAsync($"{portName} -> ++port send request");
                                string response = await Task.Run(() => port.ReadExisting());
                                //await Console.Out.WriteLineAsync($"{portName} -> +++port get response");
                                if (response.Contains(dev.Response))
                                {
                                    dev.PortName = portName;
                                    Console.WriteLine($"Device {dev.Name} found on port {portName}");
                                    PortFind?.Invoke(dev.Name, dev.PortName);
                                    return dev;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!]{portName} -> Attempt {attempt} on port {portName} failed: {ex.Message}");
                    if (attempt < maxRetries)
                    {
                        await Task.Delay(retryDelay);
                    }
                }
            }

            return null;
        }

        public static void AddDeviceToSearch(string Name, string Request, string Response, int baudRate = 9600)
        {
            SearchDevices.Enqueue(new DeviceInfo { Name = Name, Request = Request, Response = Response, BaudRate = baudRate });
        }
        public static async Task<DeviceInfo[]> StartSearch()
        {
            if (!SingleSearchExecuted)
            {
                string[] ports = SerialPort.GetPortNames();
                var cts = new CancellationTokenSource();
                var tks = ports.Select(pn => Task.Run(() => TrySearchDevicesOnPort(pn, cts.Token)));
                var cdi = await Task.WhenAll(tks);
                var cdf = cdi.Where(x => x != null).ToArray();
                if (cdf.Length > 0)
                {
                    PortFindCompleted?.Invoke(cdf.Length, false);
                    return cdf;
                }
                else
                {
                    PortFindCompleted?.Invoke(cdf?.Length ?? 0, true);
                    return null;
                }
            }
            else throw new InvalidOperationException("Данный метод нельзя выполнить сейчас. Выполняется единичный поиск устройства.");

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