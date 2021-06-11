// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSIdentity.RequestInformationCardsResponseSerializer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
  [ComVisible(true)]
  public class RequestInformationCardsResponseSerializer
  {
    public virtual RequestInformationCardsResponse ReadXml(
      XmlReader reader)
    {
      XmlDictionaryReader dictionaryReader = reader != null ? XmlDictionaryReader.CreateDictionaryReader(reader) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!dictionaryReader.IsStartElement("RequestInformationCardsResponse", "http://docs.oasis-open.org/imi/ns/identity-200903"))
        dictionaryReader.ReadStartElement("RequestInformationCardsResponse", "http://docs.oasis-open.org/imi/ns/identity-200903");
      RequestInformationCardsResponse informationCardsResponse = new RequestInformationCardsResponse();
      if (dictionaryReader.IsEmptyElement)
      {
        dictionaryReader.Skip();
        return informationCardsResponse;
      }
      dictionaryReader.ReadStartElement();
      while (dictionaryReader.IsStartElement())
      {
        string str = dictionaryReader.IsStartElement("InformationCard", "http://schemas.xmlsoap.org/ws/2005/05/identity") || dictionaryReader.IsStartElement("Signature", "http://www.w3.org/2000/09/xmldsig#") ? dictionaryReader.ReadOuterXml() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) dictionaryReader, SR.GetString("ID3007", (object) dictionaryReader.LocalName, (object) dictionaryReader.NamespaceURI));
        informationCardsResponse.InformationCards.Add(str);
      }
      dictionaryReader.ReadEndElement();
      return informationCardsResponse;
    }

    public virtual void WriteXml(Stream stream, RequestInformationCardsResponse response)
    {
      if (stream == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (stream));
      using (XmlDictionaryWriter textWriter = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8, false))
        this.WriteXml((XmlWriter) textWriter, response);
    }

    public virtual void WriteXml(XmlWriter writer, RequestInformationCardsResponse response)
    {
      if (writer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
      if (response == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
      XmlDictionaryWriter dictionaryWriter = XmlDictionaryWriter.CreateDictionaryWriter(writer);
      dictionaryWriter.WriteStartElement("ic09", "RequestInformationCardsResponse", "http://docs.oasis-open.org/imi/ns/identity-200903");
      foreach (string informationCard in response.InformationCards)
        dictionaryWriter.WriteRaw(informationCard);
      dictionaryWriter.WriteEndElement();
      writer.Flush();
    }
  }
}
