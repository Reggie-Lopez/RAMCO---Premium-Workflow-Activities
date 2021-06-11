// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlEncryption.EncryptionMethodElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Diagnostics;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlEncryption
{
  internal class EncryptionMethodElement
  {
    private string _algorithm;
    private string _parameters;

    public string Algorithm
    {
      get => this._algorithm;
      set => this._algorithm = value;
    }

    public string Parameters
    {
      get => this._parameters;
      set => this._parameters = value;
    }

    public void ReadXml(XmlDictionaryReader reader)
    {
      int num = reader != null ? (int) reader.MoveToContent() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("EncryptionMethod", "http://www.w3.org/2001/04/xmlenc#"))
        return;
      this._algorithm = reader.GetAttribute("Algorithm", (string) null);
      if (!reader.IsEmptyElement)
      {
        string str = reader.ReadOuterXml();
        if (!DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
          return;
        DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8024", (object) reader.Name, (object) reader.NamespaceURI, (object) str));
      }
      else
        reader.Read();
    }

    public void WriteXml(XmlWriter writer)
    {
      writer.WriteStartElement("xenc", "EncryptionMethod", "http://www.w3.org/2001/04/xmlenc#");
      writer.WriteAttributeString("Algorithm", (string) null, this._algorithm);
      writer.WriteEndElement();
    }
  }
}
