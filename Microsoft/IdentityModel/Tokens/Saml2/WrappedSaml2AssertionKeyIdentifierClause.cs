// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.Saml2.WrappedSaml2AssertionKeyIdentifierClause
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
  internal class WrappedSaml2AssertionKeyIdentifierClause : SamlAssertionKeyIdentifierClause
  {
    private Saml2AssertionKeyIdentifierClause _clause;

    public WrappedSaml2AssertionKeyIdentifierClause(Saml2AssertionKeyIdentifierClause clause)
      : base(clause.AssertionId)
      => this._clause = clause;

    public override bool CanCreateKey => this._clause.CanCreateKey;

    public override SecurityKey CreateKey() => this._clause.CreateKey();

    public override bool Matches(SecurityKeyIdentifierClause keyIdentifierClause) => this._clause.Matches(keyIdentifierClause);

    public Saml2AssertionKeyIdentifierClause WrappedClause => this._clause;
  }
}
