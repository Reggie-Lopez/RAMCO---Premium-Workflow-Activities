// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SafeCloseHandle
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.IdentityModel
{
  internal sealed class SafeCloseHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    private const string KERNEL32 = "kernel32.dll";

    private SafeCloseHandle()
      : base(true)
    {
    }

    internal SafeCloseHandle(IntPtr handle, bool ownsHandle)
      : base(ownsHandle)
      => this.SetHandle(handle);

    protected override bool ReleaseHandle() => SafeCloseHandle.CloseHandle(this.handle);

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseHandle(IntPtr handle);
  }
}
