﻿using System.Runtime.InteropServices;
using RefTest.OSC.Interfaces;
using RefTest.OSC.Structs;

namespace RefTest.OSC
{
    public class OSCControl : IOSCControl
    {
        public event OSCDataReceivedEventHandler DataReceived;
        public event OSCConnectStateChangeEventHandler ConnectStateChange;
        CancellationTokenSource cts;
        CancellationToken ct;
        Task WorkerTask;
        ManualResetEvent WorkerWaiter = new ManualResetEvent(true);



        bool IsConnect = false;
        public bool SingleConnect { get; set; } = false; //возможность управлять потоком подключения, либо после
                                                         //запуска он останавливается, 
                                                         //либо после запуска и последующего отключения он будет 
                                                         //продолжать искать прибор


        private static OSCControl _instance;
        int failConnectCounter = 3;
        ushort deviceIndex = 32;                //значение по умолчанию (Init() == false если ничего не нашел)
        ushort ver = 0;
        ushort chMode = 1;
        ushort TriggerSens = 10;
        public CollectDataMode collectDataMode = CollectDataMode.Single;
        int errorCollectingDataCounter = 0;

        ushort[] CalLevels = new ushort[577];
        ushort[] ch1 = new ushort[65536];
        ushort[] ch2 = new ushort[65536];
        ushort[] ch3 = new ushort[65536];
        ushort[] ch4 = new ushort[65536];

        /*
        * ZERO_FLAG				       0xFBCF
        * 
        * 
        * 
        * CAL_LEVEL_LEN           ZEROCALI_LEN +1                         = 577 
        * ZEROCALI_LEN            (ZEROCALI_PER_CH_LEN*MAX_CH_NUM)        = 576
        * ZEROCALI_PER_CH_LEN     (ZEROCALI_PER_VOLT_LEN*MAX_VOLTDIV_NUM) = 144
        * MAX_VOLTDIV_NUM         12
        * ZEROCALI_PER_VOLT_LEN   12
        * MAX_CH_NUM              4
        */


        public PCONTROLDATA stControl = new()
        {
            nCHSet = 0x01,
            nTimeDIV = (ushort)TimeDiv.ns200,
            nTriggerSource = 0,
            nHTriggerPos = 1,
            nVTriggerPos = 190,
            nTriggerSlope = 0, //RISE
            nBufferLen = 0x10000, //BUF_64K_LEN
            nReadDataLen = 0x10000, //BUF_64K_LEN
            nAlreadyReadLen = 0,
            nALT = 0,
        };
               RELAYCONTROL relayControl = new()
               {
            bCHEnable = [true, false, false, false],
            nCHVoltDIV = [(ushort)VoltDiv.V4, (ushort)VoltDiv.V8, 0, 0],
            nCHCoupling = [0, 0, 0, 0],
            bCHBWLimit = [false, false, false, false],
            bTrigFilt = false,
            nTrigSource = 0,
            nALT = 0
        };
        private bool CanConnectWorker = false;

        public static OSCControl Instance {
            get
            {
                if(_instance == null)
                {
                    _instance = new OSCControl();
                }
                return _instance;
            }
        }
        private OSCControl()
        {
            
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
                                Console.WriteLine("Подключение установлено");
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

                   await Task.Delay(1000);
                }
        }

        private void OnConnectStateChange()
        {
            ConnectStateChange?.Invoke(IsConnect);
        }

        private void FailConnectRegister()
        {
            failConnectCounter--;
            if(failConnectCounter < 0)
            {
                deviceIndex = 32;
                failConnectCounter = 3;
            }
        }


