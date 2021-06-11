// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2Id
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2Id
  {
    private string _value;

    public Saml2Id()
      : this(Microsoft.IdentityModel.UniqueId.CreateRandomId())
    {
    }

    public Saml2Id(string value)
    {
      if (string.IsNullOrEmpty(value))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
      try
      {
        this._value = XmlConvert.VerifyNCName(value);
      }
      catch (XmlException ex)
      {
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID4128"), nameof (value), (Exception) ex));
      }
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj is Saml2Id saml2Id && StringComparer.Ordinal.Equals(this._value, saml2Id.Value);
    }

    public override int GetHashCode() => this._value.GetHashCode();

    public override string ToString() => this._value;

    public string Value => this._value;
  }
}
