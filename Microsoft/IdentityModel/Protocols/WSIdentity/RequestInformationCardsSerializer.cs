// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.RequestInformationCardsSerializer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.WSTrust;
using System;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class RequestInformationCardsSerializer
  {
    private WSTrustSerializationContext _trustSerializationContext = new WSTrustSerializationContext();

    public RequestInformationCardsSerializer()
    {
    }

    public RequestInformationCardsSerializer(
      WSTrustSerializationContext trustSerializationContext)
    {
      this._trustSerializationContext = trustSerializationContext != null ? trustSerializationContext : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustSerializationContext));
    }

    public virtual RequestInformationCards ReadXml(XmlReader reader)
    {
      XmlDictionaryReader reader1 = reader != null ? XmlDictionaryReader.CreateDictionaryReader(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader1.IsStartElement("RequestInformationCards", "http://docs.oasis-open.org/imi/ns/identity-200903"))
        reader1.ReadStartElement("RequestInformationCards", "http://docs.oasis-open.org/imi/ns/identity-200903");
      RequestInformationCards informationCards = new RequestInformationCards();
      if (reader1.IsEmptyElement)
      {
        reader1.Skip();
        return informationCards;
      }
      reader1.ReadStartElement();
      while (reader1.IsStartElement())
      {
        if (reader1.IsStartElement("Issuer", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
          informationCards.Issuer = !(informationCards.Issuer != (Uri) null) ? RequestInformationCardsSerializer.ReadElementUri(reader1, "Issuer", "http://schemas.xmlsoap.org/ws/2005/05/identity") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader1, Microsoft.IdentityModel.SR.GetString("ID3280", (object) reader1.LocalName, (object) reader1.NamespaceURI));
        else if (reader1.IsStartElement("CardId", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
          informationCards.CardIdentifier = !(informationCards.CardIdentifier != (Uri) null) ? RequestInformationCardsSerializer.ReadElementUri(reader1, "CardId", "http://schemas.xmlsoap.org/ws/2005/05/identity") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader1, Microsoft.IdentityModel.SR.GetString("ID3280", (object) reader1.LocalName, (object) reader1.NamespaceURI));
        else if (reader1.IsStartElement("CardType", "http://docs.oasis-open.org/imi/ns/identity-200903"))
          informationCards.CardType = !(informationCards.CardType != (Uri) null) ? RequestInformationCardsSerializer.ReadElementUri(reader1, "CardType", "http://docs.oasis-open.org/imi/ns/identity-200903") : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader1, Microsoft.IdentityModel.SR.GetString("ID3280", (object) reader1.LocalName, (object) reader1.NamespaceURI));
        else if (reader1.IsStartElement("OnBehalfOf", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
        {
          if (informationCards.OnBehalfOf != null)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader1, Microsoft.IdentityModel.SR.GetString("ID3280", (object) reader1.LocalName, (object) reader1.NamespaceURI));
          informationCards.OnBehalfOf = new Microsoft.IdentityModel.Tokens.SecurityTokenElement(RequestInformationCardsSerializer.ReadInnerXml((XmlReader) reader1), this._trustSerializationContext.SecurityTokenHandlers);
        }
        else if (reader1.IsStartElement("CardSignatureFormat", "http://docs.oasis-open.org/imi/ns/identity-200903"))
          informationCards.CardSignatureFormat = informationCards.CardSignatureFormat == CardSignatureFormatType.None ? this.ReadCardSignatureFormat(reader1) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader1, Microsoft.IdentityModel.SR.GetString("ID3280", (object) reader1.LocalName, (object) reader1.NamespaceURI));
        else
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader1, Microsoft.IdentityModel.SR.GetString("ID3007", (object) reader1.LocalName, (object) reader1.NamespaceURI));
      }
      reader1.ReadEndElement();
      return informationCards;
    }

    public virtual void WriteXml(Stream stream, RequestInformationCards request)
    {
      if (stream == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (stream));
      using (XmlDictionaryWriter textWriter = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8, false))
        this.WriteXml((XmlWriter) textWriter, request);
    }

    public virtual void WriteXml(XmlWriter writer, RequestInformationCards request)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (request == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (request));
      XmlDictionaryWriter dictionaryWriter = XmlDictionaryWriter.CreateDictionaryWriter(writer);
      dictionaryWriter.WriteStartElement("ic09", "RequestInformationCards", "http://docs.oasis-open.org/imi/ns/identity-200903");
      if (request.Issuer != (Uri) null)
        dictionaryWriter.WriteElementString("i", "Issuer", "http://schemas.xmlsoap.org/ws/2005/05/identity", request.Issuer.OriginalString);
      if (request.CardIdentifier != (Uri) null)
        dictionaryWriter.WriteElementString("i", "CardId", "http://schemas.xmlsoap.org/ws/2005/05/identity", request.CardIdentifier.OriginalString);
      if (request.CardType != (Uri) null)
        dictionaryWriter.WriteElementString("ic09", "CardType", "http://docs.oasis-open.org/imi/ns/identity-200903", request.CardType.OriginalString);
      if (request.OnBehalfOf != null)
      {
        dictionaryWriter.WriteStartElement("trust", "OnBehalfOf", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
        RequestInformationCardsSerializer.WriteTokenElement(request.OnBehalfOf, "OnBehalfOf", this._trustSerializationContext, (XmlWriter) dictionaryWriter);
        dictionaryWriter.WriteEndElement();
      }
      dictionaryWriter.WriteElementString("ic09", "CardSignatureFormat", "http://docs.oasis-open.org/imi/ns/identity-200903", request.CardSignatureFormat.ToString());
      dictionaryWriter.WriteEndElement();
      writer.Flush();
    }

    private CardSignatureFormatType ReadCardSignatureFormat(
      XmlDictionaryReader reader)
    {
      string str = reader.ReadElementString("CardSignatureFormat", "http://docs.oasis-open.org/imi/ns/identity-200903");
      switch (str)
      {
        case "Enveloping":
          return CardSignatureFormatType.Enveloping;
        case "Enveloped":
          return CardSignatureFormatType.Enveloped;
        case "None":
          return CardSignatureFormatType.None;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader, Microsoft.IdentityModel.SR.GetString("ID3281", (object) str));
      }
    }

    private static Uri ReadElementUri(XmlDictionaryReader reader, string localName, string ns)
    {
      string uriString = reader.ReadElementString(localName, ns);
      return UriUtil.CanCreateValidUri(uriString, UriKind.Absolute) ? new Uri(uriString) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader, Microsoft.IdentityModel.SR.GetString("ID0014", (object) uriString));
    }

    private static XmlElement ReadInnerXml(XmlReader reader) => RequestInformationCardsSerializer.ReadInnerXml(reader, false);

    private static XmlElement ReadInnerXml(XmlReader reader, bool onStartElement)
    {
      string str = reader != null ? reader.LocalName : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      string namespaceUri = reader.NamespaceURI;
      if (reader.IsEmptyElement)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3061", (object) str, (object) namespaceUri)));
      if (!onStartElement)
        reader.ReadStartElement();
      int content = (int) reader.MoveToContent();
      XmlElement documentElement;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (XmlWriter textWriter = (XmlWriter) XmlDictionaryWriter.CreateTextWriter((Stream) memoryStream, Encoding.UTF8, false))
        {
          textWriter.WriteNode(reader, true);
          textWriter.Flush();
        }
        memoryStream.Seek(0L, SeekOrigin.Begin);
        if (memoryStream.Length == 0L)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new WSTrustSerializationException(Microsoft.IdentityModel.SR.GetString("ID3061", (object) str, (object) namespaceUri)));
        XmlDictionaryReader textReader = XmlDictionaryReader.CreateTextReader((Stream) memoryStream, Encoding.UTF8, XmlDictionaryReaderQuotas.Max, (OnXmlDictionaryReaderClose) null);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.PreserveWhitespace = true;
        xmlDocument.Load((XmlReader) textReader);
        documentElement = xmlDocument.DocumentElement;
      }
      if (!onStartElement)
        reader.ReadEndElement();
      return documentElement;
    }

    private static void WriteTokenElement(
      Microsoft.IdentityModel.Tokens.SecurityTokenElement tokenElement,
      string usage,
      WSTrustSerializationContext context,
      XmlWriter writer)
    {
      if (tokenElement.SecurityTokenXml != null)
      {
        tokenElement.SecurityTokenXml.WriteTo(writer);
      }
      else
      {
        Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection handlerCollection = !context.SecurityTokenHandlerCollectionManager.ContainsKey(usage) ? context.SecurityTokenHandlers : context.SecurityTokenHandlerCollectionManager[usage];
        SecurityToken securityToken = tokenElement.GetSecurityToken();
        bool flag = false;
        if (handlerCollection != null && handlerCollection.CanWriteToken(securityToken))
        {
          handlerCollection.WriteToken(writer, securityToken);
          flag = true;
        }
        if (flag)
          return;
        context.SecurityTokenSerializer.WriteToken(writer, securityToken);
      }
    }
  }
}
