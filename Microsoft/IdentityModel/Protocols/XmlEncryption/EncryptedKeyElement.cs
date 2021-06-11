// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlEncryption.EncryptedKeyElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlEncryption
{
  internal class EncryptedKeyElement : EncryptedTypeElement
  {
    private string _carriedName;
    private string _recipient;
    private List<string> _keyReferences;
    private List<string> _dataReferences;

    public EncryptedKeyElement(SecurityTokenSerializer keyInfoSerializer)
      : base(keyInfoSerializer)
    {
      this._keyReferences = new List<string>();
      this._dataReferences = new List<string>();
    }

    public string CarriedName => this._carriedName;

    public IList<string> DataReferences => (IList<string>) this._dataReferences;

    public IList<string> KeyReferences => (IList<string>) this._keyReferences;

    public override void ReadExtensions(XmlDictionaryReader reader)
    {
      int content1 = (int) reader.MoveToContent();
      if (!reader.IsStartElement("ReferenceList", "http://www.w3.org/2001/04/xmlenc#"))
        return;
      reader.ReadStartElement();
      if (reader.IsStartElement("DataReference", "http://www.w3.org/2001/04/xmlenc#"))
      {
        while (reader.IsStartElement())
        {
          if (reader.IsStartElement("DataReference", "http://www.w3.org/2001/04/xmlenc#"))
          {
            string attribute = reader.GetAttribute("URI");
            if (!string.IsNullOrEmpty(attribute))
              this._dataReferences.Add(attribute);
            reader.Skip();
          }
          else
          {
            string str = !reader.IsStartElement("KeyReference", "http://www.w3.org/2001/04/xmlenc#") ? reader.ReadOuterXml() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader, SR.GetString("ID4189"));
            if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
              DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8024", (object) reader.Name, (object) reader.NamespaceURI, (object) str));
          }
        }
      }
      else
      {
        if (!reader.IsStartElement("KeyReference", "http://www.w3.org/2001/04/xmlenc#"))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader, SR.GetString("ID4191"));
        while (reader.IsStartElement())
        {
          if (reader.IsStartElement("KeyReference", "http://www.w3.org/2001/04/xmlenc#"))
          {
            string attribute = reader.GetAttribute("URI");
            if (!string.IsNullOrEmpty(attribute))
              this._keyReferences.Add(attribute);
            reader.Skip();
          }
          else
          {
            string str = !reader.IsStartElement("DataReference", "http://www.w3.org/2001/04/xmlenc#") ? reader.ReadOuterXml() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader, SR.GetString("ID4190"));
            if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
              DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8024", (object) reader.Name, (object) reader.NamespaceURI, (object) str));
          }
        }
      }
      int content2 = (int) reader.MoveToContent();
      if (reader.IsStartElement("CarriedKeyName", "http://www.w3.org/2001/04/xmlenc#"))
      {
        reader.ReadStartElement();
        this._carriedName = reader.ReadString();
        reader.ReadEndElement();
      }
      reader.ReadEndElement();
    }

    public override void ReadXml(XmlDictionaryReader reader)
    {
      int num = reader != null ? (int) reader.MoveToContent() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      this._recipient = reader.IsStartElement("EncryptedKey", "http://www.w3.org/2001/04/xmlenc#") ? reader.GetAttribute("Recipient", (string) null) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader, SR.GetString("ID4187"));
      base.ReadXml(reader);
    }

    public EncryptedKeyIdentifierClause GetClause() => new EncryptedKeyIdentifierClause(this.CipherData.CipherValue, this.Algorithm, this.KeyIdentifier);
  }
}
