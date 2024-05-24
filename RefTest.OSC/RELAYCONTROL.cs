using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RefTest.OSC
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RELAYCONTROL
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public bool[] bCHEnable;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public ushort[] nCHVoltDIV;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public ushort[] nCHCoupling;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public bool[] bCHBWLimit;

        public ushort nTrigSource;
        public bool bTrigFilt;
        public ushort nALT;
    }
}
