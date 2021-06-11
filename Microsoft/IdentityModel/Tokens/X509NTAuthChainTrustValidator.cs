// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.X509NTAuthChainTrustValidator
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class X509NTAuthChainTrustValidator : X509CertificateValidator
  {
    private bool _useMachineContext;
    private X509ChainPolicy _chainPolicy;
    private uint _chainPolicyOID = 6;

    public X509NTAuthChainTrustValidator()
      : this(false, (X509ChainPolicy) null)
    {
    }

    public X509NTAuthChainTrustValidator(bool useMachineContext, X509ChainPolicy chainPolicy)
    {
      this._useMachineContext = useMachineContext;
      this._chainPolicy = chainPolicy;
    }

    public override void Validate(X509Certificate2 certificate)
    {
      if (certificate == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (certificate));
      Microsoft.IdentityModel.X509CertificateChain certificateChain = new Microsoft.IdentityModel.X509CertificateChain(this._useMachineContext, this._chainPolicyOID);
      if (this._chainPolicy != null)
        certificateChain.ChainPolicy = this._chainPolicy;
      if (!certificateChain.Build(certificate))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenValidationException(Microsoft.IdentityModel.SR.GetString("ID4070", (object) X509Util.GetCertificateId(certificate), (object) X509NTAuthChainTrustValidator.GetChainStatusInformation(certificateChain.ChainStatus))));
    }

    private static string GetChainStatusInformation(X509ChainStatus[] chainStatus)
    {
      if (chainStatus == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder(128);
      for (int index = 0; index < chainStatus.Length; ++index)
      {
        stringBuilder.Append(chainStatus[index].StatusInformation);
        stringBuilder.Append(" ");
      }
      return stringBuilder.ToString();
    }
  }
}
