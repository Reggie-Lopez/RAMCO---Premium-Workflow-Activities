// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.X509DataSecurityKeyIdentifierClauseSerializer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class X509DataSecurityKeyIdentifierClauseSerializer : SecurityKeyIdentifierClauseSerializer
  {
    public override bool CanReadKeyIdentifierClause(XmlReader reader) => reader != null ? reader.IsStartElement("X509Data", "http://www.w3.org/2000/09/xmldsig#") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));

    public override bool CanWriteKeyIdentifierClause(
      SecurityKeyIdentifierClause securityKeyIdentifierClause)
    {
      switch (securityKeyIdentifierClause)
      {
        case null:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityKeyIdentifierClause));
        case X509IssuerSerialKeyIdentifierClause _:
        case X509RawDataKeyIdentifierClause _:
          return true;
        default:
          return securityKeyIdentifierClause is X509SubjectKeyIdentifierClause;
      }
    }

    public override SecurityKeyIdentifierClause ReadKeyIdentifierClause(
      XmlReader reader)
    {
      XmlDictionaryReader dictionaryReader = reader != null ? XmlDictionaryReader.CreateDictionaryReader(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!dictionaryReader.IsStartElement("X509Data", "http://www.w3.org/2000/09/xmldsig#"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3032", (object) reader.LocalName, (object) reader.NamespaceURI, (object) "X509Data", (object) "http://www.w3.org/2000/09/xmldsig#"));
      dictionaryReader.ReadStartElement("X509Data", "http://www.w3.org/2000/09/xmldsig#");
      SecurityKeyIdentifierClause identifierClause;
      if (dictionaryReader.IsStartElement("X509IssuerSerial", "http://www.w3.org/2000/09/xmldsig#"))
      {
        dictionaryReader.ReadStartElement("X509IssuerSerial", "http://www.w3.org/2000/09/xmldsig#");
        string issuerName = dictionaryReader.IsStartElement("X509IssuerName", "http://www.w3.org/2000/09/xmldsig#") ? dictionaryReader.ReadElementContentAsString("X509IssuerName", "http://www.w3.org/2000/09/xmldsig#") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3032", (object) reader.LocalName, (object) reader.NamespaceURI, (object) "X509IssuerName", (object) "http://www.w3.org/2000/09/xmldsig#"));
        string issuerSerialNumber = dictionaryReader.IsStartElement("X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#") ? dictionaryReader.ReadElementContentAsString("X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3032", (object) reader.LocalName, (object) reader.NamespaceURI, (object) "X509SerialNumber", (object) "http://www.w3.org/2000/09/xmldsig#"));
        dictionaryReader.ReadEndElement();
        identifierClause = (SecurityKeyIdentifierClause) new X509IssuerSerialKeyIdentifierClause(issuerName, issuerSerialNumber);
      }
      else if (dictionaryReader.IsStartElement("X509SKI", "http://www.w3.org/2000/09/xmldsig#"))
      {
        byte[] ski = dictionaryReader.ReadElementContentAsBase64();
        identifierClause = ski != null && ski.Length != 0 ? (SecurityKeyIdentifierClause) new X509SubjectKeyIdentifierClause(ski) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4258", (object) "X509SKI", (object) "http://www.w3.org/2000/09/xmldsig#"));
      }
      else if (dictionaryReader.IsStartElement("X509Certificate", "http://www.w3.org/2000/09/xmldsig#"))
      {
        byte[] certificateRawData = (byte[]) null;
        while (reader.IsStartElement("X509Certificate", "http://www.w3.org/2000/09/xmldsig#"))
        {
          if (certificateRawData == null)
          {
            certificateRawData = dictionaryReader.ReadElementContentAsBase64();
            if (certificateRawData == null || certificateRawData.Length == 0)
              throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4258", (object) "X509Certificate", (object) "http://www.w3.org/2000/09/xmldsig#"));
          }
          else
            reader.Skip();
        }
        identifierClause = (SecurityKeyIdentifierClause) new X509RawDataKeyIdentifierClause(certificateRawData);
      }
      else
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4260", (object) dictionaryReader.LocalName, (object) dictionaryReader.NamespaceURI));
      dictionaryReader.ReadEndElement();
      return identifierClause;
    }

    public override void WriteKeyIdentifierClause(
      XmlWriter writer,
      SecurityKeyIdentifierClause securityKeyIdentifierClause)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      switch (securityKeyIdentifierClause)
      {
        case null:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityKeyIdentifierClause));
        case X509IssuerSerialKeyIdentifierClause identifierClause:
          writer.WriteStartElement("ds", "X509Data", "http://www.w3.org/2000/09/xmldsig#");
          writer.WriteStartElement("ds", "X509IssuerSerial", "http://www.w3.org/2000/09/xmldsig#");
          writer.WriteElementString("ds", "X509IssuerName", "http://www.w3.org/2000/09/xmldsig#", identifierClause.IssuerName);
          writer.WriteElementString("ds", "X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#", identifierClause.IssuerSerialNumber);
          writer.WriteEndElement();
          writer.WriteEndElement();
          break;
        case X509SubjectKeyIdentifierClause identifierClause:
          writer.WriteStartElement("ds", "X509Data", "http://www.w3.org/2000/09/xmldsig#");
          writer.WriteStartElement("ds", "X509SKI", "http://www.w3.org/2000/09/xmldsig#");
          byte[] subjectKeyIdentifier = identifierClause.GetX509SubjectKeyIdentifier();
          writer.WriteBase64(subjectKeyIdentifier, 0, subjectKeyIdentifier.Length);
          writer.WriteEndElement();
          writer.WriteEndElement();
          break;
        case X509RawDataKeyIdentifierClause identifierClause:
          writer.WriteStartElement("ds", "X509Data", "http://www.w3.org/2000/09/xmldsig#");
          writer.WriteStartElement("ds", "X509Certificate", "http://www.w3.org/2000/09/xmldsig#");
          byte[] x509RawData = identifierClause.GetX509RawData();
          writer.WriteBase64(x509RawData, 0, x509RawData.Length);
          writer.WriteEndElement();
          writer.WriteEndElement();
          break;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument(nameof (securityKeyIdentifierClause), SR.GetString("ID4259", (object) securityKeyIdentifierClause.GetType()));
      }
    }
  }
}
