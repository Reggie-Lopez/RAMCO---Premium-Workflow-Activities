﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Diagnostics.TraceRecord
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
  internal abstract class TraceRecord
  {
    protected const string EventIdBase = "http://schemas.microsoft.com/2009/06/IdentityModel/";
    protected const string ElementName = "TraceRecord";
    internal const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/EmptyTraceRecord";

    public virtual string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/EmptyTraceRecord";

    public abstract void WriteTo(XmlWriter writer);
  }
}