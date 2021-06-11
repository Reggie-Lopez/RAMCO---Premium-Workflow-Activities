// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.IssuerTokenResolver
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class IssuerTokenResolver : SecurityTokenResolver
  {
    public static readonly StoreName DefaultStoreName = StoreName.TrustedPeople;
    public static readonly StoreLocation DefaultStoreLocation = StoreLocation.LocalMachine;
    private SecurityTokenResolver _wrappedTokenResolver;
    internal static IssuerTokenResolver DefaultInstance = new IssuerTokenResolver();

    public IssuerTokenResolver()
      : this((SecurityTokenResolver) new X509CertificateStoreTokenResolver(IssuerTokenResolver.DefaultStoreName, IssuerTokenResolver.DefaultStoreLocation))
    {
    }

    public IssuerTokenResolver(SecurityTokenResolver wrappedTokenResolver) => this._wrappedTokenResolver = wrappedTokenResolver != null ? wrappedTokenResolver : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (wrappedTokenResolver));

    public SecurityTokenResolver WrappedTokenResolver => this._wrappedTokenResolver;

    protected override bool TryResolveSecurityKeyCore(
      SecurityKeyIdentifierClause keyIdentifierClause,
      out SecurityKey key)
    {
      if (keyIdentifierClause == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (keyIdentifierClause));
      key = (SecurityKey) null;
      if (keyIdentifierClause is X509RawDataKeyIdentifierClause identifierClause)
      {
        key = identifierClause.CreateKey();
        return true;
      }
      if (keyIdentifierClause is RsaKeyIdentifierClause identifierClause)
      {
        key = identifierClause.CreateKey();
        return true;
      }
      return this._wrappedTokenResolver.TryResolveSecurityKey(keyIdentifierClause, out key);
    }

    protected override bool TryResolveTokenCore(
      SecurityKeyIdentifier keyIdentifier,
      out SecurityToken token)
    {
      if (keyIdentifier == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (keyIdentifier));
      token = (SecurityToken) null;
      foreach (SecurityKeyIdentifierClause keyIdentifierClause in keyIdentifier)
      {
        if (this.TryResolveTokenCore(keyIdentifierClause, out token))
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
      if (keyIdentifierClause is X509RawDataKeyIdentifierClause identifierClause)
      {
        token = (SecurityToken) new X509SecurityToken(new X509Certificate2(identifierClause.GetX509RawData()));
        return true;
      }
      if (keyIdentifierClause is RsaKeyIdentifierClause identifierClause)
      {
        token = (SecurityToken) new RsaSecurityToken(identifierClause.Rsa);
        return true;
      }
      return this._wrappedTokenResolver.TryResolveToken(keyIdentifierClause, out token);
    }
  }
}
