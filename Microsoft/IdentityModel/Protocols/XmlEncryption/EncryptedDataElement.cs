// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlEncryption.EncryptedDataElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlEncryption
{
  internal class EncryptedDataElement : EncryptedTypeElement
  {
    public static bool CanReadFrom(XmlReader reader) => reader != null && reader.IsStartElement("EncryptedData", "http://www.w3.org/2001/04/xmlenc#");

    public EncryptedDataElement()
      : this((SecurityTokenSerializer) null)
    {
    }

    public EncryptedDataElement(SecurityTokenSerializer tokenSerializer)
      : base(tokenSerializer)
      => this.KeyIdentifier = new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[1]
      {
        (SecurityKeyIdentifierClause) new Microsoft.IdentityModel.Protocols.XmlSignature.EmptySecurityKeyIdentifierClause()
      });

    public byte[] Decrypt(SymmetricAlgorithm algorithm)
    {
      if (algorithm == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (algorithm));
      byte[] cipherText = this.CipherData != null && this.CipherData.CipherValue != null ? this.CipherData.CipherValue : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID6000")));
      return EncryptedDataElement.ExtractIVAndDecrypt(algorithm, cipherText, 0, cipherText.Length);
    }

    public void Encrypt(SymmetricAlgorithm algorithm, byte[] buffer, int offset, int length)
    {
      byte[] iv;
      byte[] cipherText;
      EncryptedDataElement.GenerateIVAndEncrypt(algorithm, buffer, offset, length, out iv, out cipherText);
      this.CipherData.SetCipherValueFragments(iv, cipherText);
    }

    private static byte[] ExtractIVAndDecrypt(
      SymmetricAlgorithm algorithm,
      byte[] cipherText,
      int offset,
      int count)
    {
      byte[] rgbIV = new byte[algorithm.BlockSize / 8];
      if (cipherText.Length - offset < rgbIV.Length)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID6019", (object) (cipherText.Length - offset), (object) rgbIV.Length)));
      Buffer.BlockCopy((Array) cipherText, offset, (Array) rgbIV, 0, rgbIV.Length);
      algorithm.Padding = PaddingMode.ISO10126;
      algorithm.Mode = CipherMode.CBC;
      ICryptoTransform cryptoTransform = (ICryptoTransform) null;
      try
      {
        cryptoTransform = algorithm.CreateDecryptor(algorithm.Key, rgbIV);
        return cryptoTransform.TransformFinalBlock(cipherText, offset + rgbIV.Length, count - rgbIV.Length);
      }
      finally
      {
        cryptoTransform?.Dispose();
      }
    }

    private static void GenerateIVAndEncrypt(
      SymmetricAlgorithm algorithm,
      byte[] plainText,
      int offset,
      int length,
      out byte[] iv,
      out byte[] cipherText)
    {
      RandomNumberGenerator randomNumberGenerator = CryptoUtil.Algorithms.NewRandomNumberGenerator();
      int length1 = algorithm.BlockSize / 8;
      iv = new byte[length1];
      randomNumberGenerator.GetBytes(iv);
      algorithm.Padding = PaddingMode.PKCS7;
      algorithm.Mode = CipherMode.CBC;
      ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, iv);
      cipherText = encryptor.TransformFinalBlock(plainText, offset, length);
      encryptor.Dispose();
    }

    public override void ReadExtensions(XmlDictionaryReader reader)
    {
    }

    public override void ReadXml(XmlDictionaryReader reader)
    {
      int num = reader != null ? (int) reader.MoveToContent() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("EncryptedData", "http://www.w3.org/2001/04/xmlenc#"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader, Microsoft.IdentityModel.SR.GetString("ID4193"));
      base.ReadXml(reader);
    }

    public virtual void WriteXml(XmlWriter writer, SecurityTokenSerializer securityTokenSerializer)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (securityTokenSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenSerializer));
      if (this.KeyIdentifier == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID6001")));
      writer.WriteStartElement("xenc", "EncryptedData", "http://www.w3.org/2001/04/xmlenc#");
      if (!string.IsNullOrEmpty(this.Id))
        writer.WriteAttributeString("Id", (string) null, this.Id);
      if (!string.IsNullOrEmpty(this.Type))
        writer.WriteAttributeString("Type", (string) null, this.Type);
      if (this.EncryptionMethod != null)
        this.EncryptionMethod.WriteXml(writer);
      if (this.KeyIdentifier != null)
        securityTokenSerializer.WriteKeyIdentifier((XmlWriter) XmlDictionaryWriter.CreateDictionaryWriter(writer), this.KeyIdentifier);
      this.CipherData.WriteXml(writer);
      writer.WriteEndElement();
    }
  }
}
