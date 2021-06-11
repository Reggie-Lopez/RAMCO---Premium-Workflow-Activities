// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Diagnostics.DictionaryTraceRecord
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
  internal class DictionaryTraceRecord : TraceRecord
  {
    private IDictionary _dictionary;

    public DictionaryTraceRecord(IDictionary dictionary) => this._dictionary = dictionary;

    public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/DictionaryTraceRecord";

    public override void WriteTo(XmlWriter xml)
    {
      if (this._dictionary == null)
        return;
      foreach (object key in (IEnumerable) this._dictionary.Keys)
        xml.WriteElementString(key.ToString(), this._dictionary[key] == null ? string.Empty : this._dictionary[key].ToString());
    }
  }
}
