// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSFederation.Metadata.ProtocolEndpoint
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
  [ComVisible(true)]
  public class ProtocolEndpoint
  {
    private Uri _binding;
    private Uri _location;
    private Uri _responseLocation;

    public ProtocolEndpoint()
      : this((Uri) null, (Uri) null)
    {
    }

    public ProtocolEndpoint(Uri binding, Uri location)
    {
      this.Binding = binding;
      this.Location = location;
    }

    public Uri Binding
    {
      get => this._binding;
      set => this._binding = value;
    }

    public Uri Location
    {
      get => this._location;
      set => this._location = value;
    }

    public Uri ResponseLocation
    {
      get => this._responseLocation;
      set => this._responseLocation = value;
    }
  }
}
