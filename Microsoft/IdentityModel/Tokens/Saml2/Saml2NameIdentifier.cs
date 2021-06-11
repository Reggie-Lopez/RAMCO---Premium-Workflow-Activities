// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2NameIdentifier
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
  public class Saml2NameIdentifier
  {
    private Uri _format;
    private string _nameQualifier;
    private string _spNameQualifier;
    private string _spProvidedId;
    private string _value;
    private Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials _encryptingCredentials;
    private Collection<EncryptedKeyIdentifierClause> _externalEncryptedKeys;

    public Saml2NameIdentifier(string name)
      : this(name, (Uri) null)
    {
    }

    public Saml2NameIdentifier(string name, Uri format)
    {
      if (string.IsNullOrEmpty(name))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (name));
      this._format = !((Uri) null != format) || format.IsAbsoluteUri ? format : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (format), Microsoft.IdentityModel.SR.GetString("ID0013"));
      this._value = name;
      this._externalEncryptedKeys = new Collection<EncryptedKeyIdentifierClause>();
    }

    public Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials EncryptingCredentials
    {
      get => this._encryptingCredentials;
      set => this._encryptingCredentials = value;
    }

    public Collection<EncryptedKeyIdentifierClause> ExternalEncryptedKeys => this._externalEncryptedKeys;

    public Uri Format
    {
      get => this._format;
      set => this._format = !((Uri) null != value) || value.IsAbsoluteUri ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (value), Microsoft.IdentityModel.SR.GetString("ID0013"));
    }

    public string NameQualifier
    {
      get => this._nameQualifier;
      set => this._nameQualifier = XmlUtil.NormalizeEmptyString(value);
    }

    public string SPNameQualifier
    {
      get => this._spNameQualifier;
      set => this._spNameQualifier = XmlUtil.NormalizeEmptyString(value);
    }

    public string SPProvidedId
    {
      get => this._spProvidedId;
      set => this._spProvidedId = XmlUtil.NormalizeEmptyString(value);
    }

    public string Value
    {
      get => this._value;
      set => this._value = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }
  }
}
