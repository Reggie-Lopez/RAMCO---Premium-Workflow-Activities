// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.EndpointAuthorizationPolicy
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class EndpointAuthorizationPolicy : IAuthorizationPolicy, IAuthorizationComponent
  {
    private string _endpointId;
    private string _id = UniqueId.CreateUniqueId();

    public EndpointAuthorizationPolicy(string endpointId) => this._endpointId = endpointId != null ? endpointId : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (endpointId));

    public string EndpointId => this._endpointId;

    bool IAuthorizationPolicy.Evaluate(
      EvaluationContext evaluationContext,
      ref object state)
    {
      return true;
    }

    ClaimSet IAuthorizationPolicy.Issuer => (ClaimSet) null;

    string IAuthorizationComponent.Id => this._id;
  }
}
