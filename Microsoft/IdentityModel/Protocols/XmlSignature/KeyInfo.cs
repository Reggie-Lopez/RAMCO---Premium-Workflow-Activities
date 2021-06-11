// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.KeyInfo
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  internal class KeyInfo
  {
    private SecurityTokenSerializer _keyInfoSerializer;
    private SecurityKeyIdentifier _ski;
    private string _retrieval;

    public KeyInfo(SecurityTokenSerializer keyInfoSerializer)
    {
      this._keyInfoSerializer = keyInfoSerializer;
      this._ski = new SecurityKeyIdentifier();
    }

    public string RetrievalMethod => this._retrieval;

    public SecurityKeyIdentifier KeyIdentifier
    {
      get => this._ski;
      set => this._ski = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public virtual void ReadXml(XmlDictionaryReader reader)
    {
      int num = reader != null ? (int) reader.MoveToContent() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement(nameof (KeyInfo), "http://www.w3.org/2000/09/xmldsig#"))
        return;
      reader.ReadStartElement();
      while (reader.IsStartElement())
      {
        if (reader.IsStartElement("RetrievalMethod", "http://www.w3.org/2000/09/xmldsig#"))
        {
          string attribute = reader.GetAttribute("URI");
          if (!string.IsNullOrEmpty(attribute))
            this._retrieval = attribute;
          reader.Skip();
        }
        else if (this._keyInfoSerializer.CanReadKeyIdentifierClause((XmlReader) reader))
          this._ski.Add(this._keyInfoSerializer.ReadKeyIdentifierClause((XmlReader) reader));
        else if (reader.IsStartElement())
        {
          string str = reader.ReadOuterXml();
          if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
            DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8023", (object) reader.Name, (object) reader.NamespaceURI, (object) str));
        }
        int content = (int) reader.MoveToContent();
      }
      int content1 = (int) reader.MoveToContent();
      reader.ReadEndElement();
    }
  }
}
