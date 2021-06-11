// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.SecurityTokenSpecificationEnumerable
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel.Security;

namespace Microsoft.IdentityModel.Tokens
{
  internal class SecurityTokenSpecificationEnumerable : 
    IEnumerable<SecurityTokenSpecification>,
    IEnumerable
  {
    private SecurityMessageProperty _securityMessageProperty;

    public SecurityTokenSpecificationEnumerable(SecurityMessageProperty securityMessageProperty) => this._securityMessageProperty = securityMessageProperty != null ? securityMessageProperty : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityMessageProperty));

    public IEnumerator<SecurityTokenSpecification> GetEnumerator()
    {
      if (this._securityMessageProperty.InitiatorToken != null)
        yield return this._securityMessageProperty.InitiatorToken;
      if (this._securityMessageProperty.ProtectionToken != null)
        yield return this._securityMessageProperty.ProtectionToken;
      if (this._securityMessageProperty.HasIncomingSupportingTokens)
      {
        foreach (SecurityTokenSpecification incomingSupportingToken in this._securityMessageProperty.IncomingSupportingTokens)
        {
          if (incomingSupportingToken != null)
            yield return incomingSupportingToken;
        }
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotImplementedException());
  }
}
