// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.X509CertificateCredential
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class X509CertificateCredential : IUserCredential
  {
    private SecurityKeyIdentifierClause _x509IdentifierClause;
    private X509Principal _x509Principal;
    private X509SubjectAndIssuer _x509SubjectAndIssuer;
    private string _x509SubjectName;

    public X509CertificateCredential(X509Certificate2 certificate) => this._x509IdentifierClause = certificate != null ? (SecurityKeyIdentifierClause) new X509ThumbprintKeyIdentifierClause(certificate) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (certificate));

    public X509CertificateCredential(SecurityKeyIdentifierClause x509IdentifierClause)
    {
      switch (x509IdentifierClause)
      {
        case null:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (x509IdentifierClause));
        case X509ThumbprintKeyIdentifierClause _:
        case X509IssuerSerialKeyIdentifierClause _:
        case X509RawDataKeyIdentifierClause _:
        case X509SubjectKeyIdentifierClause _:
          this._x509IdentifierClause = x509IdentifierClause;
          break;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID2041"), nameof (x509IdentifierClause)));
      }
    }

    public X509CertificateCredential(X509Principal x509Principal) => this._x509Principal = x509Principal != null ? x509Principal : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (x509Principal));

    public X509CertificateCredential(X509SubjectAndIssuer x509SubjectAndIssuer) => this._x509SubjectAndIssuer = x509SubjectAndIssuer != null ? x509SubjectAndIssuer : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (x509SubjectAndIssuer));

    public X509CertificateCredential(string x509SubjectName) => this._x509SubjectName = !string.IsNullOrEmpty(x509SubjectName) ? x509SubjectName : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (x509SubjectName));

    public X509Principal X509Principal => this._x509Principal;

    public SecurityKeyIdentifierClause X509SecurityTokenIdentifierClause => this._x509IdentifierClause;

    public X509SubjectAndIssuer X509SubjectAndIssuer => this._x509SubjectAndIssuer;

    public string X509SubjectName => this._x509SubjectName;

    public UserCredentialType CredentialType => UserCredentialType.X509V3Credential;
  }
}
