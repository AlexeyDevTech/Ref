using System.Runtime.InteropServices;

namespace RefTest.OSC
{

    public class OSCImport
    {
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSearchDevice", CharSet = CharSet.Auto ,CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSearchDevice(IntPtr pDevInfo);

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
        public static extern ushort dsoHTSetCHAndTrigger(ushort nDeviceIndex, IntPtr pRelayControl, ushort nTimeDIV);
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetCHAndTriggerDirect", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetCHAndTriggerDirect(ushort nDeviceIndex, IntPtr pRelayControl, ushort uDirect, ushort nDriverCode);
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetCHAndTriggerVB", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetCHAndTriggerVB(ushort nDeviceIndex,ref ushort pCHEnable,ref ushort pCHVoltDIV,ref ushort pCHCoupling,ref ushort pCHBWLimit,ushort nTriggerSource,ushort nTriggerFilt,ushort nALT,ushort nTimeDIV );
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetTriggerAndSyncOutput", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetTriggerAndSyncOutput(ushort nDeviceIndex, IntPtr pControl, ushort nTriggerMode, ushort nTriggerSlope, ushort nPWCondition, uint nPW, ushort nVideoStandard, ushort nVedioSyncSelect, ushort nVideoHsyncNumOption, ushort nSync);
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetSampleRate", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSetSampleRate(ushort nDeviceIndex, ushort nYTFormat, ref RELAYCONTROL pRelayControl, ref PCONTROLDATA pControl);
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTInitSDRam", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTStartCollectData", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTStartTrigger", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTForceTrigger", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTGetState", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTGetPackState", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTGetSDRamInit", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTGetData", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTGetScanData", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTWriteCalibrationData", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoSDGetData", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoSDHTGetRollData", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoSDHTGetScanData", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTGetRollData", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTOpenRollMode", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTCloseRollMode", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetPeakDetect", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTClosePeakDetect", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTGetHardFC", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetHardFC", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTResetCnter", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTStartRoll", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetHardVersion", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoUSBModeSetIPAddr", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoUSBModeGetIPAddr", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoOpenLan", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoOpenWIFIPower", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoResetWIFI", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetUSBModulVersion", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoSetUSBBus", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoSetSPIBus", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoSetHardInfo", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetHardInfo", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoWriteFlash", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoReadFlash", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetDeviceName", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetDeviceSN", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetPCBVersion", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetDriverVersion", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetLANEnable", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoSetLANEnable", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoWriteIIC", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTWRAmpCali", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTRDAmpCali", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTWRADCCali", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTRDADCCali", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoInitHard", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTADCCHModGain", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetAmpCalibrate", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetRamAndTrigerControl", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetTrigerMode", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetVideoTriger", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetPulseTriger", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetInBufferWithoutOpen", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetInBuffer", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoSendOutBuffer", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetChmod", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetSampleRate", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetIIC", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetSPI", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetUart", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetLinCan", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetSeriseData", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSetSeriesTriggerCommon", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTGetCanDecode", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTGetLinDecode", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTGetUartDecode", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTGetSPIDecode", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTGetIICDecode", CallingConvention = CallingConvention.Cdecl)]




        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetFPGAVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoGetFPGAVersion(ushort DeviceIndex);

        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTReadCalibrationData", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTReadCalibrationData(ushort nDeviceIndex, [In, Out] ushort[] pLevel, ushort nLen);






    }
}
