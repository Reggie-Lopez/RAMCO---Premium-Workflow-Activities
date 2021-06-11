// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlSignature.Signature
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IO;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
  internal sealed class Signature : IDisposable
  {
    private SignedXml _signedXml;
    private string _id;
    private SecurityKeyIdentifier _keyIdentifier;
    private string _prefix = "ds";
    private Signature.SignatureValueElement _signatureValueElement = new Signature.SignatureValueElement();
    private SignedInfo _signedInfo;
    private List<Stream> _signedObjects = new List<Stream>(1);
    private bool _disposed;

    public Signature(SignedXml signedXml, SignedInfo signedInfo)
    {
      this._signedXml = signedXml;
      this._signedInfo = signedInfo;
    }

    ~Signature() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      if (disposing && !this._disposed)
      {
        for (int index = 0; index < this._signedObjects.Count; ++index)
          this._signedObjects[index].Close();
        this._signedInfo.Dispose();
      }
      this._disposed = true;
    }

    public string Id
    {
      get => this._id;
      set => this._id = value;
    }

    public SecurityKeyIdentifier KeyIdentifier
    {
      get => this._keyIdentifier;
      set => this._keyIdentifier = value;
    }

    public SignedInfo SignedInfo => this._signedInfo;

    public IList<Stream> SignedObjects => (IList<Stream>) this._signedObjects;

    public XmlDictionaryReader GetSignedObjectReader(int index)
    {
      if (index > this._signedObjects.Count)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (index));
      this._signedObjects[index].Position = 0L;
      return XmlDictionaryReader.CreateTextReader(this._signedObjects[index], XmlDictionaryReaderQuotas.Max);
    }

    public byte[] GetSignatureBytes() => this._signatureValueElement.Value;

    public void ReadFrom(XmlDictionaryReader reader)
    {
      reader.MoveToStartElement(nameof (Signature), "http://www.w3.org/2000/09/xmldsig#");
      this._prefix = reader.Prefix;
      this.Id = reader.GetAttribute("Id", (string) null);
      reader.Read();
      this._signedInfo.ReadFrom(reader, this._signedXml.TransformFactory);
      this._signatureValueElement.ReadFrom(reader);
      if (this._signedXml.SecurityTokenSerializer.CanReadKeyIdentifier((XmlReader) reader))
        this._keyIdentifier = this._signedXml.SecurityTokenSerializer.ReadKeyIdentifier((XmlReader) reader);
      while (reader.IsStartElement("Object", "http://www.w3.org/2000/09/xmldsig#"))
      {
        MemoryStream memoryStream = new MemoryStream();
        using (XmlDictionaryWriter textWriter = XmlDictionaryWriter.CreateTextWriter((Stream) memoryStream, Encoding.UTF8, false))
        {
          textWriter.WriteNode(reader, false);
          textWriter.Flush();
        }
        this._signedObjects.Add((Stream) memoryStream);
      }
      reader.ReadEndElement();
    }

    public void SetSignatureValue(byte[] signatureValue) => this._signatureValueElement.Value = signatureValue;

    public void WriteTo(XmlDictionaryWriter writer)
    {
      writer.WriteStartElement(this._prefix, nameof (Signature), "http://www.w3.org/2000/09/xmldsig#");
      if (this._id != null)
        writer.WriteAttributeString("Id", (string) null, this._id);
      this._signedInfo.WriteTo(writer);
      this._signatureValueElement.WriteTo(writer);
      if (this._keyIdentifier != null)
        this._signedXml.SecurityTokenSerializer.WriteKeyIdentifier((XmlWriter) writer, this._keyIdentifier);
      if (this._signedObjects.Count > 0)
      {
        for (int index = 0; index < this._signedObjects.Count; ++index)
        {
          XmlDictionaryReader textReader = XmlDictionaryReader.CreateTextReader(this._signedObjects[index], XmlDictionaryReaderQuotas.Max);
          int content = (int) textReader.MoveToContent();
          writer.WriteNode(textReader, false);
        }
      }
      writer.WriteEndElement();
    }

    private sealed class SignatureValueElement
    {
      private string _id;
      private string _prefix = "ds";
      private byte[] _signatureValue;
      private string _signatureText;

      internal byte[] Value
      {
        get => this._signatureValue;
        set
        {
          this._signatureValue = value;
          this._signatureText = (string) null;
        }
      }

      public void ReadFrom(XmlDictionaryReader reader)
      {
        reader.MoveToStartElement("SignatureValue", "http://www.w3.org/2000/09/xmldsig#");
        this._prefix = reader.Prefix;
        this._id = reader.GetAttribute("Id", (string) null);
        reader.Read();
        this._signatureText = reader.ReadString();
        this._signatureValue = Convert.FromBase64String(this._signatureText.Trim());
        reader.ReadEndElement();
      }

      public void WriteTo(XmlDictionaryWriter writer)
      {
        writer.WriteStartElement(this._prefix, "SignatureValue", "http://www.w3.org/2000/09/xmldsig#");
        if (this._id != null)
          writer.WriteAttributeString("Id", (string) null, this._id);
        if (this._signatureText != null)
          writer.WriteString(this._signatureText);
        else
          writer.WriteBase64(this._signatureValue, 0, this._signatureValue.Length);
        writer.WriteEndElement();
      }
    }
  }
}
