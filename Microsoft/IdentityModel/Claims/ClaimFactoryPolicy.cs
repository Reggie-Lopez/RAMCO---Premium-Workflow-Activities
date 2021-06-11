// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Claims.ClaimFactoryPolicy
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;

namespace Microsoft.IdentityModel.Claims
{
  internal class ClaimFactoryPolicy : IAuthorizationPolicy, IAuthorizationComponent
  {
    private ReadOnlyCollection<ClaimSet> _claimSets;
    private DateTime _expirationTime;
    private string _id = UniqueId.CreateUniqueId(nameof (ClaimFactoryPolicy));

    public ClaimFactoryPolicy(ReadOnlyCollection<ClaimSet> claimSets)
      : this(claimSets, DateTime.MaxValue)
    {
    }

    public ClaimFactoryPolicy(ReadOnlyCollection<ClaimSet> claimSets, DateTime expirationTime)
    {
      this._claimSets = claimSets ?? EmptyReadOnlyCollection<ClaimSet>.Instance;
      this._expirationTime = expirationTime;
    }

    public ClaimSet Issuer => ClaimSet.System;

    public bool Evaluate(EvaluationContext evaluationContext, ref object state)
    {
      if (evaluationContext == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (evaluationContext));
      evaluationContext.RecordExpirationTime(this._expirationTime);
      foreach (ClaimSet claimSet in this._claimSets)
        evaluationContext.AddClaimSet((IAuthorizationPolicy) this, claimSet);
      return true;
    }

    public string Id => this._id;
  }
}
