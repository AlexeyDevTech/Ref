using RefTest.OSC.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RefTest.OSC
{
    public class ARMControl : IARMControl
    {
        SerialPort port = new SerialPort("COM8", 115200);

        public ARMControl()
        {
            port.Open();
        }

        public async Task<bool> GetState()
        {
            return await SetCommand("get_state", "State");
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
                port.Write(text);
                await Task.Delay(10);
                var msg = port.ReadExisting();
                if (string.IsNullOrEmpty(msg) || !msg.Contains(callback))
                {
                    fail_counter--;
                    if (fail_counter == 0) return false;
                }
                
            } catch(Exception) { return false; }
            return true;
        }
    }
}
