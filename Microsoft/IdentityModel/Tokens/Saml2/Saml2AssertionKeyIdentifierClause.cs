// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2AssertionKeyIdentifierClause
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  [ComVisible(true)]
  public class Saml2AssertionKeyIdentifierClause : SecurityKeyIdentifierClause
  {
    private string _assertionId;

    public Saml2AssertionKeyIdentifierClause(string id)
      : this(id, (byte[]) null, 0)
    {
    }

    public Saml2AssertionKeyIdentifierClause(
      string id,
      byte[] derivationNonce,
      int derivationLength)
      : base((string) null, derivationNonce, derivationLength)
    {
      this._assertionId = !string.IsNullOrEmpty(id) ? id : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (id));
    }

    public string AssertionId => this._assertionId;

    [Obsolete("Use AssertionId property instead")]
    public new string Id => this._assertionId;

    public override bool Matches(SecurityKeyIdentifierClause keyIdentifierClause) => object.ReferenceEquals((object) this, (object) keyIdentifierClause) || Saml2AssertionKeyIdentifierClause.Matches(this.AssertionId, keyIdentifierClause);

    public static bool Matches(string assertionId, SecurityKeyIdentifierClause keyIdentifierClause)
    {
      if (string.IsNullOrEmpty(assertionId))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (assertionId));
      return keyIdentifierClause != null && (keyIdentifierClause is Saml2AssertionKeyIdentifierClause identifierClause && StringComparer.Ordinal.Equals(assertionId, identifierClause.AssertionId) || keyIdentifierClause is SamlAssertionKeyIdentifierClause identifierClause && StringComparer.Ordinal.Equals(assertionId, identifierClause.AssertionId));
    }

    public override string ToString() => "Saml2AssertionKeyIdentifierClause( Id = '" + this.AssertionId + "' )";
  }
}
