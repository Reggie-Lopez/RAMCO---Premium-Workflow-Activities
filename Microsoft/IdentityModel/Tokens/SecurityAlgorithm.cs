// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SecurityAlgorithm
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

namespace Microsoft.IdentityModel.Tokens
{
  internal static class SecurityAlgorithm
  {
    internal const string Sha1Digest = "http://www.w3.org/2000/09/xmldsig#sha1";
    internal const string Sha256Digest = "http://www.w3.org/2001/04/xmlenc#sha256";
    internal const string HmacSha1Signature = "http://www.w3.org/2000/09/xmldsig#hmac-sha1";
    internal const string RsaSha1Signature = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
    internal const string RsaSha256Signature = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
    internal const string Aes256Encryption = "http://www.w3.org/2001/04/xmlenc#aes256-cbc";
    internal const string RsaOaepKeyWrap = "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p";
    internal const int DefaultSymmetricKeyLength = 256;
    internal const string DefaultEncryptionAlgorithm = "http://www.w3.org/2001/04/xmlenc#aes256-cbc";
    internal const string DefaultAsymmetricKeyWrapAlgorithm = "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p";
    internal const string DefaultAsymmetricSignatureAlgorithm = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
    internal const string DefaultDigestAlgorithm = "http://www.w3.org/2001/04/xmlenc#sha256";
  }
}
