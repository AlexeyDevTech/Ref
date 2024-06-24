using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefTest.OSC.Interfaces
{
    public delegate void OSCDataReceivedEventHandler(ushort[] data);
    
    public interface IOSCControl : IControl
    {
        event OSCDataReceivedEventHandler DataReceived;
        

       
        OSCStates State { get; }

     
        ushort GetVersion();
        Task<bool> Init();
        float GetSampleRate();
        Task<bool> SetVoltDiv(VoltDiv vd);
        TimeDiv GetTimeDiv();
        VoltDiv GetVoltDiv();

        ushort GetVTriggerLevel();
        ushort GetHTriggerLevel();
        Task<bool> SetVTriggerLevel(byte level);
        Task<bool> SetHTriggerLevel(byte level);
        Task<bool> SetTriggerLevel(byte hLevel, byte vLevel);
        Task<bool> SetSampleRate(YTFormat format);
        Task<bool> SetSampleRate(TimeDiv timeDiv, YTFormat format);
        Task<bool> SetSampleRate(TimeDiv timeDiv);
        ushort[] GetData();
        void Start();
        void Pause();
        void Play();
        void Stop();
    }
}
