// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Diagnostics.DeflateCookieTraceRecord
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Globalization;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
  internal class DeflateCookieTraceRecord : TraceRecord
  {
    internal new const string ElementName = "DeflateCookieTraceRecord";
    internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/DeflateCookieTraceRecord";
    private int _originalSize;
    private int _deflatedSize;

    public DeflateCookieTraceRecord(int originalSize, int deflatedSize)
    {
      this._originalSize = originalSize;
      this._deflatedSize = deflatedSize;
    }

    public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/DeflateCookieTraceRecord";

    public override void WriteTo(XmlWriter writer)
    {
      writer.WriteStartElement(nameof (DeflateCookieTraceRecord));
      writer.WriteAttributeString("xmlns", this.EventId);
      writer.WriteElementString("OriginalSize", this._originalSize.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      writer.WriteElementString("AfterDeflating", this._deflatedSize.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      writer.WriteEndElement();
    }
  }
}
