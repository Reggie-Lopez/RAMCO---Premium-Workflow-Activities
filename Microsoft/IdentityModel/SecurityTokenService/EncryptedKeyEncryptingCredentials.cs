// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SecurityTokenService.EncryptedKeyEncryptingCredentials
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.SecurityTokenService
{
  [ComVisible(true)]
  public class EncryptedKeyEncryptingCredentials : EncryptingCredentials
  {
    private EncryptingCredentials _wrappingCredentials;
    private byte[] _keyBytes;

    public EncryptedKeyEncryptingCredentials(X509Certificate2 certificate)
      : this((EncryptingCredentials) new X509EncryptingCredentials(certificate), 256, "http://www.w3.org/2001/04/xmlenc#aes256-cbc")
    {
    }

    public EncryptedKeyEncryptingCredentials(
      X509Certificate2 certificate,
      string keyWrappingAlgorithm,
      int keySizeInBits,
      string encryptionAlgorithm)
      : this((EncryptingCredentials) new X509EncryptingCredentials(certificate, keyWrappingAlgorithm), keySizeInBits, encryptionAlgorithm)
    {
    }

    public EncryptedKeyEncryptingCredentials(
      EncryptingCredentials wrappingCredentials,
      int keySizeInBits,
      string encryptionAlgorithm)
    {
      if (wrappingCredentials == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (wrappingCredentials));
      this._keyBytes = encryptionAlgorithm == "http://www.w3.org/2001/04/xmlenc#des-cbc" || encryptionAlgorithm == "http://www.w3.org/2001/04/xmlenc#tripledes-cbc" || encryptionAlgorithm == "http://www.w3.org/2001/04/xmlenc#kw-tripledes" ? KeyGenerator.GenerateDESKey(keySizeInBits) : KeyGenerator.GenerateSymmetricKey(keySizeInBits);
      this.SecurityKey = (SecurityKey) new InMemorySymmetricSecurityKey(this._keyBytes);
      this._wrappingCredentials = wrappingCredentials;
      this.SecurityKeyIdentifier = new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[1]
      {
        (SecurityKeyIdentifierClause) new EncryptedKeyIdentifierClause(this._wrappingCredentials.SecurityKey.EncryptKey(this._wrappingCredentials.Algorithm, this._keyBytes), this._wrappingCredentials.Algorithm, this._wrappingCredentials.SecurityKeyIdentifier)
      });
      this.Algorithm = encryptionAlgorithm;
    }

    public EncryptingCredentials WrappingCredentials => this._wrappingCredentials;
  }
}