        /// <summary>
        /// метод для непрерывного считывания данных из осциллографа
        /// </summary>
        /// <param name="token">токен отмены задачи</param>
        private async void Worker(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                int fail_counter = 50;
                WorkerWaiter.WaitOne();
                //await Console.Out.WriteAsync("prc1");
                if (OSCImport.dsoHTStartCollectData(deviceIndex, (ushort)collectDataMode) == 0)
                {
               //     await Console.Out.WriteAsync("-");
                    await Task.Delay(10);
                    continue;
                }
                //await Console.Out.WriteAsync("f+");
                //await Console.Out.WriteAsync("2");
                while ((OSCImport.dsoHTGetState(0) & 0x02) == 0)
                {
                  //  await Console.Out.WriteAsync(".");
                    if (token.IsCancellationRequested) break;
                    if (collectDataMode == CollectDataMode.Single)
                    {
                        fail_counter--;
                        if (fail_counter == 0) break;
                    }
                    await Task.Delay(10); //40
                }
                if (fail_counter == 0)
                {
                    errorCollectingDataCounter++;
                    continue;
                }
                //await Console.Out.WriteAsync("3");
                if (OSCImport.dsoHTGetData(deviceIndex, ch1, ch2, ch3, ch4, ref stControl) != 0)
                {
                    OnDataReceived();
                }
                await Task.Delay(10);
            }
        }

        public void Connect()
        {
            CanConnectWorker = true;
            Task.Run(ConnectWorker);
        }
        public void StopConnect() => CanConnectWorker = false;

        public async Task<bool> Init()
        {
            //смысл этого условия в том, что если после выполнения функции Connect()
            //сразу выполнить функцию Init(), то есть большая вероятность что функция
            //Init() вернет false
            if (!await Task.Run(() =>
                {
                    var fail_counter = 100;  //~5sec
                    while (!IsConnect && fail_counter > 0)
                    {
                        fail_counter--;
                        Task.Delay(50);
                    }
                    if (fail_counter == 0) return false;
                    return true;
                })) return false;  
            
            OSCImport.dsoSetUSBBus(deviceIndex);
            await Console.Out.WriteLineAsync("InitHard");
            if (!OSCImport.dsoInitHard(deviceIndex)) return false;
            await Console.Out.WriteLineAsync("dsoHTADCCHModGain");
            if (OSCImport.dsoHTADCCHModGain(deviceIndex, chMode) == 0) return false;
            await Console.Out.WriteLineAsync("ReadCalibrationData");
            if (!ReadCalibrationData()) return false;
            await Console.Out.WriteLineAsync("TryRecalib");
            if (!TryRecalib()) return false;         //вернет false только если в процессе перекалибровки была ошибка
            await Console.Out.WriteLineAsync("dsoHTSetSampleRate");
            if (OSCImport.dsoHTSetSampleRate(deviceIndex, (ushort)YTFormat.Normal, ref relayControl, ref stControl) == 0) return false;
            await Console.Out.WriteLineAsync("dsoHTSetCHAndTrigger");
            if (OSCImport.dsoHTSetCHAndTrigger(deviceIndex, ref relayControl, stControl.nTimeDIV) == 0) return false;
            await Console.Out.WriteLineAsync("dsoHTSetRamAndTrigerControl");
            if (OSCImport.dsoHTSetRamAndTrigerControl(deviceIndex, stControl.nTimeDIV, stControl.nCHSet, stControl.nTriggerSource, 1) == 0) return false;
            await Console.Out.WriteLineAsync("dsoHTSetCHPos");
            if (OSCImport.dsoHTSetCHPos(deviceIndex, relayControl.nCHVoltDIV[0], 128, 0, 1) == 0) return false;
            await Console.Out.WriteLineAsync("dsoHTSetCHPos2");
            if (OSCImport.dsoHTSetCHPos(deviceIndex, relayControl.nCHVoltDIV[1], 128, 0, 1) == 0) return false;
            await Console.Out.WriteLineAsync("dsoHTSetVTriggerLevel");
            if (OSCImport.dsoHTSetVTriggerLevel(deviceIndex, stControl.nVTriggerPos, TriggerSens) == 0) return false;
            await Console.Out.WriteLineAsync("dsoHTSetTrigerMode");
            if (OSCImport.dsoHTSetTrigerMode(deviceIndex, 0, stControl.nTriggerSlope, 0) == 0) return false;
            

            return true;
        }

        public bool SearchAndConnectCheck()
        {

            if (!SearchDeviceIndex(out deviceIndex)) return false;
            if (!ConnectDevice(deviceIndex)) return false;
            return true;
        }

        public bool SearchDeviceIndex(out ushort DeviceIndex)
        {
            DeviceIndex = 32;
            var devices = SearchDevices();
            //определяем индекс подключенного устройства
            for (int i = 0; i < devices.Length; i++)
            {
                if (devices[i] != 0)
                {
                    DeviceIndex = ushort.Parse(i.ToString());
                    break;
                }
            }
            if (DeviceIndex == 32) return false;
            return true;
        }

        public ushort GetVersion()
        {
            return OSCImport.dsoGetFPGAVersion(deviceIndex);
        }
        private bool ConnectDevice(ushort DeviceIndex)
        {
            var con = OSCImport.dsoHTDeviceConnect(DeviceIndex);
            if (con == 1) return true;
            else return false;
        }
        private short[] SearchDevices()
        {
            short[] devices = new short[32];
            IntPtr pDevInfo = Marshal.AllocHGlobal(devices.Length * sizeof(short));

            Marshal.Copy(devices, 0, pDevInfo, devices.Length);
            ushort dev = OSCImport.dsoHTSearchDevice(pDevInfo);
            Marshal.Copy(pDevInfo, devices, 0, devices.Length);
            Marshal.FreeHGlobal(pDevInfo);
            return devices;
        }
        private bool ReadCalibrationData() => OSCImport.dsoHTReadCalibrationData(deviceIndex, CalLevels, ushort.Parse(CalLevels.Length.ToString())) != 0;
        private bool TryRecalib()
        {
            var res = true;
            if (CalLevels[CalLevels.Length - 1] != 0xFBCF)
            {
                //re-calibrate
                for (int i = 0; i < 577; i++)
                {
                    int n_volt = (i % 144) / 12;
                    if (n_volt == 5 || n_volt == 8 || n_volt == 11)
                    {
                        switch ((i % 144) % 12)
                        {
                            case 0:
                                CalLevels[i] = 16602;
                                break;
                            case 1:
                                CalLevels[i] = 60111;
                                break;
                            case 2:
                                CalLevels[i] = 17528;
                                break;
                            case 3:
                                CalLevels[i] = 59201;
                                break;
                            case 4:
                                CalLevels[i] = 17710;
                                break;
                            case 5:
                                CalLevels[i] = 58900;
                                break;
                            default:
                                CalLevels[i] = 0;
                                break;


                        }
                    }
                }
                var l = ushort.Parse(CalLevels.Length.ToString());
                try
                {
                    if (OSCImport.dsoHTWriteCalibrationData(0, CalLevels, l) == 0) throw new ApplicationException("calibration data writing not success");
                }
                catch (ApplicationException) { res = false; }
            }
            return res;
        }

        public bool SetSampleRate(YTFormat format)
        {
            if (OSCImport.dsoHTSetSampleRate(deviceIndex, (ushort)format, ref relayControl, ref stControl) != 0) return false;
            if (OSCImport.dsoHTSetCHAndTrigger(0, ref relayControl, stControl.nTimeDIV) != 0) return false;
            return true;
        }
        public bool SetSampleRate(TimeDiv td, YTFormat format)
        {
            var utd = (ushort)td;
            if (stControl.nTimeDIV != utd) stControl.nTimeDIV = utd;
            return SetSampleRate(format);
        }
        public float GetSampleRate()
        {
            return OSCImport.dsoGetSampleRate(deviceIndex);
        }
        public TimeDiv GetTimeDiv()
        {
            var td = stControl.nTimeDIV;
            var res = Enum.Parse<TimeDiv>(td.ToString());
            return res;
        }
        public VoltDiv GetVoltDiv()
        {
            var vd = relayControl.nCHVoltDIV[0];
            var res = Enum.Parse<VoltDiv>(vd.ToString());
            return res;
        }
        public bool SetVoltDiv(VoltDiv vd)
        {
            relayControl.nCHVoltDIV[0] = (ushort)vd;
            if(OSCImport.dsoHTSetCHAndTrigger(deviceIndex, ref relayControl, (ushort)GetTimeDiv()) == 0) return false;
            return true;
        }
        public ushort GetVTriggerLevel() => stControl.nVTriggerPos;
        public ushort GetHTriggerLevel() => stControl.nHTriggerPos;
        public bool SetVTriggerLevel(byte level)
        {
            var tl = ushort.Parse(level.ToString());
            stControl.nVTriggerPos = tl;
            if (OSCImport.dsoHTSetVTriggerLevel(deviceIndex, tl, TriggerSens) == 0) return false;
            return true;
        }
        public bool SetHTriggerLevel(byte level)
        {
            if (level > 100) return false;
            var tl = ushort.Parse(level.ToString());
            stControl.nHTriggerPos = tl;
            if (OSCImport.dsoHTSetHTriggerLength(deviceIndex, ref stControl, chMode) == 0) return false;
            return true;
        }
        public bool SetTriggerLevel(byte hLevel, byte vLevel) => SetVTriggerLevel(vLevel) && SetHTriggerLevel(hLevel);

        public ushort[] GetData() => ch1;

        void OnDataReceived()
        {
            DataReceived?.Invoke(GetData());
        }

        //Task management
        public void Start() {
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

    public enum YTFormat : ushort
    {
        Normal = 0,
        Scan = 1,
        Roll = 2
    }
    public enum TimeDiv : ushort
    {
       ns200 = 6,   //1    GHz
       ns500 = 7,   //500  MHz
       us1   = 8,   //250  MHz
       us2   = 9,   //125  MHz
       us5   = 10,  //50   MHz
       us10  = 11,  //25   MHz
       us20  = 12,  //12.5 MHz
       us50  = 13,  //5    MHz
       us100 = 14,  //2.5  MHz
       us200 = 15,  //1.25 MHz
       us500 = 16,  //500  kHz
       ms1   = 17,  //250  kHz
       ms2   = 18,  //125  kHz
       ms5   = 19,  //50   kHz
       ms10  = 20,  //25   kHz
       ms20  = 21,  //12.5 kHz
       ms50  = 22,  //5    kHz
       ms100 = 23,  //2.5  kHz
       ms200 = 24,  //1.25 kHz

    }
    public enum VoltDiv : ushort
    {
        mV16 = 0,
        mV40 = 1,
        mV80 = 2,
        mV160 = 3,
        mV400 = 4,
        mV800 = 5,
        V1_6 = 6,
        V4 = 7,
        V8 = 8,
        V16 = 9,
        V40 = 10,
        V80 = 11
    }
    
    public enum CollectDataMode : ushort
    {
        Auto = 1,
        ROLL = 3,
        Wait = 4,
        Single = 5
    }

}
