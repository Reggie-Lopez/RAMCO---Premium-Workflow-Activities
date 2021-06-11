// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.X509CertificateValidatorEx
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;

namespace Microsoft.IdentityModel
{
  internal class X509CertificateValidatorEx : X509CertificateValidator
  {
    private X509CertificateValidationMode _certificateValidationMode;
    private X509ChainPolicy _chainPolicy;
    private X509CertificateValidator _validator;

    public X509CertificateValidatorEx(
      X509CertificateValidationMode certificateValidationMode,
      X509RevocationMode revocationMode,
      StoreLocation trustedStoreLocation)
    {
      this._certificateValidationMode = certificateValidationMode;
      switch (this._certificateValidationMode)
      {
        case X509CertificateValidationMode.None:
          this._validator = X509CertificateValidator.None;
          break;
        case X509CertificateValidationMode.PeerTrust:
          this._validator = X509CertificateValidator.PeerTrust;
          break;
        case X509CertificateValidationMode.ChainTrust:
          bool useMachineContext1 = trustedStoreLocation == StoreLocation.LocalMachine;
          this._chainPolicy = new X509ChainPolicy();
          this._chainPolicy.RevocationMode = revocationMode;
          this._validator = X509CertificateValidator.CreateChainTrustValidator(useMachineContext1, this._chainPolicy);
          break;
        case X509CertificateValidationMode.PeerOrChainTrust:
          bool useMachineContext2 = trustedStoreLocation == StoreLocation.LocalMachine;
          this._chainPolicy = new X509ChainPolicy();
          this._chainPolicy.RevocationMode = revocationMode;
          this._validator = X509CertificateValidator.CreatePeerOrChainTrustValidator(useMachineContext2, this._chainPolicy);
          break;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(SR.GetString("ID4256")));
      }
    }

    public override void Validate(X509Certificate2 certificate)
    {
      if (this._certificateValidationMode == X509CertificateValidationMode.ChainTrust || this._certificateValidationMode == X509CertificateValidationMode.PeerOrChainTrust)
        this._chainPolicy.VerificationTime = DateTime.Now;
      this._validator.Validate(certificate);
    }
  }
}
