// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Diagnostics.PlainXmlWriter
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IO;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
  internal class PlainXmlWriter : XmlWriter
  {
    private TraceXPathNavigator _navigator;
    private bool _writingAttribute;
    private string _currentAttributeName;
    private string _currentAttributePrefix;
    private string _currentAttributeNs;
    private string _currentAttributeText = string.Empty;

    public PlainXmlWriter() => this._navigator = new TraceXPathNavigator();

    public override void Close()
    {
    }

    public override void Flush()
    {
    }

    public override string LookupPrefix(string ns) => this._navigator.LookupPrefix(ns);

    public TraceXPathNavigator Navigator => this._navigator;

    public override void WriteBase64(byte[] buffer, int offset, int count) => this._navigator.AddText(Convert.ToBase64String(buffer, offset, count));

    public override void WriteCData(string text) => this.WriteRaw("<![CDATA[" + text + "]]>");

    public override void WriteCharEntity(char ch)
    {
    }

    public override void WriteChars(char[] buffer, int index, int count)
    {
      if (buffer == null || index < 0 || count < 0)
        return;
      this.WriteString(new string(buffer, index, count));
    }

    public override void WriteComment(string text) => this._navigator.AddComment(text);

    public static void WriteDecoded(string startElement, string wresult, XmlWriter writer)
    {
      writer.WriteStartElement(startElement);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(XmlReader.Create((TextReader) new StringReader(wresult), new XmlReaderSettings()
      {
        XmlResolver = (XmlResolver) null
      }));
      xmlDocument.CreateNavigator().WriteSubtree(writer);
      writer.WriteEndElement();
    }

    public override void WriteDocType(string name, string pubid, string sysid, string subset)
    {
    }

    public override void WriteEndAttribute()
    {
      if (!this._writingAttribute)
        return;
      this._navigator.AddAttribute(this._currentAttributeName, this._currentAttributeText, this._currentAttributeNs, this._currentAttributePrefix);
      this._writingAttribute = false;
    }

    public override void WriteEndElement() => this._navigator.CloseElement();

    public override void WriteEndDocument()
    {
    }

    public override void WriteEntityRef(string name)
    {
    }

    public override void WriteFullEndElement() => this.WriteEndElement();

    public override void WriteProcessingInstruction(string name, string text) => this._navigator.AddProcessingInstruction(name, text);

    public override void WriteRaw(string data) => this.WriteString(data);

    public override void WriteRaw(char[] buffer, int index, int count) => this.WriteChars(buffer, index, count);

    public override void WriteStartAttribute(string prefix, string localName, string ns)
    {
      if (this._writingAttribute)
        return;
      this._currentAttributeName = localName;
      this._currentAttributePrefix = prefix;
      this._currentAttributeNs = ns;
      this._currentAttributeText = string.Empty;
      this._writingAttribute = true;
    }

    public override void WriteStartDocument()
    {
    }

    public override void WriteStartDocument(bool standalone)
    {
    }

    public override void WriteStartElement(string prefix, string localName, string ns)
    {
      if (string.IsNullOrEmpty(localName))
        this._navigator.AddElement(prefix, nameof (localName), ns);
      else
        this._navigator.AddElement(prefix, localName, ns);
    }

    public override WriteState WriteState => this._navigator.WriteState;

    public override void WriteString(string text)
    {
      if (this._writingAttribute)
        this._currentAttributeText += text;
      else
        this.WriteValue(text);
    }

    public override void WriteSurrogateCharEntity(char lowChar, char highChar)
    {
    }

    public override void WriteValue(object value) => this._navigator.AddText(value.ToString());

    public override void WriteValue(string value) => this._navigator.AddText(value);

    public override void WriteWhitespace(string ws)
    {
    }

    public override string XmlLang => string.Empty;

    public override XmlSpace XmlSpace => XmlSpace.Default;
  }
}
