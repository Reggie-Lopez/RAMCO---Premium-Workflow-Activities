// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.EnvelopedSignatureWriter
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
  public sealed class EnvelopedSignatureWriter : DelegatingXmlDictionaryWriter
  {
    private XmlWriter _innerWriter;
    private SigningCredentials _signingCreds;
    private string _referenceId;
    private SecurityTokenSerializer _tokenSerializer;
    private HashStream _hashStream;
    private HashAlgorithm _hashAlgorithm;
    private int _elementCount;
    private MemoryStream _signatureFragment;
    private MemoryStream _endFragment;
    private bool _hasSignatureBeenMarkedForInsert;
    private MemoryStream _writerStream;
    private MemoryStream _preCanonicalTracingStream;
    private bool _disposed;

    public EnvelopedSignatureWriter(
      XmlWriter innerWriter,
      SigningCredentials signingCredentials,
      string referenceId,
      SecurityTokenSerializer securityTokenSerializer)
    {
      if (innerWriter == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (innerWriter));
      if (signingCredentials == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (signingCredentials));
      if (string.IsNullOrEmpty(referenceId))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID0006"), nameof (referenceId)));
      if (securityTokenSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenSerializer));
      this._innerWriter = innerWriter;
      this._signingCreds = signingCredentials;
      this._referenceId = referenceId;
      this._tokenSerializer = securityTokenSerializer;
      this._signatureFragment = new MemoryStream();
      this._endFragment = new MemoryStream();
      this._writerStream = new MemoryStream();
      this.InitializeInnerWriter(XmlDictionaryWriter.CreateTextWriter((Stream) this._writerStream, Encoding.UTF8, false));
      this._hashAlgorithm = CryptoUtil.Algorithms.CreateHashAlgorithm(this._signingCreds.DigestAlgorithm);
      this._hashStream = new HashStream(this._hashAlgorithm);
      this.InnerWriter.StartCanonicalization((Stream) this._hashStream, false, (string[]) null);
      if (!DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Verbose))
        return;
      this._preCanonicalTracingStream = new MemoryStream();
      this.InitializeTracingWriter((XmlWriter) new XmlTextWriter((Stream) this._preCanonicalTracingStream, Encoding.UTF8));
    }

    private void ComputeSignature()
    {
      PreDigestedSignedInfo digestedSignedInfo = new PreDigestedSignedInfo();
      digestedSignedInfo.AddEnvelopedSignatureTransform = true;
      digestedSignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";
      digestedSignedInfo.SignatureMethod = this._signingCreds.SignatureAlgorithm;
      digestedSignedInfo.DigestMethod = this._signingCreds.DigestAlgorithm;
      digestedSignedInfo.AddReference(this._referenceId, this._hashStream.FlushHashAndGetValue(this._preCanonicalTracingStream));
      SignedXml signedXml = new SignedXml((SignedInfo) digestedSignedInfo, this._tokenSerializer);
      signedXml.ComputeSignature(this._signingCreds.SigningKey);
      signedXml.Signature.KeyIdentifier = this._signingCreds.SigningKeyIdentifier;
      signedXml.WriteTo(this.InnerWriter);
      signedXml.Dispose();
      this._hashStream.Dispose();
      this._hashStream = (HashStream) null;
    }

    private void OnEndRootElement()
    {
      if (!this._hasSignatureBeenMarkedForInsert)
      {
        ((IFragmentCapableXmlDictionaryWriter) this.InnerWriter).StartFragment((Stream) this._endFragment, false);
        base.WriteEndElement();
        ((IFragmentCapableXmlDictionaryWriter) this.InnerWriter).EndFragment();
      }
      else if (this._hasSignatureBeenMarkedForInsert)
      {
        base.WriteEndElement();
        ((IFragmentCapableXmlDictionaryWriter) this.InnerWriter).EndFragment();
      }
      this.EndCanonicalization();
      ((IFragmentCapableXmlDictionaryWriter) this.InnerWriter).StartFragment((Stream) this._signatureFragment, false);
      this.ComputeSignature();
      ((IFragmentCapableXmlDictionaryWriter) this.InnerWriter).EndFragment();
      ((IFragmentCapableXmlDictionaryWriter) this.InnerWriter).WriteFragment(this._signatureFragment.GetBuffer(), 0, (int) this._signatureFragment.Length);
      ((IFragmentCapableXmlDictionaryWriter) this.InnerWriter).WriteFragment(this._endFragment.GetBuffer(), 0, (int) this._endFragment.Length);
      this._signatureFragment.Close();
      this._endFragment.Close();
      this._writerStream.Position = 0L;
      this._hasSignatureBeenMarkedForInsert = false;
      XmlReader textReader = (XmlReader) XmlDictionaryReader.CreateTextReader((Stream) this._writerStream, XmlDictionaryReaderQuotas.Max);
      int content = (int) textReader.MoveToContent();
      this._innerWriter.WriteNode(textReader, false);
      this._innerWriter.Flush();
      textReader.Close();
      this.Close();
    }

    public void WriteSignature()
    {
      this.Flush();
      if (this._writerStream == null || this._writerStream.Length == 0L)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID6029")));
      if (this._signatureFragment.Length != 0L)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID6030")));
      ((IFragmentCapableXmlDictionaryWriter) this.InnerWriter).StartFragment((Stream) this._endFragment, false);
      this._hasSignatureBeenMarkedForInsert = true;
    }

    public override void WriteEndElement()
    {
      --this._elementCount;
      if (this._elementCount == 0)
      {
        this.Flush();
        this.OnEndRootElement();
      }
      else
        base.WriteEndElement();
    }

    public override void WriteFullEndElement()
    {
      --this._elementCount;
      if (this._elementCount == 0)
      {
        this.Flush();
        this.OnEndRootElement();
      }
      else
        base.WriteFullEndElement();
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
        if (this._hashStream != null)
        {
          this._hashStream.Dispose();
          this._hashStream = (HashStream) null;
        }
        if (this._hashAlgorithm != null)
        {
          this._hashAlgorithm.Dispose();
          this._hashAlgorithm = (HashAlgorithm) null;
        }
        if (this._signatureFragment != null)
        {
          this._signatureFragment.Dispose();
          this._signatureFragment = (MemoryStream) null;
        }
        if (this._endFragment != null)
        {
          this._endFragment.Dispose();
          this._endFragment = (MemoryStream) null;
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
