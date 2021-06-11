// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.SignedInfo
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  internal class SignedInfo : IDisposable
  {
    private ExclusiveCanonicalizationTransform _canonicalizationMethodElement = new ExclusiveCanonicalizationTransform(true);
    private string _id;
    private string _signatureMethodAlgorithm;
    private MemoryStream _canonicalStream;
    private MemoryStream _bufferedStream;
    private List<Reference> _references;
    private Dictionary<string, string> _context;
    private string _prefix;
    private string _defaultNamespace = string.Empty;
    private bool _sendSide = true;
    private bool _disposed;

    public SignedInfo() => this._references = new List<Reference>();

    ~SignedInfo() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      if (disposing)
      {
        if (this._canonicalStream != null)
        {
          this._canonicalStream.Close();
          this._canonicalStream = (MemoryStream) null;
        }
        if (this._bufferedStream != null)
        {
          this._bufferedStream.Close();
          this._bufferedStream = (MemoryStream) null;
        }
      }
      this._disposed = true;
    }

    public virtual int ReferenceCount => this._references.Count;

    public Reference this[int index] => this._references[index];

    public void AddReference(Reference reference) => this._references.Add(reference);

    protected MemoryStream CanonicalStream
    {
      get => this._canonicalStream;
      set => this._canonicalStream = value;
    }

    protected bool SendSide
    {
      get => this._sendSide;
      set => this._sendSide = value;
    }

    public string CanonicalizationMethod
    {
      get => this._canonicalizationMethodElement.Algorithm;
      set
      {
        if (value != this._canonicalizationMethodElement.Algorithm)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID6006")));
      }
    }

    public string Id
    {
      get => this._id;
      set => this._id = value;
    }

    public string SignatureMethod
    {
      get => this._signatureMethodAlgorithm;
      set => this._signatureMethodAlgorithm = value;
    }

    public void ComputeHash(HashAlgorithm algorithm)
    {
      if (this.CanonicalizationMethod != "http://www.w3.org/2001/10/xml-exc-c14n#")
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6006")));
      using (HashStream hashStream = new HashStream(algorithm))
      {
        this.ComputeHash(hashStream);
        hashStream.FlushHash();
      }
    }

    protected virtual void ComputeHash(HashStream hashStream)
    {
      if (this._sendSide)
      {
        using (XmlDictionaryWriter textWriter = XmlDictionaryWriter.CreateTextWriter(Stream.Null, Encoding.UTF8, false))
        {
          textWriter.StartCanonicalization((Stream) hashStream, false, (string[]) null);
          this.WriteTo(textWriter);
          textWriter.EndCanonicalization();
        }
      }
      else if (this._canonicalStream != null)
      {
        this._canonicalStream.WriteTo((Stream) hashStream);
      }
      else
      {
        this._bufferedStream.Position = 0L;
        using (XmlDictionaryReader textReader = XmlDictionaryReader.CreateTextReader((Stream) this._bufferedStream, XmlDictionaryReaderQuotas.Max))
        {
          int content = (int) textReader.MoveToContent();
          using (XmlDictionaryWriter textWriter = XmlDictionaryWriter.CreateTextWriter(Stream.Null, Encoding.UTF8, false))
          {
            textWriter.WriteStartElement("a", this._defaultNamespace);
            string[] inclusivePrefixes = this.GetInclusivePrefixes();
            for (int index = 0; index < inclusivePrefixes.Length; ++index)
            {
              string forInclusivePrefix = this.GetNamespaceForInclusivePrefix(inclusivePrefixes[index]);
              if (forInclusivePrefix != null)
                textWriter.WriteXmlnsAttribute(inclusivePrefixes[index], forInclusivePrefix);
            }
            textWriter.StartCanonicalization((Stream) hashStream, false, inclusivePrefixes);
            textWriter.WriteNode(textReader, false);
            textWriter.EndCanonicalization();
            textWriter.WriteEndElement();
          }
        }
      }
    }

    public virtual void ComputeReferenceDigests()
    {
      if (this._references.Count == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6007")));
      for (int index = 0; index < this._references.Count; ++index)
        this._references[index].ComputeAndSetDigest();
    }

    protected string[] GetInclusivePrefixes() => this._canonicalizationMethodElement.GetInclusivePrefixes();

    protected virtual string GetNamespaceForInclusivePrefix(string prefix)
    {
      if (this._context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID3015")));
      return prefix != null ? this._context[prefix] : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (prefix));
    }

    public virtual void EnsureAllReferencesVerified()
    {
      for (int index = 0; index < this._references.Count; ++index)
      {
        if (!this._references[index].Verified)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6008", (object) this._references[index].Uri)));
      }
    }

    public void EnsureDigestValidity(string id, object resolvedXmlSource)
    {
      if (!this.EnsureDigestValidityIfIdMatches(id, resolvedXmlSource))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6009", (object) id)));
    }

    public virtual bool EnsureDigestValidityIfIdMatches(string id, object resolvedXmlSource)
    {
      for (int index = 0; index < this._references.Count; ++index)
      {
        if (this._references[index].EnsureDigestValidityIfIdMatches(id, resolvedXmlSource))
          return true;
      }
      return false;
    }

    protected void ReadCanonicalizationMethod(XmlDictionaryReader reader) => this._canonicalizationMethodElement.ReadFrom(reader);

    public virtual void ReadFrom(XmlDictionaryReader reader, TransformFactory transformFactory)
    {
      reader.MoveToStartElement(nameof (SignedInfo), "http://www.w3.org/2000/09/xmldsig#");
      this._sendSide = false;
      this._defaultNamespace = reader.LookupNamespace(string.Empty);
      this._bufferedStream = new MemoryStream();
      using (XmlWriter xmlWriter = XmlWriter.Create((Stream) this._bufferedStream, new XmlWriterSettings()
      {
        Encoding = Encoding.UTF8,
        NewLineHandling = NewLineHandling.None
      }))
      {
        xmlWriter.WriteNode((XmlReader) reader, true);
        xmlWriter.Flush();
      }
      this._bufferedStream.Position = 0L;
      using (XmlDictionaryReader textReader = XmlDictionaryReader.CreateTextReader((Stream) this._bufferedStream, XmlDictionaryReaderQuotas.Max))
      {
        this.CanonicalStream = new MemoryStream();
        textReader.StartCanonicalization((Stream) this.CanonicalStream, false, (string[]) null);
        textReader.MoveToStartElement(nameof (SignedInfo), "http://www.w3.org/2000/09/xmldsig#");
        this._prefix = textReader.Prefix;
        this.Id = textReader.GetAttribute("Id", (string) null);
        textReader.Read();
        this.ReadCanonicalizationMethod(textReader);
        this.ReadSignatureMethod(textReader);
        while (textReader.IsStartElement("Reference", "http://www.w3.org/2000/09/xmldsig#"))
        {
          Reference reference = new Reference();
          reference.ReadFrom(textReader, transformFactory);
          this.AddReference(reference);
        }
        textReader.ReadEndElement();
        textReader.EndCanonicalization();
      }
      string[] inclusivePrefixes = this.GetInclusivePrefixes();
      if (inclusivePrefixes == null)
        return;
      this.CanonicalStream = (MemoryStream) null;
      this._context = new Dictionary<string, string>(inclusivePrefixes.Length);
      for (int index = 0; index < inclusivePrefixes.Length; ++index)
        this._context.Add(inclusivePrefixes[index], reader.LookupNamespace(inclusivePrefixes[index]));
    }

    protected void ReadSignatureMethod(XmlDictionaryReader reader)
    {
      reader.MoveToStartElement("SignatureMethod", "http://www.w3.org/2000/09/xmldsig#");
      bool isEmptyElement = reader.IsEmptyElement;
      this._signatureMethodAlgorithm = reader.GetAttribute("Algorithm", (string) null);
      if (string.IsNullOrEmpty(this._signatureMethodAlgorithm))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID0001", (object) "Algorithm", (object) "SignatureMethod")));
      reader.Read();
      int content = (int) reader.MoveToContent();
      if (isEmptyElement)
        return;
      reader.ReadEndElement();
    }

    protected void WriteCanonicalizationMethod(XmlDictionaryWriter writer) => this._canonicalizationMethodElement.WriteTo(writer);

    protected void WriteSignatureMethod(XmlDictionaryWriter writer)
    {
      writer.WriteStartElement("ds", "SignatureMethod", "http://www.w3.org/2000/09/xmldsig#");
      writer.WriteAttributeString("Algorithm", (string) null, this._signatureMethodAlgorithm);
      writer.WriteEndElement();
    }

    public virtual void WriteTo(XmlDictionaryWriter writer)
    {
      writer.WriteStartElement(this._prefix, nameof (SignedInfo), "http://www.w3.org/2000/09/xmldsig#");
      if (this.Id != null)
        writer.WriteAttributeString("Id", (string) null, this.Id);
      this.WriteCanonicalizationMethod(writer);
      this.WriteSignatureMethod(writer);
      for (int index = 0; index < this._references.Count; ++index)
        this._references[index].WriteTo(writer);
      writer.WriteEndElement();
    }
  }
}
