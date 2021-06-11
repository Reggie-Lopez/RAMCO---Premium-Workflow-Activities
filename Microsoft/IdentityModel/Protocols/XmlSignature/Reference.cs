// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.Reference
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Diagnostics;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  internal sealed class Reference
  {
    private string _digestMethodElementAlgorithm;
    private Reference.DigestValueElement _digestValueElement = new Reference.DigestValueElement();
    private string _id;
    private string _prefix = "ds";
    private object _resolvedXmlSource;
    private TransformChain _transformChain = new TransformChain();
    private string _type;
    private string _uri;
    private bool _verified;
    private string _referredId;

    public Reference()
      : this((string) null)
    {
    }

    public Reference(string uri)
      : this(uri, (object) null)
    {
    }

    public Reference(string uri, object resolvedXmlSource)
    {
      this._uri = uri;
      this._resolvedXmlSource = resolvedXmlSource;
    }

    public string DigestMethod
    {
      get => this._digestMethodElementAlgorithm;
      set => this._digestMethodElementAlgorithm = value;
    }

    public string Id
    {
      get => this._id;
      set => this._id = value;
    }

    public TransformChain TransformChain => this._transformChain;

    public int TransformCount => this._transformChain.TransformCount;

    public string Type
    {
      get => this._type;
      set => this._type = value;
    }

    public string Uri
    {
      get => this._uri;
      set => this._uri = value;
    }

    public bool Verified => this._verified;

    public void AddTransform(Transform transform) => this._transformChain.Add(transform);

    public void EnsureDigestValidity(string id, byte[] computedDigest)
    {
      if (!this.EnsureDigestValidityIfIdMatches(id, computedDigest))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6009", (object) id)));
    }

    public void EnsureDigestValidity(string id, object resolvedXmlSource)
    {
      if (!this.EnsureDigestValidityIfIdMatches(id, resolvedXmlSource))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6009", (object) id)));
    }

    public bool EnsureDigestValidityIfIdMatches(string id, byte[] computedDigest)
    {
      if (this._verified || id != this.ExtractReferredId())
        return false;
      if (!ByteArrayComparer.Instance.Equals(computedDigest, this.DigestValue))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6018", (object) this._uri)));
      this._verified = true;
      return true;
    }

    public bool EnsureDigestValidityIfIdMatches(string id, object resolvedXmlSource)
    {
      if (this._verified || id != this.ExtractReferredId())
        return false;
      this._resolvedXmlSource = resolvedXmlSource;
      if (!this.CheckDigest())
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6018", (object) this._uri)));
      this._verified = true;
      return true;
    }

    public string ExtractReferredId()
    {
      if (this._referredId == null)
      {
        if (StringComparer.OrdinalIgnoreCase.Equals(this._uri, string.Empty))
          return string.Empty;
        this._referredId = this._uri != null && this._uri.Length >= 2 && this._uri[0] == '#' ? this._uri.Substring(1) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6008", (object) this._uri)));
      }
      return this._referredId;
    }

    public bool CheckDigest()
    {
      byte[] digest = this.ComputeDigest();
      bool areEqual = ByteArrayComparer.Instance.Equals(digest, this.DigestValue);
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Verbose))
        DiagnosticUtil.TraceUtil.Trace(TraceEventType.Verbose, TraceCode.Diagnostics, Microsoft.IdentityModel.SR.GetString("TraceDigestOfReference"), (TraceRecord) new ReferenceTraceRecord(areEqual, digest, this.DigestValue, this._uri), (Exception) null);
      return areEqual;
    }

    public void ComputeAndSetDigest() => this._digestValueElement.Value = this.ComputeDigest();

    public byte[] ComputeDigest()
    {
      if (this._transformChain.TransformCount == 0)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID6020")));
      if (this._resolvedXmlSource == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID6008", (object) this._uri)));
      return this._transformChain.TransformToDigest(this._resolvedXmlSource, this.DigestMethod);
    }

    public byte[] DigestValue
    {
      get => this._digestValueElement.Value;
      set => this._digestValueElement.Value = value;
    }

    public void ReadFrom(XmlDictionaryReader reader, TransformFactory transformFactory)
    {
      reader.MoveToStartElement(nameof (Reference), "http://www.w3.org/2000/09/xmldsig#");
      this._prefix = reader.Prefix;
      this.Id = reader.GetAttribute("Id", (string) null);
      this.Uri = reader.GetAttribute("URI", (string) null);
      this.Type = reader.GetAttribute("Type", (string) null);
      reader.Read();
      if (reader.IsStartElement("Transforms", "http://www.w3.org/2000/09/xmldsig#"))
        this._transformChain.ReadFrom(reader, transformFactory);
      reader.MoveToStartElement("DigestMethod", "http://www.w3.org/2000/09/xmldsig#");
      bool isEmptyElement = reader.IsEmptyElement;
      this._digestMethodElementAlgorithm = reader.GetAttribute("Algorithm", (string) null);
      if (string.IsNullOrEmpty(this._digestMethodElementAlgorithm))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new CryptographicException(Microsoft.IdentityModel.SR.GetString("ID0001", (object) "Algorithm", (object) "DigestMethod")));
      reader.Read();
      int content1 = (int) reader.MoveToContent();
      if (!isEmptyElement)
        reader.ReadEndElement();
      this._digestValueElement.ReadFrom(reader);
      int content2 = (int) reader.MoveToContent();
      reader.ReadEndElement();
    }

    public void SetResolvedXmlSource(object resolvedXmlSource) => this._resolvedXmlSource = resolvedXmlSource;

    public void WriteTo(XmlDictionaryWriter writer)
    {
      writer.WriteStartElement(this._prefix, nameof (Reference), "http://www.w3.org/2000/09/xmldsig#");
      if (this._id != null)
        writer.WriteAttributeString("Id", (string) null, this._id);
      if (this._uri != null)
        writer.WriteAttributeString("URI", (string) null, this._uri);
      if (this._type != null)
        writer.WriteAttributeString("Type", (string) null, this._type);
      if (this._transformChain.TransformCount > 0)
        this._transformChain.WriteTo(writer);
      writer.WriteStartElement("ds", "DigestMethod", "http://www.w3.org/2000/09/xmldsig#");
      writer.WriteAttributeString("Algorithm", this._digestMethodElementAlgorithm);
      writer.WriteEndElement();
      this._digestValueElement.WriteTo(writer);
      writer.WriteEndElement();
    }

    private struct DigestValueElement
    {
      private byte[] digestValue;
      private string digestText;
      private string prefix;

      internal byte[] Value
      {
        get => this.digestValue;
        set
        {
          this.digestValue = value;
          this.digestText = (string) null;
        }
      }

      public void ReadFrom(XmlDictionaryReader reader)
      {
        reader.MoveToStartElement("DigestValue", "http://www.w3.org/2000/09/xmldsig#");
        this.prefix = reader.Prefix;
        reader.Read();
        int content1 = (int) reader.MoveToContent();
        this.digestText = reader.ReadString();
        this.digestValue = Convert.FromBase64String(this.digestText.Trim());
        int content2 = (int) reader.MoveToContent();
        reader.ReadEndElement();
      }

      public void WriteTo(XmlDictionaryWriter writer)
      {
        writer.WriteStartElement(this.prefix ?? "ds", "DigestValue", "http://www.w3.org/2000/09/xmldsig#");
        if (this.digestText != null)
          writer.WriteString(this.digestText);
        else
          writer.WriteBase64(this.digestValue, 0, this.digestValue.Length);
        writer.WriteEndElement();
      }
    }
  }
}
