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

        public event ControlConnectStateChangeEventHandler ConnectStateChange;

        public async Task<bool> GetState()
        {
            return await SetCommand("get_state", "State");
        }

        public async Task<bool> Connect()
        {
            var baudRate = 115200;
            var finder = new SerialPortFinder();
            var portName = await finder.FindDeviceAsync("R120#", "R120_OK", baudRate);
            if (!portName.Contains("Device not found."))
            {
                Port = new SerialPort(portName, baudRate);
                
                return true;
            }
            else return false;
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

        void IControl.Connect()
        {
            throw new NotImplementedException();
        }

        public void StopConnect()
        {
            throw new NotImplementedException();
        }

        public void PauseConnect()
        {
            throw new NotImplementedException();
        }

        public void ResumeConnect()
        {
            throw new NotImplementedException();
        }
    }
}
