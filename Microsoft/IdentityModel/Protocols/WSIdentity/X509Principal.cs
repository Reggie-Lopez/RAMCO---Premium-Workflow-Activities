// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.X509Principal
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class X509Principal
  {
    private EkuPolicy _ekuPolicy;
    private string _principalName;

    public X509Principal(string principalName)
      : this(principalName, (EkuPolicy) null)
    {
    }

    public X509Principal(string principalName, EkuPolicy ekuPolicy)
    {
      this._principalName = !string.IsNullOrEmpty(principalName) ? principalName : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (principalName));
      this._ekuPolicy = ekuPolicy;
    }

    public EkuPolicy EkuPolicy
    {
      get => this._ekuPolicy;
      set => this._ekuPolicy = value;
    }

    public string PrincipalName
    {
      get => this._principalName;
      set => this._principalName = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (value));
    }
  }
}
