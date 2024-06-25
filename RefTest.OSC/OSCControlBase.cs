using RefTest.OSC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefTest.OSC
{
    public abstract class OSCControlBase : IControl
    {
        ManualResetEvent ConnectWaiter = new ManualResetEvent(true);

        protected bool CanConnectWorker = false;
        protected int faultCounter = 0; //регистрирует количество неудачных попыток подключения 
        protected bool IsConnect = false;     
        public bool SingleConnect { get; set; }
        protected Action ConnectAction { get; set; }

        public event ControlConnectStateChangeEventHandler ConnectStateChange;
        //logic
        public virtual void Connect()
        {
            CanConnectWorker = true;
            Task.Run(ConnectWorker);
        }
        //events
        protected void OnConnectStateChange()
        {
            ConnectStateChange?.Invoke(IsConnect, faultCounter);
        }

        public virtual void StopConnect() => CanConnectWorker = false;
        public virtual void PauseConnect() => ConnectWaiter.Reset();
        public virtual void ResumeConnect() => ConnectWaiter.Set();

        private async void ConnectWorker()
        {
            while (CanConnectWorker)
            {
                ConnectWaiter.WaitOne();
                ConnectAction?.Invoke();
                await Task.Delay(1000);
            }
        }
    }
}
