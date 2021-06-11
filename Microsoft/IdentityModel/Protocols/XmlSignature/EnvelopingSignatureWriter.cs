// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.EnvelopingSignatureWriter
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  [ComVisible(true)]
  public sealed class EnvelopingSignatureWriter : DelegatingXmlDictionaryWriter
  {
    private SigningCredentials _signingCreds;
    private XmlDictionaryWriter _innerWriter;
    private HashStream _hashStream;
    private HashAlgorithm _hashAlgorithm;
    private MemoryStream _writerStream;
    private MemoryStream _preCanonicalTracingStream;
    private string _objectId;
    private SecurityTokenSerializer _tokenSerializer;
    private int _elementCount;
    private bool _disposed;

    public EnvelopingSignatureWriter(
      XmlWriter innerWriter,
      SigningCredentials signingCredentials,
      string objectId,
      SecurityTokenSerializer securityTokenSerializer)
    {
      if (innerWriter == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (innerWriter));
      if (signingCredentials == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (signingCredentials));
      if (string.IsNullOrEmpty(objectId))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0006"), nameof (objectId)));
      if (securityTokenSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenSerializer));
      this._objectId = XmlConvert.VerifyNCName(objectId);
      this._innerWriter = XmlDictionaryWriter.CreateDictionaryWriter(innerWriter);
      this._signingCreds = signingCredentials;
      this._hashAlgorithm = CryptoUtil.Algorithms.CreateHashAlgorithm(this._signingCreds.DigestAlgorithm);
      this._hashStream = new HashStream(this._hashAlgorithm);
      this._writerStream = new MemoryStream();
      this._tokenSerializer = securityTokenSerializer;
      this.InitializeInnerWriter(XmlDictionaryWriter.CreateTextWriter((Stream) this._writerStream, Encoding.UTF8, false));
      this.InnerWriter.StartCanonicalization((Stream) this._hashStream, false, (string[]) null);
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Verbose))
      {
        this._preCanonicalTracingStream = new MemoryStream();
        this.InitializeTracingWriter((XmlWriter) new XmlTextWriter((Stream) this._preCanonicalTracingStream, Encoding.UTF8));
      }
      this.InnerWriter.WriteStartElement("ds", "Object", "http://www.w3.org/2000/09/xmldsig#");
      this.InnerWriter.WriteAttributeString("Id", this._objectId);
    }

    private void OnEndRootElement()
    {
      this.InnerWriter.WriteEndElement();
      this.InnerWriter.Flush();
      this.InnerWriter.EndCanonicalization();
      this._writerStream.Position = 0L;
      PreDigestedSignedInfo digestedSignedInfo = new PreDigestedSignedInfo();
      digestedSignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";
      digestedSignedInfo.SignatureMethod = this._signingCreds.SignatureAlgorithm;
      digestedSignedInfo.DigestMethod = this._signingCreds.DigestAlgorithm;
      digestedSignedInfo.AddReference(this._objectId, this._hashStream.FlushHashAndGetValue(this._preCanonicalTracingStream));
      SignedXml signedXml = new SignedXml((SignedInfo) digestedSignedInfo, this._tokenSerializer);
      signedXml.ComputeSignature(this._signingCreds.SigningKey);
      signedXml.Signature.KeyIdentifier = this._signingCreds.SigningKeyIdentifier;
      signedXml.Signature.SignedObjects.Add((Stream) this._writerStream);
      signedXml.WriteTo(this._innerWriter);
      this._innerWriter.Flush();
      this.InnerWriter.Close();
      this._hashAlgorithm.Dispose();
      this._hashAlgorithm = (HashAlgorithm) null;
      this._hashStream.Dispose();
      this._hashStream = (HashStream) null;
      this._writerStream.Dispose();
      this._writerStream = (MemoryStream) null;
    }

    public override void WriteEndElement()
    {
      this.InnerWriter.WriteEndElement();
      --this._elementCount;
      if (this._elementCount != 0)
        return;
      this.OnEndRootElement();
    }

    public override void WriteFullEndElement()
    {
      this.InnerWriter.WriteFullEndElement();
      --this._elementCount;
      if (this._elementCount != 0)
        return;
      this.OnEndRootElement();
    }

    public override void WriteStartElement(string prefix, string localName, string ns)
    {
      ++this._elementCount;
      base.WriteStartElement(prefix, localName, ns);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this._disposed)
        return;
      if (disposing)
      {
        if (this._hashAlgorithm != null)
        {
          this._hashAlgorithm.Dispose();
          this._hashAlgorithm = (HashAlgorithm) null;
        }
        if (this._hashStream != null)
        {
          this._hashStream.Dispose();
          this._hashStream = (HashStream) null;
        }
        if (this._writerStream != null)
        {
          this._writerStream.Dispose();
          this._writerStream = (MemoryStream) null;
        }
        if (this._preCanonicalTracingStream != null)
        {
          this._preCanonicalTracingStream.Dispose();
          this._preCanonicalTracingStream = (MemoryStream) null;
        }
      }
      this._disposed = true;
    }
  }
}
