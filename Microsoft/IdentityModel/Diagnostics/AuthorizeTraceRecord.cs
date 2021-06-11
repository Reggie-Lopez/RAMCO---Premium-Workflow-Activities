// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Diagnostics.AuthorizeTraceRecord
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System.Web;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
  internal class AuthorizeTraceRecord : TraceRecord
  {
    internal new const string ElementName = "AuthorizeTraceRecord";
    internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/AuthorizeTraceRecord";
    private IClaimsPrincipal _claimsPrincipal;
    private string _url;
    private string _action;

    public AuthorizeTraceRecord(IClaimsPrincipal claimsPrincipal, HttpRequest request)
    {
      this._claimsPrincipal = claimsPrincipal;
      this._url = request.Url.AbsoluteUri;
      this._action = request.HttpMethod;
    }

    public AuthorizeTraceRecord(IClaimsPrincipal claimsPrincipal, string url, string action)
    {
      this._claimsPrincipal = claimsPrincipal;
      this._url = url;
      this._action = action;
    }

    public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/AuthorizeTraceRecord";

    public override void WriteTo(XmlWriter writer)
    {
      writer.WriteStartElement(nameof (AuthorizeTraceRecord));
      writer.WriteAttributeString("xmlns", this.EventId);
      writer.WriteStartElement("Authorize");
      writer.WriteElementString("Url", this._url);
      writer.WriteElementString("Action", this._action);
      writer.WriteStartElement("ClaimsPrincipal");
      writer.WriteAttributeString("Identity.Name", this._claimsPrincipal.Identity.Name);
      foreach (IClaimsIdentity identity in this._claimsPrincipal.Identities)
      {
        writer.WriteStartElement("ClaimsIdentity");
        writer.WriteAttributeString("name", identity.Name);
        foreach (Claim claim in identity.Claims)
        {
          writer.WriteStartElement("Claim");
          writer.WriteAttributeString("Value", claim.Value);
          writer.WriteAttributeString("Type", claim.ClaimType);
          writer.WriteAttributeString("ValueType", claim.ValueType);
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
  }
}
