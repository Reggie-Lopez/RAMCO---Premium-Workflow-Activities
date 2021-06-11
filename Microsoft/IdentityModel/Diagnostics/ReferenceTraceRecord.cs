// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Diagnostics.ReferenceTraceRecord
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
  internal class ReferenceTraceRecord : TraceRecord
  {
    internal new const string ElementName = "ReferenceTraceRecord";
    internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/ReferenceTraceRecord";
    private bool _areEqual;
    private byte[] _computedDigest;
    private byte[] _referenceDigest;
    private string _uri;

    public ReferenceTraceRecord(
      bool areEqual,
      byte[] computedDigest,
      byte[] referenceDigest,
      string uri)
    {
      this._areEqual = areEqual;
      this._computedDigest = computedDigest;
      this._referenceDigest = referenceDigest;
      this._uri = uri;
    }

    public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/ReferenceTraceRecord";

    public override void WriteTo(XmlWriter writer)
    {
      writer.WriteStartElement(nameof (ReferenceTraceRecord));
      writer.WriteAttributeString("xmlns", this.EventId);
      writer.WriteElementString("Reference", this._uri);
      writer.WriteElementString("Equal", this._areEqual.ToString());
      writer.WriteElementString("ComputedDigestBase64", Convert.ToBase64String(this._computedDigest));
      writer.WriteElementString("ReferenceDigestBase64", Convert.ToBase64String(this._referenceDigest));
      writer.WriteEndElement();
    }
  }
}
