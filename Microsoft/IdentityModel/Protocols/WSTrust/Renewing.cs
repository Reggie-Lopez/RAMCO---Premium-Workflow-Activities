// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.Renewing
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class Renewing
  {
    private bool _allowRenewal = true;
    private bool _okForRenewalAfterExpiration;

    public Renewing()
    {
    }

    public Renewing(bool allowRenewal, bool okForRenewalAfterExpiration)
    {
      this._allowRenewal = allowRenewal;
      this._okForRenewalAfterExpiration = okForRenewalAfterExpiration;
    }

    public bool AllowRenewal
    {
      get => this._allowRenewal;
      set => this._allowRenewal = value;
    }

    public bool OkForRenewalAfterExpiration
    {
      get => this._okForRenewalAfterExpiration;
      set => this._okForRenewalAfterExpiration = value;
    }
  }
}
