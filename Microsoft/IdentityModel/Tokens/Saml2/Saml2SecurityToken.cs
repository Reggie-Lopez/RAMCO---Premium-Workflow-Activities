// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityToken
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
  public class Saml2SecurityToken : SecurityToken
  {
    private Saml2Assertion _assertion;
    private ReadOnlyCollection<SecurityKey> _keys;
    private SecurityToken _issuerToken;

    public Saml2SecurityToken(Saml2Assertion assertion)
      : this(assertion, EmptyReadOnlyCollection<SecurityKey>.Instance, (SecurityToken) null)
    {
    }

    public Saml2SecurityToken(
      Saml2Assertion assertion,
      ReadOnlyCollection<SecurityKey> keys,
      SecurityToken issuerToken)
    {
      if (assertion == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (assertion));
      if (keys == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (keys));
      this._assertion = assertion;
      this._keys = keys;
      this._issuerToken = issuerToken;
    }

    public Saml2Assertion Assertion => this._assertion;

    public override bool CanCreateKeyIdentifierClause<T>() => (object) typeof (T) == (object) typeof (Saml2AssertionKeyIdentifierClause) || base.CanCreateKeyIdentifierClause<T>();

    public override T CreateKeyIdentifierClause<T>()
    {
      if ((object) typeof (T) == (object) typeof (Saml2AssertionKeyIdentifierClause))
        return new Saml2AssertionKeyIdentifierClause(this._assertion.Id.Value) as T;
      return (object) typeof (T) == (object) typeof (SamlAssertionKeyIdentifierClause) ? new WrappedSaml2AssertionKeyIdentifierClause(new Saml2AssertionKeyIdentifierClause(this._assertion.Id.Value)) as T : base.CreateKeyIdentifierClause<T>();
    }

    public override bool MatchesKeyIdentifierClause(SecurityKeyIdentifierClause keyIdentifierClause) => Saml2AssertionKeyIdentifierClause.Matches(this.Id, keyIdentifierClause) || base.MatchesKeyIdentifierClause(keyIdentifierClause);

    public override string Id => this._assertion.Id.Value;

    public SecurityToken IssuerToken => this._issuerToken;

    public override ReadOnlyCollection<SecurityKey> SecurityKeys => this._keys;

    public override DateTime ValidFrom => this._assertion.Conditions != null && this._assertion.Conditions.NotBefore.HasValue ? this._assertion.Conditions.NotBefore.Value : DateTime.MinValue;

    public override DateTime ValidTo => this._assertion.Conditions != null && this._assertion.Conditions.NotOnOrAfter.HasValue ? this._assertion.Conditions.NotOnOrAfter.Value : DateTime.MaxValue;
  }
}
