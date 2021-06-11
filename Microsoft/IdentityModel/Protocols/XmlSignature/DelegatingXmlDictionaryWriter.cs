// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.DelegatingXmlDictionaryWriter
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  [ComVisible(true)]
  public class DelegatingXmlDictionaryWriter : XmlDictionaryWriter
  {
    private XmlDictionaryWriter _innerWriter;
    private XmlWriter _tracingWriter;
    public static AsymmetricSignatureOperatorsDelegate GetAsymmetricSignatureOperators;

    protected DelegatingXmlDictionaryWriter()
    {
    }

    protected void InitializeInnerWriter(XmlDictionaryWriter innerWriter) => this._innerWriter = innerWriter != null ? innerWriter : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (innerWriter));

    protected void InitializeTracingWriter(XmlWriter tracingWriter) => this._tracingWriter = tracingWriter;

    protected XmlDictionaryWriter InnerWriter => this._innerWriter;

    public override void Close()
    {
      this._innerWriter.Close();
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.Close();
    }

    public override void Flush()
    {
      this._innerWriter.Flush();
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.Flush();
    }

    public override void WriteBase64(byte[] buffer, int index, int count)
    {
      this._innerWriter.WriteBase64(buffer, index, count);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteBase64(buffer, index, count);
    }

    public override void WriteCData(string text)
    {
      this._innerWriter.WriteCData(text);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteCData(text);
    }

    public override void WriteCharEntity(char ch)
    {
      this._innerWriter.WriteCharEntity(ch);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteCharEntity(ch);
    }

    public override void WriteChars(char[] buffer, int index, int count)
    {
      this._innerWriter.WriteChars(buffer, index, count);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteChars(buffer, index, count);
    }

    public override void WriteComment(string text)
    {
      this._innerWriter.WriteComment(text);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteComment(text);
    }

    public override void WriteDocType(string name, string pubid, string sysid, string subset)
    {
      this._innerWriter.WriteDocType(name, pubid, sysid, subset);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteDocType(name, pubid, sysid, subset);
    }

    public override void WriteEndAttribute()
    {
      this._innerWriter.WriteEndAttribute();
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteEndAttribute();
    }

    public override void WriteEndDocument()
    {
      this._innerWriter.WriteEndDocument();
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteEndDocument();
    }

    public override void WriteEndElement()
    {
      this._innerWriter.WriteEndElement();
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteEndElement();
    }

    public override void WriteEntityRef(string name)
    {
      this._innerWriter.WriteEntityRef(name);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteEntityRef(name);
    }

    public override void WriteFullEndElement()
    {
      this._innerWriter.WriteFullEndElement();
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteFullEndElement();
    }

    public override void WriteProcessingInstruction(string name, string text)
    {
      this._innerWriter.WriteProcessingInstruction(name, text);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteProcessingInstruction(name, text);
    }

    public override void WriteRaw(char[] buffer, int index, int count)
    {
      this._innerWriter.WriteRaw(buffer, index, count);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteRaw(buffer, index, count);
    }

    public override void WriteRaw(string data)
    {
      this._innerWriter.WriteRaw(data);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteRaw(data);
    }

    public override void WriteStartAttribute(string prefix, string localName, string ns)
    {
      this._innerWriter.WriteStartAttribute(prefix, localName, ns);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteStartAttribute(prefix, localName, ns);
    }

    public override void WriteStartDocument()
    {
      this._innerWriter.WriteStartDocument();
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteStartDocument();
    }

    public override void WriteStartDocument(bool standalone)
    {
      this._innerWriter.WriteStartDocument(standalone);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteStartDocument(standalone);
    }

    public override void WriteStartElement(string prefix, string localName, string ns)
    {
      this._innerWriter.WriteStartElement(prefix, localName, ns);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteStartElement(prefix, localName, ns);
    }

    public override WriteState WriteState => this._innerWriter.WriteState;

    public override void WriteString(string text)
    {
      this._innerWriter.WriteString(text);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteString(text);
    }

    public override void WriteSurrogateCharEntity(char lowChar, char highChar)
    {
      this._innerWriter.WriteSurrogateCharEntity(lowChar, highChar);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteSurrogateCharEntity(lowChar, highChar);
    }

    public override void WriteWhitespace(string ws)
    {
      this._innerWriter.WriteWhitespace(ws);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteWhitespace(ws);
    }

    public override void WriteXmlAttribute(string localName, string value)
    {
      this._innerWriter.WriteXmlAttribute(localName, value);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteAttributeString(localName, value);
    }

    public override void WriteXmlnsAttribute(string prefix, string namespaceUri)
    {
      this._innerWriter.WriteXmlnsAttribute(prefix, namespaceUri);
      if (this._tracingWriter == null)
        return;
      this._tracingWriter.WriteAttributeString(prefix, string.Empty, namespaceUri, string.Empty);
    }

    public override string LookupPrefix(string ns) => this._innerWriter.LookupPrefix(ns);

    public override bool CanCanonicalize => this._innerWriter.CanCanonicalize;

    public override void StartCanonicalization(
      Stream stream,
      bool includeComments,
      string[] inclusivePrefixes)
    {
      this._innerWriter.StartCanonicalization(stream, includeComments, inclusivePrefixes);
    }

    public override void EndCanonicalization() => this._innerWriter.EndCanonicalization();
  }
}
