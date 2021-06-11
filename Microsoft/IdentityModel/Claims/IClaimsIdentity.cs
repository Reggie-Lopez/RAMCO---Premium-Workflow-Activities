// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.IClaimsIdentity
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Microsoft.IdentityModel.Claims
{
  [ComVisible(true)]
  public interface IClaimsIdentity : IIdentity
  {
    ClaimCollection Claims { get; }

    IClaimsIdentity Actor { get; set; }

    string Label { get; set; }

    string NameClaimType { get; set; }

    string RoleClaimType { get; set; }

    SecurityToken BootstrapToken { get; set; }

    IClaimsIdentity Copy();
  }
}
