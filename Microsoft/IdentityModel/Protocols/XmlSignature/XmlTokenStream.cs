// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.XmlTokenStream
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  internal sealed class XmlTokenStream
  {
    private int _count;
    private XmlTokenStream.XmlTokenEntry[] _entries;
    private string _excludedElement;
    private int? _excludedElementDepth;
    private string _excludedElementNamespace;

    public XmlTokenStream(int initialSize) => this._entries = initialSize >= 1 ? new XmlTokenStream.XmlTokenEntry[initialSize] : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentOutOfRangeException(nameof (initialSize), Microsoft.IdentityModel.SR.GetString("ID0002")));

    public XmlTokenStream(XmlTokenStream other)
    {
      this._count = other._count;
      this._excludedElement = other._excludedElement;
      this._excludedElementDepth = other._excludedElementDepth;
      this._excludedElementNamespace = other._excludedElementNamespace;
      this._entries = new XmlTokenStream.XmlTokenEntry[this._count];
      Array.Copy((Array) other._entries, (Array) this._entries, this._count);
    }

    public void Add(XmlNodeType nodeType, string value)
    {
      this.EnsureCapacityToAdd();
      this._entries[this._count++].Set(nodeType, value);
    }

    public void AddAttribute(string prefix, string localName, string namespaceUri, string value)
    {
      this.EnsureCapacityToAdd();
      this._entries[this._count++].SetAttribute(prefix, localName, namespaceUri, value);
    }

    public void AddElement(
      string prefix,
      string localName,
      string namespaceUri,
      bool isEmptyElement)
    {
      this.EnsureCapacityToAdd();
      this._entries[this._count++].SetElement(prefix, localName, namespaceUri, isEmptyElement);
    }

    private void EnsureCapacityToAdd()
    {
      if (this._count != this._entries.Length)
        return;
      XmlTokenStream.XmlTokenEntry[] xmlTokenEntryArray = new XmlTokenStream.XmlTokenEntry[this._entries.Length * 3 / 2];
      Array.Copy((Array) this._entries, 0, (Array) xmlTokenEntryArray, 0, this._count);
      this._entries = xmlTokenEntryArray;
    }

    public void SetElementExclusion(string excludedElement, string excludedElementNamespace) => this.SetElementExclusion(excludedElement, excludedElementNamespace, new int?());

    public void SetElementExclusion(
      string excludedElement,
      string excludedElementNamespace,
      int? excludedElementDepth)
    {
      this._excludedElement = excludedElement;
      this._excludedElementDepth = excludedElementDepth;
      this._excludedElementNamespace = excludedElementNamespace;
    }

    public XmlTokenStream Trim() => new XmlTokenStream(this);

    public XmlTokenStream.XmlTokenStreamWriter GetWriter() => new XmlTokenStream.XmlTokenStreamWriter(this._entries, this._count, this._excludedElement, this._excludedElementDepth, this._excludedElementNamespace);

    internal class XmlTokenStreamWriter
    {
      private XmlTokenStream.XmlTokenEntry[] _entries;
      private int _count;
      private int _position;
      private string _excludedElement;
      private int? _excludedElementDepth;
      private string _excludedElementNamespace;

      public XmlTokenStreamWriter(
        XmlTokenStream.XmlTokenEntry[] entries,
        int count,
        string excludedElement,
        int? excludedElementDepth,
        string excludedElementNamespace)
      {
        this._entries = entries != null ? entries : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (entries));
        this._count = count;
        this._excludedElement = excludedElement;
        this._excludedElementDepth = excludedElementDepth;
        this._excludedElementNamespace = excludedElementNamespace;
      }

      public int Count => this._count;

      public int Position => this._position;

      public XmlNodeType NodeType => this._entries[this._position]._nodeType;

      public bool IsEmptyElement => this._entries[this._position].IsEmptyElement;

      public string Prefix => this._entries[this._position]._prefix;

      public string LocalName => this._entries[this._position]._localName;

      public string NamespaceUri => this._entries[this._position]._namespaceUri;

      public string Value => this._entries[this._position].Value;

      public string ExcludedElement => this._excludedElement;

      public string ExcludedElementNamespace => this._excludedElementNamespace;

      public bool MoveToFirst()
      {
        this._position = 0;
        return this._count > 0;
      }

      public bool MoveToFirstAttribute()
      {
        if (this._position >= this._count - 1 || this._entries[this._position + 1]._nodeType != XmlNodeType.Attribute)
          return false;
        ++this._position;
        return true;
      }

      public bool MoveToNext()
      {
        if (this._position >= this._count - 1)
          return false;
        ++this._position;
        return true;
      }

      public bool MoveToNextAttribute()
      {
        if (this._position >= this._count - 1 || this._entries[this._position + 1]._nodeType != XmlNodeType.Attribute)
          return false;
        ++this._position;
        return true;
      }

      public void WriteTo(XmlDictionaryWriter writer)
      {
        if (writer == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (writer));
        if (!this.MoveToFirst())
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID6025")));
        int num1 = 0;
        int num2 = -1;
        bool flag = true;
        do
        {
          switch (this.NodeType)
          {
            case XmlNodeType.Element:
              bool isEmptyElement = this.IsEmptyElement;
              ++num1;
              if (flag)
              {
                if (this._excludedElementDepth.HasValue)
                {
                  int? excludedElementDepth = this._excludedElementDepth;
                  int num3 = num1 - 1;
                  if ((excludedElementDepth.GetValueOrDefault() != num3 ? 0 : (excludedElementDepth.HasValue ? 1 : 0)) == 0)
                    goto label_12;
                }
                if (this.LocalName == this._excludedElement && this.NamespaceUri == this._excludedElementNamespace)
                {
                  flag = false;
                  num2 = num1;
                }
              }
label_12:
              if (flag)
                writer.WriteStartElement(this.Prefix, this.LocalName, this.NamespaceUri);
              if (this.MoveToFirstAttribute())
              {
                do
                {
                  if (flag)
                    writer.WriteAttributeString(this.Prefix, this.LocalName, this.NamespaceUri, this.Value);
                }
                while (this.MoveToNextAttribute());
              }
              if (!isEmptyElement)
                break;
              goto case XmlNodeType.EndElement;
            case XmlNodeType.Text:
              if (flag)
              {
                writer.WriteString(this.Value);
                break;
              }
              break;
            case XmlNodeType.CDATA:
              if (flag)
              {
                writer.WriteCData(this.Value);
                break;
              }
              break;
            case XmlNodeType.Comment:
              if (flag)
              {
                writer.WriteComment(this.Value);
                break;
              }
              break;
            case XmlNodeType.Whitespace:
            case XmlNodeType.SignificantWhitespace:
              if (flag)
              {
                writer.WriteWhitespace(this.Value);
                break;
              }
              break;
            case XmlNodeType.EndElement:
              if (flag)
                writer.WriteEndElement();
              else if (num2 == num1)
              {
                flag = true;
                num2 = -1;
              }
              --num1;
              break;
          }
        }
        while (this.MoveToNext());
      }
    }

    internal struct XmlTokenEntry
    {
      internal XmlNodeType _nodeType;
      internal string _prefix;
      internal string _localName;
      internal string _namespaceUri;
      private string _value;

      public bool IsEmptyElement
      {
        get => this._value == null;
        set => this._value = value ? (string) null : "";
      }

      public string Value => this._value;

      public void Set(XmlNodeType nodeType, string value)
      {
        this._nodeType = nodeType;
        this._value = value;
      }

      public void SetAttribute(string prefix, string localName, string namespaceUri, string value)
      {
        this._nodeType = XmlNodeType.Attribute;
        this._prefix = prefix;
        this._localName = localName;
        this._namespaceUri = namespaceUri;
        this._value = value;
      }

      public void SetElement(
        string prefix,
        string localName,
        string namespaceUri,
        bool isEmptyElement)
      {
        this._nodeType = XmlNodeType.Element;
        this._prefix = prefix;
        this._localName = localName;
        this._namespaceUri = namespaceUri;
        this.IsEmptyElement = isEmptyElement;
      }
    }
  }
}
