using System.Runtime.InteropServices;

namespace RefTest.OSC
{

    public class OSCImport
    {
        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTSearchDevice", CharSet = CharSet.Auto ,CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTSearchDevice(IntPtr pDevInfo);

        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTDeviceConnect", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTDeviceConnect(ushort nDeviceIndex);

        [DllImport("HTHardDll.dll", EntryPoint = "dsoGetFPGAVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoGetFPGAVersion(ushort DeviceIndex);

        //[DllImport("HTHardDll.dll" )]

        [DllImport("HTHardDll.dll", EntryPoint = "dsoHTReadCalibrationData", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort dsoHTReadCalibrationData(ushort nDeviceIndex, [In, Out] ushort[] pLevel, ushort nLen);




    }
}
