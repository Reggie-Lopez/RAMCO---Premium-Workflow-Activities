// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.CanonicalizationDriver
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  internal sealed class CanonicalizationDriver
  {
    private bool _closeReadersAfterProcessing;
    private XmlReader _reader;
    private string[] _inclusivePrefixes;
    private bool _includeComments;

    public bool CloseReadersAfterProcessing
    {
      get => this._closeReadersAfterProcessing;
      set => this._closeReadersAfterProcessing = value;
    }

    public bool IncludeComments
    {
      get => this._includeComments;
      set => this._includeComments = value;
    }

    public string[] GetInclusivePrefixes() => this._inclusivePrefixes;

    public void Reset() => this._reader = (XmlReader) null;

    public void SetInclusivePrefixes(string[] inclusivePrefixes) => this._inclusivePrefixes = inclusivePrefixes;

    public void SetInput(XmlReader reader) => this._reader = reader != null ? reader : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));

    public byte[] GetBytes()
    {
      using (MemoryStream memoryStream = this.GetMemoryStream())
        return memoryStream.ToArray();
    }

    public MemoryStream GetMemoryStream()
    {
      MemoryStream memoryStream = new MemoryStream();
      this.WriteTo((Stream) memoryStream);
      memoryStream.Seek(0L, SeekOrigin.Begin);
      return memoryStream;
    }

    public void WriteTo(HashAlgorithm hashAlgorithm) => this.WriteTo((Stream) new HashStream(hashAlgorithm));

    public void WriteTo(Stream canonicalStream)
    {
      if (this._reader == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID6003")));
      if (this._reader is XmlDictionaryReader reader && reader.CanCanonicalize)
      {
        int content = (int) reader.MoveToContent();
        reader.StartCanonicalization(canonicalStream, this._includeComments, this._inclusivePrefixes);
        reader.Skip();
        reader.EndCanonicalization();
      }
      else
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (XmlDictionaryWriter textWriter = XmlDictionaryWriter.CreateTextWriter((Stream) memoryStream, Encoding.UTF8, false))
          {
            if (this._inclusivePrefixes != null)
            {
              textWriter.WriteStartElement("a", this._reader.LookupNamespace(string.Empty));
              for (int index = 0; index < this._inclusivePrefixes.Length; ++index)
              {
                string namespaceUri = this._reader.LookupNamespace(this._inclusivePrefixes[index]);
                if (namespaceUri != null)
                  textWriter.WriteXmlnsAttribute(this._inclusivePrefixes[index], namespaceUri);
              }
            }
            textWriter.StartCanonicalization(canonicalStream, this._includeComments, this._inclusivePrefixes);
            if (this._reader is WrappedReader)
              ((WrappedReader) this._reader).XmlTokens.GetWriter().WriteTo(textWriter);
            else
              textWriter.WriteNode(this._reader, false);
            textWriter.Flush();
            textWriter.EndCanonicalization();
            if (this._inclusivePrefixes != null)
              textWriter.WriteEndElement();
          }
        }
      }
      if (this._closeReadersAfterProcessing)
        this._reader.Close();
      this._reader = (XmlReader) null;
    }
  }
}
