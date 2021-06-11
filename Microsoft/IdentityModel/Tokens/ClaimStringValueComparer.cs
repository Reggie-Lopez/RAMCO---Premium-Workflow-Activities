// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.ClaimStringValueComparer
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Tokens
{
  internal class ClaimStringValueComparer : IEqualityComparer<Claim>
  {
    public bool Equals(Claim claim1, Claim claim2)
    {
      if (object.ReferenceEquals((object) claim1, (object) claim2))
        return true;
      return claim1 != null && claim2 != null && (!(claim1.ClaimType != claim2.ClaimType) && !(claim1.Right != claim2.Right)) && StringComparer.OrdinalIgnoreCase.Equals(claim1.Resource, claim2.Resource);
    }

    public int GetHashCode(Claim claim)
    {
      if (claim == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (claim));
      return claim.ClaimType.GetHashCode() ^ claim.Right.GetHashCode() ^ (claim.Resource == null ? 0 : claim.Resource.GetHashCode());
    }
  }
}
