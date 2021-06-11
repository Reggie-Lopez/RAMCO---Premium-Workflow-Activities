// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SafeHGlobalHandle
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.IdentityModel
{
  internal sealed class SafeHGlobalHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    private SafeHGlobalHandle()
      : base(true)
    {
    }

    private SafeHGlobalHandle(IntPtr handle)
      : base(true)
      => this.SetHandle(handle);

    protected override bool ReleaseHandle()
    {
      Marshal.FreeHGlobal(this.handle);
      return true;
    }

    public static SafeHGlobalHandle InvalidHandle => new SafeHGlobalHandle(IntPtr.Zero);

    public static SafeHGlobalHandle AllocHGlobal(string s)
    {
      byte[] bytes = new byte[checked (s.Length + 1 * 2)];
      Encoding.Unicode.GetBytes(s, 0, s.Length, bytes, 0);
      return SafeHGlobalHandle.AllocHGlobal(bytes);
    }

    public static SafeHGlobalHandle AllocHGlobal(byte[] bytes)
    {
      SafeHGlobalHandle safeHglobalHandle = SafeHGlobalHandle.AllocHGlobal(bytes.Length);
      Marshal.Copy(bytes, 0, safeHglobalHandle.DangerousGetHandle(), bytes.Length);
      return safeHglobalHandle;
    }

    public static SafeHGlobalHandle AllocHGlobal(uint cb) => SafeHGlobalHandle.AllocHGlobal((int) cb);

    public static SafeHGlobalHandle AllocHGlobal(int cb)
    {
      if (cb < 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentOutOfRangeException(nameof (cb), SR.GetString("ID0017")));
      SafeHGlobalHandle safeHglobalHandle = new SafeHGlobalHandle();
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
      }
      finally
      {
        IntPtr handle = Marshal.AllocHGlobal(cb);
        safeHglobalHandle.SetHandle(handle);
      }
      return safeHglobalHandle;
    }
  }
}
