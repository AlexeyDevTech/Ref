using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RefTest.OSC.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PCONTROLDATA
    {
        public ushort nCHSet;             // WORD -> ushort
        public ushort nTimeDIV;           // WORD -> ushort
        public ushort nTriggerSource;     // WORD -> ushort
        public ushort nHTriggerPos;       // WORD -> ushort
        public ushort nVTriggerPos;       // WORD -> ushort
        public ushort nTriggerSlope;      // WORD -> ushort
        public uint nBufferLen;           // ULONG -> uint
        public uint nReadDataLen;         // ULONG -> uint
        public uint nAlreadyReadLen;      // ULONG -> uint
        public ushort nALT;               // WORD -> ushort
        public ushort nETSOpen;           // WORD -> ushort
    }
}
