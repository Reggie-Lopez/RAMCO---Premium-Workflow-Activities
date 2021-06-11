// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlEncryption.XmlEncryptionConstants
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.XmlEncryption
{
  [ComVisible(true)]
  public static class XmlEncryptionConstants
  {
    public const string Namespace = "http://www.w3.org/2001/04/xmlenc#";
    public const string Prefix = "xenc";

    public static class Algorithms
    {
      public const string DesCbc = "http://www.w3.org/2001/04/xmlenc#des-cbc";
      public const string TripleDesCbc = "http://www.w3.org/2001/04/xmlenc#tripledes-cbc";
      public const string Aes128Cbc = "http://www.w3.org/2001/04/xmlenc#aes128-cbc";
      public const string Aes256Cbc = "http://www.w3.org/2001/04/xmlenc#aes256-cbc";
      public const string Aes192Cbc = "http://www.w3.org/2001/04/xmlenc#aes192-cbc";
      public const string Rsa15 = "http://www.w3.org/2001/04/xmlenc#rsa-1_5";
      public const string RsaOaepMgf1p = "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p";
      public const string TripleDesKeyWrap = "http://www.w3.org/2001/04/xmlenc#kw-tripledes";
      public const string Aes128KeyWrap = "http://www.w3.org/2001/04/xmlenc#kw-aes128";
      public const string Aes256KeyWrap = "http://www.w3.org/2001/04/xmlenc#kw-aes256";
      public const string Aes192KeyWrap = "http://www.w3.org/2001/04/xmlenc#kw-aes192";
      public const string Sha256 = "http://www.w3.org/2001/04/xmlenc#sha256";
      public const string Sha512 = "http://www.w3.org/2001/04/xmlenc#sha512";
    }

    public static class Attributes
    {
      public const string Algorithm = "Algorithm";
      public const string Encoding = "Encoding";
      public const string Id = "Id";
      public const string MimeType = "MimeType";
      public const string Recipient = "Recipient";
      public const string Type = "Type";
      public const string Uri = "URI";
    }

    public static class Elements
    {
      public const string CarriedKeyName = "CarriedKeyName";
      public const string CipherData = "CipherData";
      public const string CipherReference = "CiperReference";
      public const string CipherValue = "CipherValue";
      public const string DataReference = "DataReference";
      public const string EncryptedData = "EncryptedData";
      public const string EncryptedKey = "EncryptedKey";
      public const string EncryptionMethod = "EncryptionMethod";
      public const string EncryptionProperties = "EncryptionProperties";
      public const string KeyReference = "KeyReference";
      public const string KeySize = "KeySize";
      public const string OaepParams = "OAEPparams";
      public const string Recipient = "Recipient";
      public const string ReferenceList = "ReferenceList";
    }

    public static class EncryptedDataTypes
    {
      public const string Element = "http://www.w3.org/2001/04/xmlenc#Element";
      public const string Content = "http://www.w3.org/2001/04/xmlenc#Content";
    }
  }
}
