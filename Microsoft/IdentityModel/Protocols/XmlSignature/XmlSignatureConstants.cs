// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.XmlSignatureConstants
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  [ComVisible(true)]
  public static class XmlSignatureConstants
  {
    public const string Namespace = "http://www.w3.org/2000/09/xmldsig#";
    public const string Prefix = "ds";

    public static class Algorithms
    {
      public const string ExcC14N = "http://www.w3.org/2001/10/xml-exc-c14n#";
      public const string ExcC14NWithComments = "http://www.w3.org/2001/10/xml-exc-c14n#WithComments";
      public const string Sha1 = "http://www.w3.org/2000/09/xmldsig#sha1";
      public const string EnvelopedSignature = "http://www.w3.org/2000/09/xmldsig#enveloped-signature";
    }

    public static class Attributes
    {
      public const string Algorithm = "Algorithm";
      public const string Id = "Id";
      public const string Type = "Type";
      public const string URI = "URI";
    }

    public static class Elements
    {
      public const string CanonicalizationMethod = "CanonicalizationMethod";
      public const string DigestMethod = "DigestMethod";
      public const string DigestValue = "DigestValue";
      public const string Exponent = "Exponent";
      public const string KeyInfo = "KeyInfo";
      public const string KeyName = "KeyName";
      public const string KeyValue = "KeyValue";
      public const string Modulus = "Modulus";
      public const string Object = "Object";
      public const string Reference = "Reference";
      public const string RetrievalMethod = "RetrievalMethod";
      public const string RsaKeyValue = "RsaKeyValue";
      public const string Signature = "Signature";
      public const string SignatureMethod = "SignatureMethod";
      public const string SignatureValue = "SignatureValue";
      public const string SignedInfo = "SignedInfo";
      public const string Transform = "Transform";
      public const string Transforms = "Transforms";
      public const string X509Data = "X509Data";
      public const string X509IssuerName = "X509IssuerName";
      public const string X509IssuerSerial = "X509IssuerSerial";
      public const string X509SerialNumber = "X509SerialNumber";
      public const string X509SubjectName = "X509SubjectName";
      public const string X509Certificate = "X509Certificate";
      public const string X509SKI = "X509SKI";
    }
  }
}
