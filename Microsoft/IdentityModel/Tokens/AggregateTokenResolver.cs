// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.AggregateTokenResolver
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class AggregateTokenResolver : SecurityTokenResolver
  {
    private List<SecurityTokenResolver> _tokenResolvers = new List<SecurityTokenResolver>();

    public AggregateTokenResolver(IEnumerable<SecurityTokenResolver> tokenResolvers)
    {
      if (tokenResolvers == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (tokenResolvers));
      this.AddNonEmptyResolvers(tokenResolvers);
    }

    public ReadOnlyCollection<SecurityTokenResolver> TokenResolvers => this._tokenResolvers.AsReadOnly();

    protected override bool TryResolveSecurityKeyCore(
      SecurityKeyIdentifierClause keyIdentifierClause,
      out SecurityKey key)
    {
      if (keyIdentifierClause == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (keyIdentifierClause));
      key = (SecurityKey) null;
      foreach (SecurityTokenResolver tokenResolver in this._tokenResolvers)
      {
        if (tokenResolver.TryResolveSecurityKey(keyIdentifierClause, out key))
          return true;
      }
      return false;
    }

    protected override bool TryResolveTokenCore(
      SecurityKeyIdentifier keyIdentifier,
      out SecurityToken token)
    {
      if (keyIdentifier == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyIdentifer");
      token = (SecurityToken) null;
      foreach (SecurityTokenResolver tokenResolver in this._tokenResolvers)
      {
        if (tokenResolver.TryResolveToken(keyIdentifier, out token))
          return true;
      }
      return false;
    }

    protected override bool TryResolveTokenCore(
      SecurityKeyIdentifierClause keyIdentifierClause,
      out SecurityToken token)
    {
      if (keyIdentifierClause == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (keyIdentifierClause));
      token = (SecurityToken) null;
      foreach (SecurityTokenResolver tokenResolver in this._tokenResolvers)
      {
        if (tokenResolver.TryResolveToken(keyIdentifierClause, out token))
          return true;
      }
      return false;
    }

    private void AddNonEmptyResolvers(IEnumerable<SecurityTokenResolver> resolvers)
    {
      foreach (SecurityTokenResolver resolver in resolvers)
      {
        if (resolver != null && resolver != EmptySecurityTokenResolver.Instance)
          this._tokenResolvers.Add(resolver);
      }
    }
  }
}
