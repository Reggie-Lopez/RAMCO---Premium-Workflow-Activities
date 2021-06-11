// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.XmlEncryption.CipherDataElement
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlEncryption
{
  internal class CipherDataElement
  {
    private byte[] _iv;
    private byte[] _cipherText;

    public byte[] CipherValue
    {
      get
      {
        if (this._iv != null)
        {
          byte[] numArray = new byte[this._iv.Length + this._cipherText.Length];
          Buffer.BlockCopy((Array) this._iv, 0, (Array) numArray, 0, this._iv.Length);
          Buffer.BlockCopy((Array) this._cipherText, 0, (Array) numArray, this._iv.Length, this._cipherText.Length);
          this._iv = (byte[]) null;
        }
        return this._cipherText;
      }
      set => this._cipherText = value;
    }

    public void ReadXml(XmlDictionaryReader reader)
    {
      int num = reader != null ? (int) reader.MoveToContent() : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (reader));
      if (!reader.IsStartElement("CipherData", "http://www.w3.org/2001/04/xmlenc#"))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml((XmlReader) reader, Microsoft.IdentityModel.SR.GetString("ID4188"));
      reader.ReadStartElement("CipherData", "http://www.w3.org/2001/04/xmlenc#");
      reader.ReadStartElement("CipherValue", "http://www.w3.org/2001/04/xmlenc#");
      this._cipherText = reader.ReadContentAsBase64();
      this._iv = (byte[]) null;
      int content1 = (int) reader.MoveToContent();
      reader.ReadEndElement();
      int content2 = (int) reader.MoveToContent();
      reader.ReadEndElement();
    }

    public void SetCipherValueFragments(byte[] iv, byte[] cipherText)
    {
      this._iv = iv;
      this._cipherText = cipherText;
    }

    public void WriteXml(XmlWriter writer)
    {
      writer.WriteStartElement("xenc", "CipherData", "http://www.w3.org/2001/04/xmlenc#");
      writer.WriteStartElement("xenc", "CipherValue", "http://www.w3.org/2001/04/xmlenc#");
      if (this._iv != null)
        writer.WriteBase64(this._iv, 0, this._iv.Length);
      writer.WriteBase64(this._cipherText, 0, this._cipherText.Length);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
  }
}
