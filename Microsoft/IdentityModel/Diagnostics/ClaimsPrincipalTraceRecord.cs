// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Diagnostics.ClaimsPrincipalTraceRecord
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
  internal class ClaimsPrincipalTraceRecord : TraceRecord
  {
    internal new const string ElementName = "ClaimsPrincipalTraceRecord";
    internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/ClaimsPrincipalTraceRecord";
    private IClaimsPrincipal _claimsPrincipal;

    public ClaimsPrincipalTraceRecord(IClaimsPrincipal claimsPrincipal) => this._claimsPrincipal = claimsPrincipal;

    public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/ClaimsPrincipalTraceRecord";

    public override void WriteTo(XmlWriter writer)
    {
      writer.WriteStartElement(nameof (ClaimsPrincipalTraceRecord));
      writer.WriteAttributeString("xmlns", this.EventId);
      writer.WriteStartElement("ClaimsPrincipal");
      writer.WriteAttributeString("Identity.Name", this._claimsPrincipal.Identity.Name);
      foreach (IClaimsIdentity identity in this._claimsPrincipal.Identities)
        this.WriteClaimsIdentity(identity, writer);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }

    private void WriteClaimsIdentity(IClaimsIdentity ci, XmlWriter writer)
    {
      writer.WriteStartElement("ClaimsIdentity");
      writer.WriteAttributeString("Name", ci.Name);
      writer.WriteAttributeString("NameClaimType", ci.NameClaimType);
      writer.WriteAttributeString("RoleClaimType", ci.RoleClaimType);
      writer.WriteAttributeString("Label", ci.Label);
      if (ci.Actor != null)
      {
        writer.WriteStartElement("Actor");
        this.WriteClaimsIdentity(ci.Actor, writer);
        writer.WriteEndElement();
      }
      foreach (Claim claim in ci.Claims)
      {
        writer.WriteStartElement("Claim");
        writer.WriteAttributeString("Value", claim.Value);
        writer.WriteAttributeString("Type", claim.ClaimType);
        writer.WriteAttributeString("ValueType", claim.ValueType);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
  }
}
