// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.DelegatingXmlDictionaryReader
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  [ComVisible(true)]
  public class DelegatingXmlDictionaryReader : XmlDictionaryReader
  {
    private XmlDictionaryReader _innerReader;

    protected DelegatingXmlDictionaryReader()
    {
    }

    protected void InitializeInnerReader(XmlDictionaryReader innerReader) => this._innerReader = innerReader != null ? innerReader : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (innerReader));

    protected XmlDictionaryReader InnerReader => this._innerReader;

    public override string this[int i] => this._innerReader[i];

    public override string this[string name] => this._innerReader[name];

    public override string this[string name, string namespaceURI] => this._innerReader[name, namespaceURI];

    public override int AttributeCount => this._innerReader.AttributeCount;

    public override string BaseURI => this._innerReader.BaseURI;

    public override int Depth => this._innerReader.Depth;

    public override bool EOF => this._innerReader.EOF;

    public override bool HasValue => this._innerReader.HasValue;

    public override bool IsDefault => this._innerReader.IsDefault;

    public override bool IsEmptyElement => this._innerReader.IsEmptyElement;

    public override string LocalName => this._innerReader.LocalName;

    public override string Name => this._innerReader.Name;

    public override string NamespaceURI => this._innerReader.NamespaceURI;

    public override XmlNameTable NameTable => this._innerReader.NameTable;

    public override XmlNodeType NodeType => this._innerReader.NodeType;

    public override string Prefix => this._innerReader.Prefix;

    public override char QuoteChar => this._innerReader.QuoteChar;

    public override ReadState ReadState => this._innerReader.ReadState;

    public override string Value => this._innerReader.Value;

    public override Type ValueType => this._innerReader.ValueType;

    public override string XmlLang => this._innerReader.XmlLang;

    public override XmlSpace XmlSpace => this._innerReader.XmlSpace;

    public override void Close() => this._innerReader.Close();

    public override string GetAttribute(int i) => this._innerReader.GetAttribute(i);

    public override string GetAttribute(string name) => this._innerReader.GetAttribute(name);

    public override string GetAttribute(string name, string namespaceURI) => this._innerReader.GetAttribute(name, namespaceURI);

    public override string LookupNamespace(string prefix) => this._innerReader.LookupNamespace(prefix);

    public override void MoveToAttribute(int i) => this._innerReader.MoveToAttribute(i);

    public override bool MoveToAttribute(string name) => this._innerReader.MoveToAttribute(name);

    public override bool MoveToAttribute(string name, string ns) => this._innerReader.MoveToAttribute(name, ns);

    public override bool MoveToElement() => this._innerReader.MoveToElement();

    public override bool MoveToFirstAttribute() => this._innerReader.MoveToFirstAttribute();

    public override bool MoveToNextAttribute() => this._innerReader.MoveToNextAttribute();

    public override bool Read() => this._innerReader.Read();

    public override bool ReadAttributeValue() => this._innerReader.ReadAttributeValue();

    public override int ReadContentAsBase64(byte[] buffer, int index, int count) => this._innerReader.ReadContentAsBase64(buffer, index, count);

    public override int ReadContentAsBinHex(byte[] buffer, int index, int count) => this._innerReader.ReadContentAsBinHex(buffer, index, count);

    public override System.Xml.UniqueId ReadContentAsUniqueId() => this._innerReader.ReadContentAsUniqueId();

    public override int ReadValueChunk(char[] buffer, int index, int count) => this._innerReader.ReadValueChunk(buffer, index, count);

    public override void ResolveEntity() => this._innerReader.ResolveEntity();
  }
}
