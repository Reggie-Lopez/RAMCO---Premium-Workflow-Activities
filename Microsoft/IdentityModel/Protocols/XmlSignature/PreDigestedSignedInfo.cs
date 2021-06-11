// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.PreDigestedSignedInfo
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  internal sealed class PreDigestedSignedInfo : SignedInfo
  {
    private const int InitialReferenceArraySize = 8;
    private bool _addEnvelopedSignatureTransform;
    private int _count;
    private string _digestMethod;
    private PreDigestedSignedInfo.ReferenceEntry[] _references;

    public PreDigestedSignedInfo() => this._references = new PreDigestedSignedInfo.ReferenceEntry[8];

    public PreDigestedSignedInfo(
      string canonicalizationMethod,
      string digestMethod,
      string signatureMethod)
    {
      this._references = new PreDigestedSignedInfo.ReferenceEntry[8];
      this.CanonicalizationMethod = canonicalizationMethod;
      this.DigestMethod = digestMethod;
      this.SignatureMethod = signatureMethod;
    }

    public bool AddEnvelopedSignatureTransform
    {
      get => this._addEnvelopedSignatureTransform;
      set => this._addEnvelopedSignatureTransform = value;
    }

    public string DigestMethod
    {
      get => this._digestMethod;
      set => this._digestMethod = value;
    }

    public override int ReferenceCount => this._count;

    public void AddReference(string id, byte[] digest)
    {
      if (this._count == this._references.Length)
      {
        PreDigestedSignedInfo.ReferenceEntry[] referenceEntryArray = new PreDigestedSignedInfo.ReferenceEntry[this._references.Length * 2];
        Array.Copy((Array) this._references, 0, (Array) referenceEntryArray, 0, this._count);
        this._references = referenceEntryArray;
      }
      this._references[this._count++].Set(id, digest);
    }

    protected override void ComputeHash(HashStream hashStream)
    {
      if (this.AddEnvelopedSignatureTransform)
      {
        base.ComputeHash(hashStream);
      }
      else
      {
        using (XmlDictionaryWriter textWriter = XmlDictionaryWriter.CreateTextWriter(Stream.Null, Encoding.UTF8, false))
        {
          textWriter.StartCanonicalization((Stream) hashStream, false, (string[]) null);
          this.WriteTo(textWriter);
          textWriter.Flush();
          textWriter.EndCanonicalization();
        }
      }
    }

    public override void ComputeReferenceDigests()
    {
    }

    public override void ReadFrom(XmlDictionaryReader reader, TransformFactory transformFactory) => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException());

    public override void EnsureAllReferencesVerified() => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException());

    public override bool EnsureDigestValidityIfIdMatches(string id, object resolvedXmlSource) => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException());

    public override void WriteTo(XmlDictionaryWriter writer)
    {
      string prefix = "ds";
      string ns = "http://www.w3.org/2000/09/xmldsig#";
      writer.WriteStartElement(prefix, "SignedInfo", ns);
      if (this.Id != null)
        writer.WriteAttributeString("Id", (string) null, this.Id);
      this.WriteCanonicalizationMethod(writer);
      this.WriteSignatureMethod(writer);
      for (int index = 0; index < this._count; ++index)
      {
        writer.WriteStartElement(prefix, "Reference", ns);
        writer.WriteStartAttribute("URI", (string) null);
        writer.WriteString("#");
        writer.WriteString(this._references[index]._id);
        writer.WriteEndAttribute();
        writer.WriteStartElement(prefix, "Transforms", ns);
        if (this._addEnvelopedSignatureTransform)
        {
          writer.WriteStartElement(prefix, "Transform", ns);
          writer.WriteStartAttribute("Algorithm", (string) null);
          writer.WriteString("http://www.w3.org/2000/09/xmldsig#enveloped-signature");
          writer.WriteEndAttribute();
          writer.WriteEndElement();
        }
        writer.WriteStartElement(prefix, "Transform", ns);
        writer.WriteStartAttribute("Algorithm", (string) null);
        writer.WriteString("http://www.w3.org/2001/10/xml-exc-c14n#");
        writer.WriteEndAttribute();
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteStartElement(prefix, "DigestMethod", ns);
        writer.WriteStartAttribute("Algorithm", (string) null);
        writer.WriteString(this._digestMethod);
        writer.WriteEndAttribute();
        writer.WriteEndElement();
        byte[] digest = this._references[index]._digest;
        writer.WriteStartElement(prefix, "DigestValue", ns);
        writer.WriteBase64(digest, 0, digest.Length);
        writer.WriteEndElement();
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }

    private struct ReferenceEntry
    {
      internal string _id;
      internal byte[] _digest;

      public void Set(string id, byte[] digest)
      {
        this._id = id;
        this._digest = digest;
      }
    }
  }
}
