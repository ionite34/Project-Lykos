using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos
{
    public class ProcessControl
    {
        // Constructor
        public ProcessControl()
        {
        }
    }

    // Gets the maximum logical cores of the system
    public static class ProcessControlMaxCores
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        static extern bool GetNumaHighestNodeNumber(out int HighestNodeNumber);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        static extern bool GetNumaNodeProcessorMaskEx(int Node, out IntPtr ProcessorMask);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        static extern bool GetNumaNodeProcessorMaskEx(int Node, out long ProcessorMask);

        public static int GetMaxCores()
        {
            int HighestNodeNumber = 0;
            GetNumaHighestNodeNumber(out HighestNodeNumber);
            int MaxCores = 0;
            for (int i = 0; i <= HighestNodeNumber; i++)
            {
                IntPtr ProcessorMask = IntPtr.Zero;
                GetNumaNodeProcessorMaskEx(i, out ProcessorMask);
                long ProcessorMaskLong = ProcessorMask.ToInt64();
                int Count = 0;
                while (ProcessorMaskLong != 0)
                {
                    if ((ProcessorMaskLong & 1) == 1)
                    {
                        Count++;
                    }
                    ProcessorMaskLong >>= 1;
                }
                if (Count > MaxCores)
                {
                    MaxCores = Count;
                }
            }
            return MaxCores;
        }
    }
}
