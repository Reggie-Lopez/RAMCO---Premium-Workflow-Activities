// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlEncryption.EncryptedTypeElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.XmlSignature;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlEncryption
{
  internal abstract class EncryptedTypeElement
  {
    private KeyInfo _keyInfo;
    private EncryptionMethodElement _encryptionMethod;
    private CipherDataElement _cipherData;
    private List<string> _properties;
    private SecurityTokenSerializer _keyInfoSerializer;
    private string _id;
    private string _type;
    private string _mimeType;
    private string _encoding;

    public EncryptedTypeElement(SecurityTokenSerializer keyInfoSerializer)
    {
      this._cipherData = new CipherDataElement();
      this._encryptionMethod = new EncryptionMethodElement();
      this._keyInfo = new KeyInfo(keyInfoSerializer);
      this._properties = new List<string>();
      this._keyInfoSerializer = keyInfoSerializer;
    }

    public string Algorithm
    {
      get => this.EncryptionMethod == null ? (string) null : this.EncryptionMethod.Algorithm;
      set => this.EncryptionMethod.Algorithm = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public string Id
    {
      get => this._id;
      set => this._id = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (value));
    }

    public EncryptionMethodElement EncryptionMethod
    {
      get => this._encryptionMethod;
      set => this._encryptionMethod = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public CipherDataElement CipherData
    {
      get => this._cipherData;
      set => this._cipherData = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public SecurityKeyIdentifier KeyIdentifier
    {
      get => this._keyInfo.KeyIdentifier;
      set => this._keyInfo.KeyIdentifier = value != null ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
    }

    public abstract void ReadExtensions(XmlDictionaryReader reader);

    public SecurityTokenSerializer TokenSerializer => this._keyInfoSerializer;

    public string Type
    {
      get => this._type;
      set => this._type = !string.IsNullOrEmpty(value) ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (value));
    }

    public virtual void ReadXml(XmlDictionaryReader reader)
    {
      int num = reader != null ? (int) reader.MoveToContent() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      this._id = reader.GetAttribute("Id", (string) null);
      this._type = reader.GetAttribute("Type", (string) null);
      this._mimeType = reader.GetAttribute("MimeType", (string) null);
      this._encoding = reader.GetAttribute("Encoding", (string) null);
      reader.ReadStartElement();
      int content1 = (int) reader.MoveToContent();
      if (reader.IsStartElement("EncryptionMethod", "http://www.w3.org/2001/04/xmlenc#"))
        this._encryptionMethod.ReadXml(reader);
      int content2 = (int) reader.MoveToContent();
      if (reader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#"))
      {
        this._keyInfo = new KeyInfo(this._keyInfoSerializer);
        if (this._keyInfoSerializer.CanReadKeyIdentifier((XmlReader) reader))
          this._keyInfo.KeyIdentifier = this._keyInfoSerializer.ReadKeyIdentifier((XmlReader) reader);
        else
          this._keyInfo.ReadXml(reader);
      }
      int content3 = (int) reader.MoveToContent();
      this._cipherData.ReadXml(reader);
      this.ReadExtensions(reader);
      int content4 = (int) reader.MoveToContent();
      reader.ReadEndElement();
    }
  }
}
