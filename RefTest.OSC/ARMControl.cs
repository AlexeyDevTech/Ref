using RefTest.OSC.Helpers;
using RefTest.OSC.Interfaces;
using System.IO.Ports;

namespace RefTest.OSC
{
    public class ARMControl : IARMControl
    {
        public SerialPort Port { get; set; }
        public bool SingleConnect { get; set; }
        public bool AutoInit { get; set; }

        private bool CanConnectWorker = false;

        public event ControlConnectStateChangeEventHandler ConnectStateChange;
        ManualResetEvent ConnectWaiter = new ManualResetEvent(true);
        private bool IsConnect = false;
        private int faultCounter;

        private async void ConnectWorker()
        {
            while (CanConnectWorker)
            {
                ConnectWaiter.WaitOne();
                if (!IsConnect) //Если не подключено
                {
                    if (await ConnectPort()) //поиск устройства 
                    {
                        Console.WriteLine("Подключение установлено");
                        IsConnect = true;
                        faultCounter = 0;
                        OnConnectStateChange();
                    }
                    else    //если не нашел
                    {
                        faultCounter++;
                        OnConnectStateChange();
                    }
                }
                else
                {
                    //некая полезная нагрузка 

                    var is_con = PortIsExist();
                    if (!is_con)
                    {
                        Console.WriteLine("Подключение разорвано");
                        IsConnect = false;
                        OnConnectStateChange();
                        if (SingleConnect)
                        {
                            StopConnect();
                            break;
                        }
                    }
                }
                await Task.Delay(2000);
            }
        }

        private void OnConnectStateChange()
        {
            ConnectStateChange?.Invoke(IsConnect, faultCounter);
        }

        public void Connect()
        {
            CanConnectWorker = true;
            Task.Run(ConnectWorker);
        }
        public void StopConnect()
        {
            CanConnectWorker = false;
        }
        public void PauseConnect()
        {
            ConnectWaiter.Set();
        }
        public void ResumeConnect()
        {
            ConnectWaiter.Reset();
        }

        public async Task<bool> GetState()
        {
            return await SetCommand("get_state", "State");
        }

        public async Task<bool> ConnectPort()
        {
            var baudRate = 115200;
            //var finder = new SerialPortFinder();
            var portName = await SerialPortFinder.FindDeviceAsync("R120#", "R120_OK", baudRate);
            if (!portName.Contains("Device not found."))
            {
                try
                {
                    Port = new SerialPort(portName, baudRate);
                    Port.Open();
                    await Task.Delay(50);
                    return Port.IsOpen ? true : false;
                        

                }
                catch (Exception) { return false; }
            }
            else return false;
        }
        public async Task<bool> PortIsExist()
        {
            var res = false;
            //var ports = SerialPort.GetPortNames();
            //return ports.FirstOrDefault(x => x == Port.PortName) != null;
            try
            {
                int fcounter = 3;
                if (Port.IsOpen)
                {
                    Port.Write("?000#");
                    await Task.Delay(20);
                    var msg = Port.ReadExisting();
                    if (!string.IsNullOrEmpty(msg)) res = true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return res;
        }

        public async Task<bool> SetAmplitude(int vol) => await SetCommand($"V{vol:D3}#", $"V{vol:D3}_OK");

        public async Task<bool> SetChannel(int channel) => await SetCommand($"C{channel:D3}#", $"C{channel:D3}_OK");

        public async Task<bool> SetImpulse(int impulse)
        {
            var _impulse = (impulse - 14) / 14 < 1 ? 1 : (impulse - 14) / 14;

            return await SetCommand($"I{_impulse:D3}#", $"I{_impulse:D3}_OK");
        }

        public async Task<bool> SetMode(int mode) => await SetCommand($"M{mode:D3}#", $"M{mode:D3}_OK");

        public async Task<bool> SetResistance(int resistance) => await SetCommand($"R{resistance:D3}#", $"R{resistance:D3}_OK");

        private async Task<bool> SetCommand(string text, string callback)
        {
            try
            {
                var fail_counter = 3;
                Port.Write(text);
                await Task.Delay(10);
                var msg = Port.ReadExisting();
                if (string.IsNullOrEmpty(msg) || !msg.Contains(callback))
                {
                    fail_counter--;
                    if (fail_counter == 0) return false;
                }

            }
            catch (Exception) { return false; }
            return true;
        }

        
    }
}
