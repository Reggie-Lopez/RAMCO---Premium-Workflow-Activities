// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SafeCertStoreHandle
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.Win32.SafeHandles;
using System;

namespace Microsoft.IdentityModel
{
  internal class SafeCertStoreHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    private SafeCertStoreHandle()
      : base(true)
    {
    }

    private SafeCertStoreHandle(IntPtr handle)
      : base(true)
      => this.SetHandle(handle);

    public static SafeCertStoreHandle InvalidHandle => new SafeCertStoreHandle(IntPtr.Zero);

    protected override bool ReleaseHandle() => CryptoUtil.CAPI.CertCloseStore(this.handle, 0U);
  }
}
