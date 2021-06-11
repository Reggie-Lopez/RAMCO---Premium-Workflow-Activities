// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.AsymmetricProofDescriptor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class AsymmetricProofDescriptor : ProofDescriptor
  {
    private SecurityKeyIdentifier _keyIdentifier;

    public AsymmetricProofDescriptor()
    {
    }

    public AsymmetricProofDescriptor(RSA rsaAlgorithm) => this._keyIdentifier = rsaAlgorithm != null ? new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[1]
    {
      (SecurityKeyIdentifierClause) new RsaKeyIdentifierClause(rsaAlgorithm)
    }) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (rsaAlgorithm));

    public AsymmetricProofDescriptor(SecurityKeyIdentifier keyIdentifier) => this._keyIdentifier = keyIdentifier != null ? keyIdentifier : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (keyIdentifier));

    public override void ApplyTo(RequestSecurityTokenResponse response)
    {
      if (response == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (response));
    }

    public override SecurityKeyIdentifier KeyIdentifier => this._keyIdentifier;
  }
}
