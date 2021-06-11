// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Diagnostics.WSFedMessageTraceRecord
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.WSFederation;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
  internal class WSFedMessageTraceRecord : TraceRecord
  {
    internal new const string ElementName = "WSFederationMessageTraceRecord";
    internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/WSFederationMessageTraceRecord";
    private WSFederationMessage _wsFederationMessage;

    public WSFedMessageTraceRecord(WSFederationMessage wsFederationMessage) => this._wsFederationMessage = wsFederationMessage;

    public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/WSFederationMessageTraceRecord";

    public override void WriteTo(XmlWriter writer)
    {
      writer.WriteStartElement("WSFederationMessageTraceRecord");
      writer.WriteAttributeString("xmlns", this.EventId);
      writer.WriteStartElement("WSFederationMessage");
      writer.WriteElementString("BaseUri", this._wsFederationMessage.BaseUri.AbsoluteUri);
      foreach (string key in (IEnumerable<string>) this._wsFederationMessage.Parameters.Keys)
      {
        if (StringComparer.OrdinalIgnoreCase.Equals(key, "wresult"))
          PlainXmlWriter.WriteDecoded(key, this._wsFederationMessage.Parameters[key] == null ? string.Empty : this._wsFederationMessage.Parameters[key], writer);
        else
          writer.WriteElementString(key, this._wsFederationMessage.Parameters[key] == null ? string.Empty : this._wsFederationMessage.Parameters[key]);
      }
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
  }
}
