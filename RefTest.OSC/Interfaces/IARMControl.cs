using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefTest.OSC.Interfaces
{
    public interface IARMControl
    {
        event ControlConnectStateChangeEventHandler ConnectStateChange;
        Task<bool> GetState();
        Task<bool> SetAmplitude(int vol);
        Task<bool> SetChannel(int channel);
        Task<bool> SetImpulse(int impulse);
        Task<bool> SetMode(int mode);
        Task<bool> SetResistance(int resistance);
        void Connect();
        void StopConnect();
        void PauseConnect();
        void ResumeConnect();

    }
}
