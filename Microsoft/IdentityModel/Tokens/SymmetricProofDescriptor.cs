// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SymmetricProofDescriptor
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class SymmetricProofDescriptor : ProofDescriptor
  {
    private byte[] _key;
    private int _keySizeInBits;
    private byte[] _sourceEntropy;
    private byte[] _targetEntropy;
    private SecurityKeyIdentifier _ski;
    private Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials _requestorWrappingCredentials;
    private Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials _targetWrappingCredentials;

    public SymmetricProofDescriptor(byte[] key, Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials targetWrappingCredentials)
    {
      this._keySizeInBits = key != null ? key.Length : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (key));
      this._key = key;
      this._targetWrappingCredentials = targetWrappingCredentials;
    }

    public SymmetricProofDescriptor(Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials targetWrappingCredentials)
      : this(256, targetWrappingCredentials)
    {
    }

    public SymmetricProofDescriptor(
      int keySizeInBits,
      Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials targetWrappingCredentials)
      : this(keySizeInBits, targetWrappingCredentials, (Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials) null)
    {
    }

    public SymmetricProofDescriptor(
      int keySizeInBits,
      Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials targetWrappingCredentials,
      Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials requestorWrappingCredentials)
      : this(keySizeInBits, targetWrappingCredentials, requestorWrappingCredentials, (string) null)
    {
    }

    public SymmetricProofDescriptor(
      int keySizeInBits,
      Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials targetWrappingCredentials,
      Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials requestorWrappingCredentials,
      string encryptWith)
    {
      this._keySizeInBits = keySizeInBits;
      this._key = encryptWith == "http://www.w3.org/2001/04/xmlenc#des-cbc" || encryptWith == "http://www.w3.org/2001/04/xmlenc#tripledes-cbc" || encryptWith == "http://www.w3.org/2001/04/xmlenc#kw-tripledes" ? KeyGenerator.GenerateDESKey(this._keySizeInBits) : KeyGenerator.GenerateSymmetricKey(this._keySizeInBits);
      this._requestorWrappingCredentials = requestorWrappingCredentials;
      this._targetWrappingCredentials = targetWrappingCredentials;
    }

    public SymmetricProofDescriptor(
      int keySizeInBits,
      Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials targetWrappingCredentials,
      Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials requestorWrappingCredentials,
      byte[] sourceEntropy)
      : this(keySizeInBits, targetWrappingCredentials, requestorWrappingCredentials, sourceEntropy, (string) null)
    {
    }

    public SymmetricProofDescriptor(
      int keySizeInBits,
      Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials targetWrappingCredentials,
      Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials requestorWrappingCredentials,
      byte[] sourceEntropy,
      string encryptWith)
    {
      if (sourceEntropy == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (sourceEntropy));
      if (sourceEntropy.Length == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (sourceEntropy), SR.GetString("ID2058"));
      this._keySizeInBits = keySizeInBits;
      this._sourceEntropy = sourceEntropy;
      this._key = encryptWith == "http://www.w3.org/2001/04/xmlenc#des-cbc" || encryptWith == "http://www.w3.org/2001/04/xmlenc#tripledes-cbc" || encryptWith == "http://www.w3.org/2001/04/xmlenc#kw-tripledes" ? KeyGenerator.GenerateDESKey(this._keySizeInBits, this._sourceEntropy, out this._targetEntropy) : KeyGenerator.GenerateSymmetricKey(this._keySizeInBits, this._sourceEntropy, out this._targetEntropy);
      this._requestorWrappingCredentials = requestorWrappingCredentials;
      this._targetWrappingCredentials = targetWrappingCredentials;
    }

    public byte[] GetKeyBytes() => this._key;

    protected Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials RequestorEncryptingCredentials => this._requestorWrappingCredentials;

    protected byte[] GetSourceEntropy() => this._sourceEntropy;

    protected byte[] GetTargetEntropy() => this._targetEntropy;

    protected Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials TargetEncryptingCredentials => this._targetWrappingCredentials;

    public override void ApplyTo(RequestSecurityTokenResponse response)
    {
      if (response == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (response));
      if (this._targetEntropy != null)
      {
        response.RequestedProofToken = new RequestedProofToken("http://schemas.microsoft.com/idfx/computedkeyalgorithm/psha1");
        response.KeySizeInBits = new int?(this._keySizeInBits);
        response.Entropy = new Entropy(this._targetEntropy, this._requestorWrappingCredentials);
      }
      else
        response.RequestedProofToken = new RequestedProofToken(this._key, this._requestorWrappingCredentials);
    }

    public override SecurityKeyIdentifier KeyIdentifier
    {
      get
      {
        if (this._ski == null)
          this._ski = KeyGenerator.GetSecurityKeyIdentifier(this._key, this._targetWrappingCredentials);
        return this._ski;
      }
    }
  }
}
