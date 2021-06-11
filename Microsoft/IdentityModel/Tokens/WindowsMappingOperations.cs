// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.WindowsMappingOperations
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System;
using System.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Tokens
{
  internal static class WindowsMappingOperations
  {
    public static string FindUpn(IClaimsIdentity claimsIdentity)
    {
      string str = (string) null;
      foreach (Claim claim in claimsIdentity.Claims)
      {
        if (StringComparer.Ordinal.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn", claim.ClaimType))
          str = str == null ? claim.Value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID1053")));
      }
      return !string.IsNullOrEmpty(str) ? str : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new SecurityTokenException(Microsoft.IdentityModel.SR.GetString("ID1054")));
    }
  }
}
