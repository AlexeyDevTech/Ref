using System.Runtime.InteropServices;
using RefTest.OSC.Structs;

namespace RefTest.OSC
{

    public class OSCImport
    {
        [DllImport("HTHardDll.dll", EntryPoint = "dsoSetUSBBus", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool dsoSetUSBBus(ushort DeviceIndex);

        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSearchDevice", CharSet = CharSet.Auto ,CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSearchDevice(IntPtr pDevInfo);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoInitADCOnce", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoInitADCOnce(ushort DeviceIndex);

        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTDeviceConnect", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTDeviceConnect(ushort nDeviceIndex);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetCHPos", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetCHPos(ushort nDeviceIndex, ushort nVoltDIV, ushort nPos, ushort nCH, ushort nCHMode);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetCHDirectLeverPos", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetCHDirectLeverPos(ushort nDeviceIndex,ushort nPos,ushort nCH);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetVTriggerLevel", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetVTriggerLevel(ushort nDeviceIndex, ushort nPos, ushort nSensitivity);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetHTriggerLength", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetHTriggerLength(ushort nDeviceIndex, ref PCONTROLDATA pControl, ushort nCHMod);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetBufferSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetBufferSize(ushort nDeviceIndex, ushort nBufferSize);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetCHAndTrigger", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetCHAndTrigger(ushort nDeviceIndex, ref RELAYCONTROL pRelayControl, ushort nTimeDIV);

        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetRamAndTrigerControl", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetRamAndTrigerControl(ushort DeviceIndex, ushort nTimeDiv, ushort nCHset, ushort nTrigerSource, ushort nPeak);

        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetCHAndTriggerDirect", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetCHAndTriggerDirect(ushort nDeviceIndex, IntPtr pRelayControl, ushort uDirect, ushort nDriverCode);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetCHAndTriggerVB", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetCHAndTriggerVB(ushort nDeviceIndex,ref ushort pCHEnable,ref ushort pCHVoltDIV,ref ushort pCHCoupling,ref ushort pCHBWLimit,ushort nTriggerSource,ushort nTriggerFilt,ushort nALT,ushort nTimeDIV );


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetTriggerAndSyncOutput", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetTriggerAndSyncOutput(ushort nDeviceIndex, IntPtr pControl, ushort nTriggerMode, ushort nTriggerSlope, ushort nPWCondition, uint nPW, ushort nVideoStandard, ushort nVedioSyncSelect, ushort nVideoHsyncNumOption, ushort nSync);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetSampleRate", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetSampleRate(ushort nDeviceIndex, ushort nYTFormat, ref RELAYCONTROL pRelayControl, ref PCONTROLDATA pControl);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTStartCollectData", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTStartCollectData(ushort nDeviceIndex, ushort nStartControl);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTStartTrigger", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTStartTrigger(ushort nDeviceIndex);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTForceTrigger", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTForceTrigger(ushort nDeviceIndex);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTGetState", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTGetState(ushort nDeviceIndex);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTGetPackState", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTGetPackState(ushort nDeviceIndex);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTGetData", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTGetData(ushort nDeviceIndex,ushort[] pCH1Data,ushort[] pCH2Data,ushort[] pCH3Data,ushort[] pCH4Data,ref PCONTROLDATA pControl);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTWriteCalibrationData", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTWriteCalibrationData(ushort nDeviceIndex, ushort[] pLevel, ushort nLen);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTGetRollData", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTGetRollData(ushort nDeviceIndex, IntPtr pCH1Data, IntPtr pCH2Data, IntPtr pCH3Data, IntPtr pCH4Data, IntPtr pControl);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetHardVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoGetHardVersion(ushort DeviceIndex);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetDeviceName", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool dsoGetDeviceName(ushort DeviceIndex, [Out] byte[] pBuffer);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetDeviceSN", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool dsoGetDeviceSN(ushort DeviceIndex, [Out] byte[] pBuffer);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetPCBVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool dsoGetPCBVersion(ushort DeviceIndex, [Out] byte[] pBuffer);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetDriverVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoGetDriverVersion(ushort DeviceIndex, [Out] byte[] pBuffer);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoInitHard", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool dsoInitHard(ushort DeviceIndex);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTADCCHModGain", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTADCCHModGain(ushort DeviceIndex, ushort nCHMod);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetAmpCalibrate", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetAmpCalibrate(ushort nDeviceIndex,ushort nCHSet,ushort nTimeDIV,ref ushort nVoltDiv,ref ushort pCHPos);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetTrigerMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetTrigerMode(ushort m_nDeviceIndex, ushort nTriggerMode, ushort nTriggerSlop, ushort nTriggerCouple);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetSampleRate", CallingConvention = CallingConvention.Cdecl)]
        public static extern float dsoGetSampleRate(ushort DeviceIndex);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetFPGAVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoGetFPGAVersion(ushort DeviceIndex);


        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTReadCalibrationData", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTReadCalibrationData(ushort nDeviceIndex, [In, Out] ushort[] pLevel, ushort nLen);






    }
}
