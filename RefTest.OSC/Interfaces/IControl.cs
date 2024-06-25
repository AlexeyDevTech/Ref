using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefTest.OSC.Interfaces
{
    public delegate void ControlConnectStateChangeEventHandler(bool connectState, int faultCounter);
    public interface IControl 
    {
        event ControlConnectStateChangeEventHandler ConnectStateChange;
        bool SingleConnect { get; set; }
        void Connect();
        void StopConnect();
        void PauseConnect();
        void ResumeConnect();
    }
}
