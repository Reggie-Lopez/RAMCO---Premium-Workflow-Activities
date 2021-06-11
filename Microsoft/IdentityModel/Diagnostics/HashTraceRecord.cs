// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Diagnostics.HashTraceRecord
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
  internal class HashTraceRecord : TraceRecord
  {
    internal new const string ElementName = "HashTraceRecord";
    internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/HashTraceRecord";
    private static readonly char[] hexDigits = new char[16]
    {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F'
    };
    private byte[] _octets;
    private byte[] _untranslatedOctets;
    private string _hash;

    public HashTraceRecord(string hash, byte[] octets, byte[] untranslatedOctets)
    {
      this._hash = hash;
      this._octets = octets;
      this._untranslatedOctets = untranslatedOctets;
    }

    public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/HashTraceRecord";

    public override void WriteTo(XmlWriter writer)
    {
      writer.WriteStartElement(nameof (HashTraceRecord));
      writer.WriteAttributeString("xmlns", this.EventId);
      if (this._untranslatedOctets != null)
        this.WriteBytes(this._untranslatedOctets, "PreCanonicalBytes", writer);
      this.WriteBytes(this._octets, "CanonicalBytes", writer);
      writer.WriteStartElement("Hash");
      writer.WriteElementString("Length", this._hash.Length.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      writer.WriteElementString("Value", this._hash);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }

    private void WriteBytes(byte[] bytes, string startElementName, XmlWriter writer)
    {
      writer.WriteStartElement(startElementName);
      writer.WriteElementString("Length", bytes.Length.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < bytes.Length; ++index)
      {
        byte num = bytes[index];
        stringBuilder.Append(HashTraceRecord.hexDigits[(int) num / 16 & 15]);
        stringBuilder.Append(HashTraceRecord.hexDigits[(int) num & 15]);
      }
      writer.WriteElementString("HexBytes", stringBuilder.ToString());
      writer.WriteElementString("Encoding.UTF8", Encoding.UTF8.GetString(bytes));
      writer.WriteEndElement();
    }
  }
}
