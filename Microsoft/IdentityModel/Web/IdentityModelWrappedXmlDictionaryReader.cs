// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Web.IdentityModelWrappedXmlDictionaryReader
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Xml;

namespace Microsoft.IdentityModel.Web
{
  internal class IdentityModelWrappedXmlDictionaryReader : XmlDictionaryReader, IXmlLineInfo
  {
    private XmlReader _reader;
    private XmlDictionaryReaderQuotas _xmlDictionaryReaderQuotas;

    public IdentityModelWrappedXmlDictionaryReader(
      XmlReader reader,
      XmlDictionaryReaderQuotas xmlDictionaryReaderQuotas)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (xmlDictionaryReaderQuotas == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (xmlDictionaryReaderQuotas));
      this._reader = reader;
      this._xmlDictionaryReaderQuotas = xmlDictionaryReaderQuotas;
    }

    public override int AttributeCount => this._reader.AttributeCount;

    public override string BaseURI => this._reader.BaseURI;

    public override bool CanReadBinaryContent => this._reader.CanReadBinaryContent;

    public override bool CanReadValueChunk => this._reader.CanReadValueChunk;

    public override void Close() => this._reader.Close();

    public override int Depth => this._reader.Depth;

    public override bool EOF => this._reader.EOF;

    public override string GetAttribute(int index) => this._reader.GetAttribute(index);

    public override string GetAttribute(string name) => this._reader.GetAttribute(name);

    public override string GetAttribute(string name, string namespaceUri) => this._reader.GetAttribute(name, namespaceUri);

    public override bool HasValue => this._reader.HasValue;

    public override bool IsDefault => this._reader.IsDefault;

    public override bool IsEmptyElement => this._reader.IsEmptyElement;

    public override bool IsStartElement(string name) => this._reader.IsStartElement(name);

    public override bool IsStartElement(string localName, string namespaceUri) => this._reader.IsStartElement(localName, namespaceUri);

    public override string LocalName => this._reader.LocalName;

    public override string LookupNamespace(string namespaceUri) => this._reader.LookupNamespace(namespaceUri);

    public override void MoveToAttribute(int index) => this._reader.MoveToAttribute(index);

    public override bool MoveToAttribute(string name) => this._reader.MoveToAttribute(name);

    public override bool MoveToAttribute(string name, string namespaceUri) => this._reader.MoveToAttribute(name, namespaceUri);

    public override bool MoveToElement() => this._reader.MoveToElement();

    public override bool MoveToFirstAttribute() => this._reader.MoveToFirstAttribute();

    public override bool MoveToNextAttribute() => this._reader.MoveToNextAttribute();

    public override string Name => this._reader.Name;

    public override string NamespaceURI => this._reader.NamespaceURI;

    public override XmlNameTable NameTable => this._reader.NameTable;

    public override XmlNodeType NodeType => this._reader.NodeType;

    public override string Prefix => this._reader.Prefix;

    public override char QuoteChar => this._reader.QuoteChar;

    public override bool Read() => this._reader.Read();

    public override bool ReadAttributeValue() => this._reader.ReadAttributeValue();

    public override string ReadElementString(string name) => this._reader.ReadElementString(name);

    public override string ReadElementString(string localName, string namespaceUri) => this._reader.ReadElementString(localName, namespaceUri);

    public override string ReadInnerXml() => this._reader.ReadInnerXml();

    public override string ReadOuterXml() => this._reader.ReadOuterXml();

    public override void ReadStartElement(string name) => this._reader.ReadStartElement(name);

    public override void ReadStartElement(string localName, string namespaceUri) => this._reader.ReadStartElement(localName, namespaceUri);

    public override void ReadEndElement() => this._reader.ReadEndElement();

    public override string ReadString() => this._reader.ReadString();

    public override ReadState ReadState => this._reader.ReadState;

    public override void ResolveEntity() => this._reader.ResolveEntity();

    public override string this[int index] => this._reader[index];

    public override string this[string name] => this._reader[name];

    public override string this[string name, string namespaceUri] => this._reader[name, namespaceUri];

    public override string Value => this._reader.Value;

    public override string XmlLang => this._reader.XmlLang;

    public override XmlSpace XmlSpace => this._reader.XmlSpace;

    public override int ReadElementContentAsBase64(byte[] buffer, int offset, int count) => this._reader.ReadElementContentAsBase64(buffer, offset, count);

    public override int ReadContentAsBase64(byte[] buffer, int offset, int count) => this._reader.ReadContentAsBase64(buffer, offset, count);

    public override int ReadElementContentAsBinHex(byte[] buffer, int offset, int count) => this._reader.ReadElementContentAsBinHex(buffer, offset, count);

    public override int ReadContentAsBinHex(byte[] buffer, int offset, int count) => this._reader.ReadContentAsBinHex(buffer, offset, count);

    public override int ReadValueChunk(char[] chars, int offset, int count) => this._reader.ReadValueChunk(chars, offset, count);

    public override Type ValueType => this._reader.ValueType;

    public override bool ReadContentAsBoolean() => this._reader.ReadContentAsBoolean();

    public override DateTime ReadContentAsDateTime() => this._reader.ReadContentAsDateTime();

    public override Decimal ReadContentAsDecimal() => (Decimal) this._reader.ReadContentAs(typeof (Decimal), (IXmlNamespaceResolver) null);

    public override double ReadContentAsDouble() => this._reader.ReadContentAsDouble();

    public override int ReadContentAsInt() => this._reader.ReadContentAsInt();

    public override long ReadContentAsLong() => this._reader.ReadContentAsLong();

    public override float ReadContentAsFloat() => this._reader.ReadContentAsFloat();

    public override string ReadContentAsString() => this._reader.ReadContentAsString();

    public override object ReadContentAs(Type valueType, IXmlNamespaceResolver namespaceResolver) => this._reader.ReadContentAs(valueType, namespaceResolver);

    public bool HasLineInfo() => this._reader is IXmlLineInfo reader && reader.HasLineInfo();

    public int LineNumber => !(this._reader is IXmlLineInfo reader) ? 1 : reader.LineNumber;

    public int LinePosition => !(this._reader is IXmlLineInfo reader) ? 1 : reader.LinePosition;

    public override XmlDictionaryReaderQuotas Quotas => this._xmlDictionaryReaderQuotas;
  }
}
