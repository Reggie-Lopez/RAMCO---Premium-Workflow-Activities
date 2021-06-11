// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.WrappedReader
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  internal sealed class WrappedReader : DelegatingXmlDictionaryReader, IXmlLineInfo
  {
    private XmlTokenStream _xmlTokens;
    private MemoryStream _contentStream;
    private TextReader _contentReader;
    private bool _recordDone;
    private int _depth;
    private bool _disposed;

    public WrappedReader(XmlDictionaryReader reader)
    {
      if (reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement())
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID6023")));
      this._xmlTokens = new XmlTokenStream(32);
      this.InitializeInnerReader(reader);
      this.Record();
    }

    public int LineNumber => !(this.InnerReader is IXmlLineInfo innerReader) ? 1 : innerReader.LineNumber;

    public int LinePosition => !(this.InnerReader is IXmlLineInfo innerReader) ? 1 : innerReader.LinePosition;

    public XmlTokenStream XmlTokens => this._xmlTokens;

    public override void Close()
    {
      this.OnEndOfContent();
      this.InnerReader.Close();
    }

    public bool HasLineInfo() => this.InnerReader is IXmlLineInfo innerReader && innerReader.HasLineInfo();

    public override void MoveToAttribute(int index)
    {
      this.OnEndOfContent();
      this.InnerReader.MoveToAttribute(index);
    }

    public override bool MoveToAttribute(string name)
    {
      this.OnEndOfContent();
      return this.InnerReader.MoveToAttribute(name);
    }

    public override bool MoveToAttribute(string name, string ns)
    {
      this.OnEndOfContent();
      return this.InnerReader.MoveToAttribute(name, ns);
    }

    public override bool MoveToElement()
    {
      this.OnEndOfContent();
      return base.MoveToElement();
    }

    public override bool MoveToFirstAttribute()
    {
      this.OnEndOfContent();
      return base.MoveToFirstAttribute();
    }

    public override bool MoveToNextAttribute()
    {
      this.OnEndOfContent();
      return base.MoveToNextAttribute();
    }

    private void OnEndOfContent()
    {
      if (this._contentReader != null)
      {
        this._contentReader.Close();
        this._contentReader = (TextReader) null;
      }
      if (this._contentStream == null)
        return;
      this._contentStream.Close();
      this._contentStream = (MemoryStream) null;
    }

    public override bool Read()
    {
      this.OnEndOfContent();
      if (!base.Read())
        return false;
      if (!this._recordDone)
        this.Record();
      return true;
    }

    private int ReadBinaryContent(byte[] buffer, int offset, int count, bool isBase64)
    {
      CryptoUtil.ValidateBufferBounds((Array) buffer, offset, count);
      if (this._contentStream == null)
      {
        string s;
        if (this.NodeType == XmlNodeType.Attribute)
        {
          s = this.Value;
        }
        else
        {
          StringBuilder stringBuilder = new StringBuilder(1000);
          while (this.NodeType != XmlNodeType.Element && this.NodeType != XmlNodeType.EndElement)
          {
            switch (this.NodeType)
            {
              case XmlNodeType.Text:
                stringBuilder.Append(this.Value);
                break;
            }
            this.Read();
          }
          s = stringBuilder.ToString();
        }
        this._contentStream = new MemoryStream(isBase64 ? Convert.FromBase64String(s) : SoapHexBinary.Parse(s).Value);
      }
      int num = this._contentStream.Read(buffer, offset, count);
      if (num == 0)
      {
        this._contentStream.Close();
        this._contentStream = (MemoryStream) null;
      }
      return num;
    }

    public override int ReadContentAsBase64(byte[] buffer, int offset, int count) => this.ReadBinaryContent(buffer, offset, count, true);

    public override int ReadContentAsBinHex(byte[] buffer, int offset, int count) => this.ReadBinaryContent(buffer, offset, count, false);

    public override int ReadValueChunk(char[] chars, int offset, int count)
    {
      if (this._contentReader == null)
        this._contentReader = (TextReader) new StringReader(this.Value);
      return this._contentReader.Read(chars, offset, count);
    }

    private void Record()
    {
      switch (this.NodeType)
      {
        case XmlNodeType.Element:
          bool isEmptyElement = this.InnerReader.IsEmptyElement;
          this._xmlTokens.AddElement(this.InnerReader.Prefix, this.InnerReader.LocalName, this.InnerReader.NamespaceURI, isEmptyElement);
          if (this.InnerReader.MoveToFirstAttribute())
          {
            do
            {
              this._xmlTokens.AddAttribute(this.InnerReader.Prefix, this.InnerReader.LocalName, this.InnerReader.NamespaceURI, this.InnerReader.Value);
            }
            while (this.InnerReader.MoveToNextAttribute());
            this.InnerReader.MoveToElement();
          }
          if (!isEmptyElement)
          {
            ++this._depth;
            break;
          }
          if (this._depth != 0)
            break;
          this._recordDone = true;
          break;
        case XmlNodeType.Text:
        case XmlNodeType.CDATA:
        case XmlNodeType.EntityReference:
        case XmlNodeType.Comment:
        case XmlNodeType.Whitespace:
        case XmlNodeType.SignificantWhitespace:
        case XmlNodeType.EndEntity:
          this._xmlTokens.Add(this.NodeType, this.Value);
          break;
        case XmlNodeType.DocumentType:
          break;
        case XmlNodeType.EndElement:
          this._xmlTokens.Add(this.NodeType, this.Value);
          if (--this._depth != 0)
            break;
          this._recordDone = true;
          break;
        case XmlNodeType.XmlDeclaration:
          break;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new XmlException(Microsoft.IdentityModel.SR.GetString("ID6024", (object) this.InnerReader.NodeType, (object) this.InnerReader.Name)));
      }
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this._disposed)
        return;
      if (disposing)
      {
        if (this._contentReader != null)
        {
          this._contentReader.Dispose();
          this._contentReader = (TextReader) null;
        }
        if (this._contentStream != null)
        {
          this._contentStream.Dispose();
          this._contentStream = (MemoryStream) null;
        }
      }
      this._disposed = true;
    }
  }
}
