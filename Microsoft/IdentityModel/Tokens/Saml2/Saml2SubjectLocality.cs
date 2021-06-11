// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2SubjectLocality
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2SubjectLocality
  {
    private string _address;
    private string _dnsName;

    public Saml2SubjectLocality()
    {
    }

    public Saml2SubjectLocality(string address, string dnsName)
    {
      this.Address = address;
      this.DnsName = dnsName;
    }

    public string Address
    {
      get => this._address;
      set => this._address = XmlUtil.NormalizeEmptyString(value);
    }

    public string DnsName
    {
      get => this._dnsName;
      set => this._dnsName = XmlUtil.NormalizeEmptyString(value);
    }
  }
}
