// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.EnvelopedSignatureTransform
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Security.Cryptography;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  internal sealed class EnvelopedSignatureTransform : Transform
  {
    private string _prefix = "ds";

    public override string Algorithm => "http://www.w3.org/2000/09/xmldsig#enveloped-signature";

    public override object Process(object input)
    {
      if (input is WrappedReader wrappedReader)
      {
        wrappedReader.XmlTokens.SetElementExclusion("Signature", "http://www.w3.org/2000/09/xmldsig#", new int?(1));
        return (object) wrappedReader;
      }
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID6004", (object) input.GetType())));
    }

    public override byte[] ProcessAndDigest(object input, string digestAlgorithm) => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID6027")));

    public override void ReadFrom(XmlDictionaryReader reader)
    {
      int content1 = (int) reader.MoveToContent();
      reader.MoveToStartElement("Transform", "http://www.w3.org/2000/09/xmldsig#");
      bool isEmptyElement = reader.IsEmptyElement;
      this._prefix = reader.Prefix;
      string attribute = reader.GetAttribute("Algorithm", (string) null);
      if (string.IsNullOrEmpty(attribute))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID0001", (object) "Algorithm", (object) reader.LocalName)));
      if (attribute != this.Algorithm)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6028", (object) attribute)));
      reader.Read();
      int content2 = (int) reader.MoveToContent();
      if (isEmptyElement)
        return;
      reader.ReadEndElement();
    }

    public override void WriteTo(XmlDictionaryWriter writer)
    {
      writer.WriteStartElement(this._prefix, "Transform", "http://www.w3.org/2000/09/xmldsig#");
      writer.WriteAttributeString("Algorithm", (string) null, this.Algorithm);
      writer.WriteEndElement();
    }
  }
}
