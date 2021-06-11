// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SafeCertContextHandle
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Win32.SafeHandles;
using System;

namespace Microsoft.IdentityModel
{
  internal class SafeCertContextHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    private SafeCertContextHandle()
      : base(true)
    {
    }

    private SafeCertContextHandle(IntPtr handle)
      : base(true)
      => this.SetHandle(handle);

    internal static SafeCertContextHandle InvalidHandle => new SafeCertContextHandle(IntPtr.Zero);

    protected override bool ReleaseHandle() => CryptoUtil.CAPI.CertFreeCertificateContext(this.handle);
  }
}
