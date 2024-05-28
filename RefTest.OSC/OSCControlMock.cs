using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RefTest.OSC
{
    public class OSCControlMock : IOSCControl
    {
        public event OSCDataReceivedEventHandler DataReceived;

        CancellationTokenSource cts;
        CancellationToken ct;
        Task WorkerTask;
        ManualResetEvent WorkerWaiter = new ManualResetEvent(true);

        private ushort[] ch1 = new ushort[65536];
        private static OSCControlMock _instance;

        public static OSCControlMock Instance
        {
            get
            {
                if (_instance == null) _instance =  new OSCControlMock();
                return _instance;
            }
        }
        private OSCControlMock()
        {

        }

        public bool Init()
        {
            // Эмуляция инициализации
            return true;
        }
        private bool Connect()
        {
            return true;
        }
        private short[] SearchDevices()
        {
            short[] devices = new short[32];
            devices[0] = 1;
            return devices;
        }

        public float GetSampleRate()
        {
            // Возвращаем фиктивное значение
            return 1000.0f;
        }
      
        public bool SetVoltDiv(VoltDiv vd)
        {
            // Эмуляция установки делителя напряжения
            return true;
        }

        public ushort GetVTriggerLevel()
        {
            // Возвращаем фиктивное значение
            return 190;
        }
        public ushort GetHTriggerLevel() => 1;

        public bool SetVTriggerLevel(byte level) => true;
        public bool SetHTriggerLevel(byte level) => true;
        public bool SetTriggerLevel(byte hLevel, byte vLevel) => true;


        public ushort[] GetData()
        {
            // Возвращаем фиктивные данные
            return ch1;
        }

      

        public void Start()
        {
            // Эмуляция запуска
            cts = new CancellationTokenSource();
            ct = cts.Token;
            WorkerTask = new Task(() => SimulateDataCollection(ct), ct, TaskCreationOptions.LongRunning);
            WorkerTask.Start();
        }
        public void Pause() => WorkerWaiter.Reset();
        public void Play() => WorkerWaiter.Set();
        public void Stop() => cts.Cancel();
        private void SimulateDataCollection(CancellationToken token)
        {
            // Эмуляция процесса сбора данных
            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(100);
                OnDataReceived();
            }
        }

        private void OnDataReceived()
        {
            // Вызов события DataReceived с фиктивными данными
            DataReceived?.Invoke(GetData());
        }

        public TimeDiv GetTimeDiv() => TimeDiv.ns200;

        public VoltDiv GetVoltDiv() => VoltDiv.V8;

        public bool SetSampleRate(YTFormat format) => true;

        public bool SetSampleRate(TimeDiv timeDiv, YTFormat format) => true;
    }
}
