// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Diagnostics.PassiveMessageTraceRecord
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
  internal class PassiveMessageTraceRecord : TraceRecord
  {
    internal new const string ElementName = "PassiveMessageTraceRecord";
    internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/PassiveMessageTraceRecord";
    private IDictionary<string, string> _dictionary;

    public PassiveMessageTraceRecord(IDictionary<string, string> dictionary) => this._dictionary = dictionary;

    public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/PassiveMessageTraceRecord";

    public override void WriteTo(XmlWriter writer)
    {
      writer.WriteStartElement(nameof (PassiveMessageTraceRecord));
      writer.WriteAttributeString("xmlns", this.EventId);
      writer.WriteStartElement("Request");
      foreach (string key in (IEnumerable<string>) this._dictionary.Keys)
      {
        if (StringComparer.OrdinalIgnoreCase.Equals(key, "wresult"))
          PlainXmlWriter.WriteDecoded(key, this._dictionary[key] == null ? string.Empty : this._dictionary[key], writer);
        else
          writer.WriteElementString(key, this._dictionary[key] == null ? string.Empty : this._dictionary[key]);
      }
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
  }
}
