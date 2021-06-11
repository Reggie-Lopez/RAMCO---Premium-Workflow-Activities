// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.EncryptionMethod
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class EncryptionMethod
  {
    private Uri _algorithm;

    public EncryptionMethod(Uri algorithm) => this._algorithm = !(algorithm == (Uri) null) ? algorithm : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (algorithm));

    public Uri Algorithm
    {
      get => this._algorithm;
      set => this._algorithm = !(value == (Uri) null) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }
  }
}
