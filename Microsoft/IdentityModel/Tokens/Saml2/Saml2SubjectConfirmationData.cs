// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2SubjectConfirmationData
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2SubjectConfirmationData
  {
    private string _address;
    private Saml2Id _inResponseTo;
    private Collection<SecurityKeyIdentifier> _keyIdentifiers = new Collection<SecurityKeyIdentifier>();
    private DateTime? _notBefore;
    private DateTime? _notOnOrAfter;
    private Uri _recipient;

    public string Address
    {
      get => this._address;
      set => this._address = XmlUtil.NormalizeEmptyString(value);
    }

    public Saml2Id InResponseTo
    {
      get => this._inResponseTo;
      set => this._inResponseTo = value;
    }

    public Collection<SecurityKeyIdentifier> KeyIdentifiers => this._keyIdentifiers;

    public DateTime? NotBefore
    {
      get => this._notBefore;
      set => this._notBefore = DateTimeUtil.ToUniversalTime(value);
    }

    public DateTime? NotOnOrAfter
    {
      get => this._notOnOrAfter;
      set => this._notOnOrAfter = DateTimeUtil.ToUniversalTime(value);
    }

    public Uri Recipient
    {
      get => this._recipient;
      set => this._recipient = !((Uri) null != value) || value.IsAbsoluteUri ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID0013"));
    }
  }
}
