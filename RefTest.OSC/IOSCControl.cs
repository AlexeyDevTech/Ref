using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefTest.OSC
{
    public delegate void OSCDataReceivedEventHandler(ushort[] data);
    public delegate void OSCConnectStateChangeEventHandler(bool connectState);
    public interface IOSCControl
    {
        event OSCDataReceivedEventHandler DataReceived;

        bool SingleConnect { get; set; }

        void Connect();
        void StopConnect();
        ushort GetVersion();
        Task<bool> Init();
        float GetSampleRate();
        bool SetVoltDiv(VoltDiv vd);
        TimeDiv GetTimeDiv();
        VoltDiv GetVoltDiv();

        ushort GetVTriggerLevel();
        ushort GetHTriggerLevel();
        bool SetVTriggerLevel(byte level);
        bool SetHTriggerLevel(byte level);
        bool SetTriggerLevel(byte hLevel, byte vLevel);
        bool SetSampleRate(YTFormat format);
        bool SetSampleRate(TimeDiv timeDiv, YTFormat format);
        ushort[] GetData();
        void Start();
        void Pause();
        void Play();
        void Stop();
    }
}
