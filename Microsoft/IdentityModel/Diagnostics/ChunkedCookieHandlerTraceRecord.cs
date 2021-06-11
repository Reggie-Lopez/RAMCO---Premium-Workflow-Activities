// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Diagnostics.ChunkedCookieHandlerTraceRecord
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Globalization;
using System.Web;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
  internal class ChunkedCookieHandlerTraceRecord : TraceRecord
  {
    internal new const string ElementName = "ChunkedCookieHandlerTraceRecord";
    internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/ChunkedCookieHandlerTraceRecord";
    private ChunkedCookieHandlerTraceRecord.Action _action;
    private HttpCookie _cookie;
    private string _cookiePath;

    public ChunkedCookieHandlerTraceRecord(
      ChunkedCookieHandlerTraceRecord.Action action,
      HttpCookie cookie,
      string cookiePath)
    {
      this._action = action;
      this._cookie = cookie;
      this._cookiePath = cookiePath;
    }

    public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/DeflateCookieTraceRecord";

    public override void WriteTo(XmlWriter writer)
    {
      string str = "unknown";
      if (this._action == ChunkedCookieHandlerTraceRecord.Action.Writing)
        str = Microsoft.IdentityModel.SR.GetString("TraceChunkedCookieHandlerWriting");
      else if (this._action == ChunkedCookieHandlerTraceRecord.Action.Reading)
        str = Microsoft.IdentityModel.SR.GetString("TraceChunkedCookieHandlerReading");
      else if (this._action == ChunkedCookieHandlerTraceRecord.Action.Deleting)
        str = Microsoft.IdentityModel.SR.GetString("TraceChunkedCookieHandlerDeleting");
      writer.WriteStartElement(nameof (ChunkedCookieHandlerTraceRecord));
      writer.WriteAttributeString("xmlns", this.EventId);
      writer.WriteAttributeString("Action", str);
      if (!string.IsNullOrEmpty(this._cookie.Name))
        writer.WriteElementString("Name", this._cookie.Name);
      if (this._action == ChunkedCookieHandlerTraceRecord.Action.Writing || this._action == ChunkedCookieHandlerTraceRecord.Action.Deleting)
      {
        if (!string.IsNullOrEmpty(this._cookie.Path))
          writer.WriteElementString("Path", this._cookiePath);
        if (!string.IsNullOrEmpty(this._cookie.Domain))
          writer.WriteElementString("Domain", this._cookie.Domain);
        if (this._action == ChunkedCookieHandlerTraceRecord.Action.Writing)
        {
          if (this._cookie.Expires == DateTime.MinValue)
            writer.WriteElementString("Expires", "Session");
          else
            writer.WriteElementString("Expires", this._cookie.Expires.ToString((IFormatProvider) DateTimeFormatInfo.InvariantInfo));
          writer.WriteElementString("Secure", this._cookie.Secure.ToString((IFormatProvider) DateTimeFormatInfo.InvariantInfo));
          writer.WriteElementString("HttpOnly", this._cookie.HttpOnly.ToString((IFormatProvider) DateTimeFormatInfo.InvariantInfo));
        }
      }
      writer.WriteEndElement();
    }

    public enum Action
    {
      Reading,
      Writing,
      Deleting,
    }
  }
}
