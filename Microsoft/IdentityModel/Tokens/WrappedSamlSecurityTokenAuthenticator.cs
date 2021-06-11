// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.WrappedSamlSecurityTokenAuthenticator
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Tokens
{
  internal class WrappedSamlSecurityTokenAuthenticator : SecurityTokenAuthenticator
  {
    private WrappedSaml11SecurityTokenAuthenticator _wrappedSaml11SecurityTokenAuthenticator;
    private WrappedSaml2SecurityTokenAuthenticator _wrappedSaml2SecurityTokenAuthenticator;

    public WrappedSamlSecurityTokenAuthenticator(
      WrappedSaml11SecurityTokenAuthenticator wrappedSaml11SecurityTokenAuthenticator,
      WrappedSaml2SecurityTokenAuthenticator wrappedSaml2SecurityTokenAuthenticator)
    {
      if (wrappedSaml11SecurityTokenAuthenticator == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (wrappedSaml11SecurityTokenAuthenticator));
      if (wrappedSaml2SecurityTokenAuthenticator == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (wrappedSaml2SecurityTokenAuthenticator));
      this._wrappedSaml11SecurityTokenAuthenticator = wrappedSaml11SecurityTokenAuthenticator;
      this._wrappedSaml2SecurityTokenAuthenticator = wrappedSaml2SecurityTokenAuthenticator;
    }

    protected override bool CanValidateTokenCore(SecurityToken token) => this._wrappedSaml11SecurityTokenAuthenticator.CanValidateToken(token) || this._wrappedSaml2SecurityTokenAuthenticator.CanValidateToken(token);

    protected override ReadOnlyCollection<IAuthorizationPolicy> ValidateTokenCore(
      SecurityToken token)
    {
      if (this._wrappedSaml11SecurityTokenAuthenticator.CanValidateToken(token))
        return this._wrappedSaml11SecurityTokenAuthenticator.ValidateToken(token);
      return this._wrappedSaml2SecurityTokenAuthenticator.CanValidateToken(token) ? this._wrappedSaml2SecurityTokenAuthenticator.ValidateToken(token) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID4101", (object) token.GetType().ToString())));
    }
  }
}
