// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.AuthorizationPolicy
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class AuthorizationPolicy : IAuthorizationPolicy, IAuthorizationComponent
  {
    public const string PrincipalKey = "Principal";
    public const string IdentitiesKey = "Identities";
    private ClaimsIdentityCollection _identityCollection = new ClaimsIdentityCollection();
    private ClaimSet _issuer = (ClaimSet) new DefaultClaimSet(new System.IdentityModel.Claims.Claim[1]
    {
      new System.IdentityModel.Claims.Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims", (object) null, Rights.Identity)
    });
    private string _id = UniqueId.CreateUniqueId();

    public AuthorizationPolicy()
    {
    }

    public AuthorizationPolicy(IClaimsIdentity identity)
    {
      if (identity == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (identity));
      this._identityCollection.Add(identity);
    }

    public AuthorizationPolicy(ClaimsIdentityCollection identityCollection) => this._identityCollection = identityCollection != null ? identityCollection : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (identityCollection));

    public ClaimsIdentityCollection IdentityCollection => this._identityCollection;

    public bool Evaluate(EvaluationContext evaluationContext, ref object state)
    {
      if (evaluationContext == null || evaluationContext.Properties == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (evaluationContext));
      if (this._identityCollection.Count == 0)
        return true;
      object obj1 = (object) null;
      if (!evaluationContext.Properties.TryGetValue("Principal", out obj1))
      {
        IClaimsPrincipal fromIdentities = ClaimsPrincipal.CreateFromIdentities(this._identityCollection);
        evaluationContext.Properties.Add("Principal", (object) fromIdentities);
        if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
          DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, Microsoft.IdentityModel.SR.GetString("TraceSetPrincipalOnEvaluationContext"), (TraceRecord) new ClaimsPrincipalTraceRecord(fromIdentities), (Exception) null);
      }
      else if (obj1 is IClaimsPrincipal claimsPrincipal && claimsPrincipal.Identities != null)
        claimsPrincipal.Identities.AddRange((IEnumerable<IClaimsIdentity>) this._identityCollection);
      else if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Error))
        DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Error, Microsoft.IdentityModel.SR.GetString("ID8004", (object) "Principal"));
      object obj2 = (object) null;
      if (!evaluationContext.Properties.TryGetValue("Identities", out obj2))
      {
        List<IIdentity> identityList = new List<IIdentity>();
        foreach (IClaimsIdentity identity in this._identityCollection)
          identityList.Add((IIdentity) identity);
        evaluationContext.Properties.Add("Identities", (object) identityList);
      }
      else
      {
        List<IIdentity> identityList = obj2 as List<IIdentity>;
        foreach (IClaimsIdentity identity in this._identityCollection)
          identityList.Add((IIdentity) identity);
      }
      return true;
    }

    public ClaimSet Issuer => this._issuer;

    public string Id => this._id;
  }
}
