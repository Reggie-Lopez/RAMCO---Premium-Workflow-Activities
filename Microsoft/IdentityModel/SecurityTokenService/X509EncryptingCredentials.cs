// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SecurityTokenService.X509EncryptingCredentials
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.SecurityTokenService
{
  [ComVisible(true)]
  public class X509EncryptingCredentials : EncryptingCredentials
  {
    private X509Certificate2 _certificate;

    public X509EncryptingCredentials(X509Certificate2 certificate)
      : this(new X509SecurityToken(certificate))
    {
    }

    public X509EncryptingCredentials(X509Certificate2 certificate, string keyWrappingAlgorithm)
      : this(new X509SecurityToken(certificate), keyWrappingAlgorithm)
    {
    }

    public X509EncryptingCredentials(X509Certificate2 certificate, SecurityKeyIdentifier ski)
      : this(new X509SecurityToken(certificate), ski, "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p")
    {
    }

    public X509EncryptingCredentials(
      X509Certificate2 certificate,
      SecurityKeyIdentifier ski,
      string keyWrappingAlgorithm)
      : this(new X509SecurityToken(certificate), ski, keyWrappingAlgorithm)
    {
    }

    internal X509EncryptingCredentials(X509SecurityToken token)
      : this(token, new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[1]
      {
        (SecurityKeyIdentifierClause) token.CreateKeyIdentifierClause<X509IssuerSerialKeyIdentifierClause>()
      }), "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p")
    {
    }

    internal X509EncryptingCredentials(X509SecurityToken token, string keyWrappingAlgorithm)
      : this(token, new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[1]
      {
        (SecurityKeyIdentifierClause) token.CreateKeyIdentifierClause<X509IssuerSerialKeyIdentifierClause>()
      }), keyWrappingAlgorithm)
    {
    }

    internal X509EncryptingCredentials(
      X509SecurityToken token,
      SecurityKeyIdentifier ski,
      string keyWrappingAlgorithm)
      : base(token.SecurityKeys[0], ski, keyWrappingAlgorithm)
    {
      this._certificate = token.Certificate;
    }

    public X509Certificate2 Certificate => this._certificate;
  }
}
