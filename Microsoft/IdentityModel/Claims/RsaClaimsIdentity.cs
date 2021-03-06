// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.RsaClaimsIdentity
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.IdentityModel.Claims
{
  [ComVisible(true)]
  [Serializable]
  public class RsaClaimsIdentity : ClaimsIdentity, ISerializable
  {
    public RsaClaimsIdentity()
    {
    }

    public RsaClaimsIdentity(IEnumerable<Claim> claims, string authenticationType)
      : base(claims, authenticationType)
    {
    }

    protected RsaClaimsIdentity(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    protected override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
  }
}
