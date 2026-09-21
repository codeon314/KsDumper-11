using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace KsDumper11.Driver
{
    // Matches KERNEL_DRIVER_INFO from UserModeBridge.h.
    //   typedef struct _KERNEL_DRIVER_INFO {
    //       PVOID BaseAddress;          // 8 bytes on x64
    //       ULONG SizeOfImage;          // 4 bytes
    //       WCHAR FullPathName[256];    // 512 bytes
    //   } KERNEL_DRIVER_INFO;
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct KERNEL_DRIVER_INFO
    {
        public ulong BaseAddress;
        public uint SizeOfImage;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string FullPathName;
    }

    // Matches KERNEL_GET_DRIVERS_OPERATION from UserModeBridge.h.
    [StructLayout(LayoutKind.Sequential)]
    internal struct KERNEL_GET_DRIVERS_OPERATION
    {
        public IntPtr bufferAddress;
        public int bufferSize;
        public int moduleCount;
    }

    // Matches KERNEL_DUMP_DRIVER_OPERATION from UserModeBridge.h.
    //   typedef struct _KERNEL_DUMP_DRIVER_OPERATION {
    //       PVOID baseAddress;   // Input:  kernel VA of the driver image to dump
    //       PVOID bufferAddress; // Input:  user-mode destination buffer
    //       INT32 bufferSize;    // Input:  size of destination buffer in bytes
    //       INT32 bytesRead;     // Output: bytes actually copied
    //   } KERNEL_DUMP_DRIVER_OPERATION;
    [StructLayout(LayoutKind.Sequential)]
    internal struct KERNEL_DUMP_DRIVER_OPERATION
    {
        public IntPtr baseAddress;
        public IntPtr bufferAddress;
        public int bufferSize;
        public int bytesRead;
    }

    /// <summary>
    /// Self-contained user-mode wrapper for the kernel-driver IOCTLs.
    /// Opens its own handle to \\.\KsDumper via CreateFile; does NOT depend on
    /// KsDumperDriverInterface, so no partial-class extension is required.
    /// </summary>
    public sealed class KernelDriverOperations : IDisposable
    {
        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint FILE_SHARE_READ = 0x00000001;
        private const uint FILE_SHARE_WRITE = 0x00000002;
        private const uint OPEN_EXISTING = 3;

        // CTL_CODE(FILE_DEVICE_UNKNOWN=0x22, 0x1728, METHOD_BUFFERED=0, FILE_SPECIAL_ACCESS=0)
        //   = (0x22 << 16) | (0x1728 << 2) | (0 << 14) | 0 = 0x225CA0
        private const uint IO_GET_KERNEL_DRIVERS = 0x225CA0;
        // CTL_CODE(..., 0x1729, ...) = 0x225CA4
        private const uint IO_DUMP_KERNEL_MODULE = 0x225CA4;

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern SafeFileHandle CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            int nInBufferSize,
            IntPtr lpOutBuffer,
            int nOutBufferSize,
            out int lpBytesReturned,
            IntPtr lpOverlapped);

        private readonly SafeFileHandle _handle;

        public KernelDriverOperations()
        {
            _handle = CreateFile(
                "\\\\.\\KsDumper",
                GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                IntPtr.Zero,
                OPEN_EXISTING,
                0,
                IntPtr.Zero);

            if (_handle.IsInvalid)
            {
                throw new Win32Exception(
                    Marshal.GetLastWin32Error(),
                    "Failed to open \\\\.\\KsDumper. Is the driver loaded?");
            }
        }

        public bool IsOpen
        {
            get { return _handle != null && !_handle.IsInvalid; }
        }

        /// <summary>
        /// Enumerates the loaded kernel modules.  The kernel-side handler walks
        /// the loaded module list (via ZwQuerySystemInformation/SystemModuleInformation)
        /// and returns one KERNEL_DRIVER_INFO per loaded image.
        /// </summary>
        public KERNEL_DRIVER_INFO[] GetKernelModules()
        {
            const int MaxBytes = 1024 * 1024; // 1 MB is plenty for the module list.
            IntPtr outBuf = Marshal.AllocHGlobal(MaxBytes);
            IntPtr inBuf = IntPtr.Zero;
            try
            {
                KERNEL_GET_DRIVERS_OPERATION op = new KERNEL_GET_DRIVERS_OPERATION();
                op.bufferAddress = outBuf;
                op.bufferSize = MaxBytes;
                op.moduleCount = 0;

                int opSize = Marshal.SizeOf(typeof(KERNEL_GET_DRIVERS_OPERATION));
                inBuf = Marshal.AllocHGlobal(opSize);
                Marshal.StructureToPtr(op, inBuf, false);

                int bytesReturned;
                bool ok = DeviceIoControl(
                    _handle, IO_GET_KERNEL_DRIVERS,
                    inBuf, opSize,
                    inBuf, opSize,
                    out bytesReturned,
                    IntPtr.Zero);

                if (!ok)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(),
                        "IO_GET_KERNEL_DRIVERS failed");
                }

                op = (KERNEL_GET_DRIVERS_OPERATION)Marshal.PtrToStructure(
                    inBuf, typeof(KERNEL_GET_DRIVERS_OPERATION));

                int entrySize = Marshal.SizeOf(typeof(KERNEL_DRIVER_INFO));
                KERNEL_DRIVER_INFO[] result = new KERNEL_DRIVER_INFO[op.moduleCount];
                IntPtr cursor = outBuf;
                for (int i = 0; i < op.moduleCount; i++)
                {
                    result[i] = (KERNEL_DRIVER_INFO)Marshal.PtrToStructure(
                        cursor, typeof(KERNEL_DRIVER_INFO));
                    cursor = IntPtr.Add(cursor, entrySize);
                }
                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(outBuf);
                if (inBuf != IntPtr.Zero) Marshal.FreeHGlobal(inBuf);
            }
        }

        /// <summary>
        /// Copies the in-memory image of the module at <paramref name="baseAddress"/>
        /// from kernel memory into a managed byte array using the
        /// IO_DUMP_KERNEL_MODULE IOCTL.  The returned buffer starts at the
        /// image base (offset 0 == ImageBase) and contains the full runtime
        /// snapshot of the module (all sections byte-for-byte).
        ///
        /// Callers (e.g. KernelModulesForm) use this single snapshot to derive
        /// both the raw .bin image and a code-only .sys PE, so both outputs
        /// reflect the same instant in time.
        /// </summary>
        public byte[] DumpKernelModule(ulong baseAddress, int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("size");

            IntPtr buffer = IntPtr.Zero;
            IntPtr inBuf = IntPtr.Zero;
            try
            {
                buffer = Marshal.AllocHGlobal(size);

                KERNEL_DUMP_DRIVER_OPERATION op = new KERNEL_DUMP_DRIVER_OPERATION();
                op.baseAddress = unchecked((IntPtr)(long)baseAddress);
                op.bufferAddress = buffer;
                op.bufferSize = size;
                op.bytesRead = 0;

                int opSize = Marshal.SizeOf(typeof(KERNEL_DUMP_DRIVER_OPERATION));
                inBuf = Marshal.AllocHGlobal(opSize);
                Marshal.StructureToPtr(op, inBuf, false);

                int bytesReturned;
                bool ok = DeviceIoControl(
                    _handle, IO_DUMP_KERNEL_MODULE,
                    inBuf, opSize,
                    inBuf, opSize,
                    out bytesReturned,
                    IntPtr.Zero);

                if (!ok)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(),
                        "IO_DUMP_KERNEL_MODULE failed");
                }

                op = (KERNEL_DUMP_DRIVER_OPERATION)Marshal.PtrToStructure(
                    inBuf, typeof(KERNEL_DUMP_DRIVER_OPERATION));

                if (op.bytesRead <= 0)
                    return new byte[0];

                byte[] result = new byte[op.bytesRead];
                Marshal.Copy(buffer, result, 0, op.bytesRead);
                return result;
            }
            finally
            {
                if (buffer != IntPtr.Zero) Marshal.FreeHGlobal(buffer);
                if (inBuf != IntPtr.Zero) Marshal.FreeHGlobal(inBuf);
            }
        }

        public void Dispose()
        {
            if (_handle != null)
                _handle.Dispose();
        }
    }
}