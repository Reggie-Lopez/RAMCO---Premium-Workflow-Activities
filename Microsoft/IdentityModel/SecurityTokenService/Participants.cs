// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.SecurityTokenService.Participants
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.SecurityTokenService
{
  [ComVisible(true)]
  public class Participants
  {
    private EndpointAddress _primary;
    private List<EndpointAddress> _participant = new List<EndpointAddress>();

    public EndpointAddress Primary
    {
      get => this._primary;
      set => this._primary = value;
    }

    public List<EndpointAddress> Participant => this._participant;
  }
}
