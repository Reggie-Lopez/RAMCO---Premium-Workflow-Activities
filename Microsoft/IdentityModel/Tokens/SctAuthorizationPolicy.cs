// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SctAuthorizationPolicy
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IdentityModel.Claims;
using System.IdentityModel.Policy;

namespace Microsoft.IdentityModel.Tokens
{
  internal class SctAuthorizationPolicy : IAuthorizationPolicy, IAuthorizationComponent
  {
    private ClaimSet _issuer;
    private string _id = UniqueId.CreateUniqueId();

    internal SctAuthorizationPolicy(Claim claim) => this._issuer = (ClaimSet) new DefaultClaimSet(new Claim[1]
    {
      claim
    });

    bool IAuthorizationPolicy.Evaluate(
      EvaluationContext evaluationContext,
      ref object state)
    {
      if (evaluationContext == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (evaluationContext));
      evaluationContext.AddClaimSet((IAuthorizationPolicy) this, this._issuer);
      return true;
    }

    ClaimSet IAuthorizationPolicy.Issuer => this._issuer;

    string IAuthorizationComponent.Id => this._id;
  }
}
