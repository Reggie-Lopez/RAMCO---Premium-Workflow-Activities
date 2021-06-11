// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SafeLsaReturnBufferHandle
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Win32.SafeHandles;
using System;

namespace Microsoft.IdentityModel
{
  internal sealed class SafeLsaReturnBufferHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    private SafeLsaReturnBufferHandle()
      : base(true)
    {
    }

    internal SafeLsaReturnBufferHandle(IntPtr handle)
      : base(true)
      => this.SetHandle(handle);

    internal static SafeLsaReturnBufferHandle InvalidHandle => new SafeLsaReturnBufferHandle(IntPtr.Zero);

    protected override bool ReleaseHandle() => NativeMethods.LsaFreeReturnBuffer(this.handle) >= 0;
  }
}
