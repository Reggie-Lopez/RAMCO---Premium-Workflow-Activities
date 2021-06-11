// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SecurityTokenService.X509SigningCredentials
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.SecurityTokenService
{
  [ComVisible(true)]
  public class X509SigningCredentials : SigningCredentials
  {
    private X509Certificate2 _certificate;

    public X509SigningCredentials(X509Certificate2 certificate)
      : this(certificate, new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[1]
      {
        (SecurityKeyIdentifierClause) new X509SecurityToken(certificate).CreateKeyIdentifierClause<X509RawDataKeyIdentifierClause>()
      }))
    {
    }

    public X509SigningCredentials(
      X509Certificate2 certificate,
      string signatureAlgorithm,
      string digestAlgorithm)
      : this(new X509SecurityToken(certificate), new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[1]
      {
        (SecurityKeyIdentifierClause) new X509SecurityToken(certificate).CreateKeyIdentifierClause<X509RawDataKeyIdentifierClause>()
      }), signatureAlgorithm, digestAlgorithm)
    {
    }

    public X509SigningCredentials(X509Certificate2 certificate, SecurityKeyIdentifier ski)
      : this(new X509SecurityToken(certificate), ski, "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", "http://www.w3.org/2001/04/xmlenc#sha256")
    {
    }

    public X509SigningCredentials(
      X509Certificate2 certificate,
      SecurityKeyIdentifier ski,
      string signatureAlgorithm,
      string digestAlgorithm)
      : this(new X509SecurityToken(certificate), ski, signatureAlgorithm, digestAlgorithm)
    {
    }

    internal X509SigningCredentials(
      X509SecurityToken token,
      SecurityKeyIdentifier ski,
      string signatureAlgorithm,
      string digestAlgorithm)
      : base(token.SecurityKeys[0], signatureAlgorithm, digestAlgorithm, ski)
    {
      this._certificate = token.Certificate;
      if (!this._certificate.HasPrivateKey)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (token), SR.GetString("ID2057"));
    }

    public X509Certificate2 Certificate => this._certificate;
  }
}
