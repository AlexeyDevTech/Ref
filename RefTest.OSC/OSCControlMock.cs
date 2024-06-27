using RefTest.OSC.Interfaces;
using RefTest.OSC.Structs;
using System.Runtime.InteropServices;

namespace RefTest.OSC
{
    public class OSCControlMock : IOSCControl
    {
        public event OSCDataReceivedEventHandler DataReceived;
        public event ControlConnectStateChangeEventHandler ConnectStateChange;

        CancellationTokenSource cts;
        CancellationToken ct;
        Task WorkerTask;
        ManualResetEvent WorkerWaiter = new ManualResetEvent(true);


        bool SimConnectState = false;
        bool IsConnect = false;
        public bool SingleConnect { get; set; } = false; //возможность управлять потоком подключения, либо после
                                                         //запуска он останавливается, 
                                                         //либо после запуска и последующего отключения он будет 
                                                         //продолжать искать прибор
        public bool AutoInit { get; set; } = false;
        private static OSCControlMock _instance;
        ushort deviceIndex = 32;                //значение по умолчанию (Init() == false если ничего не нашел)
        public CollectDataMode collectDataMode = CollectDataMode.Single;


        ushort vTriggerPos = 190;
        ushort hTriggerPos = 1;

        ushort[] ch1 = new ushort[65536];


        private bool CanConnectWorker = false;

        public static OSCControlMock Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OSCControlMock();
                }
                return _instance;
            }
        }

        public OSCStates State { get; private set; }

        private OSCControlMock()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(2000);
                    SimConnectState = true;
                    await Task.Delay(3000);
                    SimConnectState = false;
                }
            });
        }

        private async void ConnectWorker()
        {

            while (CanConnectWorker)
            {
                if (!IsConnect) //Если не подключено
                {
                    if (SearchDeviceIndex(out deviceIndex)) //поиск устройства 
                        if (ConnectDevice(deviceIndex))
                        {
                            //Console.WriteLine("Подключение установлено");
                            IsConnect = true;
                            OnConnectStateChange();
                        }
                }
                else
                {
                    //некая полезная нагрузка 

                    var is_con = ConnectDevice(deviceIndex);
                    if (!is_con)
                    {
                        //Console.WriteLine("Подключение разорвано");
                        IsConnect = false;
                        OnConnectStateChange();
                        if (SingleConnect)
                        {
                            StopConnect();
                            break;
                        }
                    }
                }

                await Task.Delay(1000);
            }
        }

        private void OnConnectStateChange()
        {
            ConnectStateChange?.Invoke(IsConnect, 0);
        }


        /// <summary>
        /// метод для непрерывного считывания данных из осциллографа
        /// </summary>
        /// <param name="token">токен отмены задачи</param>
        private void Worker(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                OnDataReceived();
                Thread.Sleep(1000);
            }
        }

        public void Connect()
        {
            CanConnectWorker = true;
            Task.Run(ConnectWorker);
        }
        public void StopConnect() => CanConnectWorker = false;
        public void PauseConnect()
        {
            return;
        }
        public void ResumeConnect()
        {
            return;
        }
        public async Task<bool> Init()
        {
            return true;
        }
        public bool SearchAndConnectCheck() => true;

        public bool SearchDeviceIndex(out ushort DeviceIndex)
        {
            DeviceIndex = 0;
            return true;
        }
        public ushort GetVersion() => ushort.Parse("61445");
        private bool ConnectDevice(ushort DeviceIndex) => SimConnectState;
        private short[] SearchDevices()
        {
            short[] devices = new short[32];
            devices[0] = 1;
            return devices;
        }
        private bool ReadCalibrationData() => true;
        private bool TryRecalib() => true;

        public async Task<bool> SetSampleRate(YTFormat format) => await Task.Run(() => true);
        public async Task<bool> SetSampleRate(TimeDiv td, YTFormat format) => await Task.Run(() => true);
        public async Task<bool> SetSampleRate(TimeDiv td) => await Task.Run(() => true);
        public float GetSampleRate() => 1000.0f;
        public TimeDiv GetTimeDiv() => TimeDiv.ns200;
        public VoltDiv GetVoltDiv() => VoltDiv.V8;
        public async Task<bool> SetVoltDiv(VoltDiv vd) => await Task.Run(() => true);
        public ushort GetVTriggerLevel() => vTriggerPos;
        public ushort GetHTriggerLevel() => hTriggerPos;
        public async Task<bool> SetVTriggerLevel(byte level)
        {
            return await Task.Run(() =>
            {
                var tl = ushort.Parse(level.ToString());
                vTriggerPos = tl;
                return true;
            });
        }
        public async Task<bool> SetHTriggerLevel(byte level)
        {
            return await Task.Run(() =>
            {
                var tl = ushort.Parse(level.ToString());
                hTriggerPos = tl;
                return true;
            });

        }
        public async Task<bool> SetTriggerLevel(byte hLevel, byte vLevel) => await SetVTriggerLevel(vLevel) && await SetHTriggerLevel(hLevel);
        public ushort[] GetData() => ch1;
        void OnDataReceived()
        {
            DataReceived?.Invoke(GetData());
        }

        //Task management
        public void Start()
        {
            cts = new CancellationTokenSource();
            ct = cts.Token;
            WorkerTask = new Task(() => Worker(ct), ct, TaskCreationOptions.LongRunning);
            WorkerTask.Start();
        }
        public void Pause() => WorkerWaiter.Reset();
        public void Play() => WorkerWaiter.Set();
        public void Stop()
        {
            cts.Cancel();
        }
    }
}
