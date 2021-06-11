// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.RequestSecurityTokenResponse
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Protocols.WSIdentity;
using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class RequestSecurityTokenResponse : WSTrustMessage
  {
    private SecurityKeyIdentifierClause _requestedAttachedReference;
    private DisplayToken _requestedDisplayToken;
    private RequestedProofToken _requestedProofToken;
    private RequestedSecurityToken _requestedSecurityToken;
    private SecurityKeyIdentifierClause _requestedUnattachedReference;
    private bool _requestedTokenCancelled;
    private Status _status;
    private bool _isFinal = true;

    public RequestSecurityTokenResponse()
    {
    }

    public RequestSecurityTokenResponse(WSTrustMessage message)
    {
      this.RequestType = message != null ? message.RequestType : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (message));
      this.Context = message.Context;
      this.KeyType = message.KeyType;
      int? keySizeInBits = message.KeySizeInBits;
      if ((keySizeInBits.GetValueOrDefault() <= 0 ? 0 : (keySizeInBits.HasValue ? 1 : 0)) == 0 || !StringComparer.Ordinal.Equals(message.KeyType, "http://schemas.microsoft.com/idfx/keytype/symmetric"))
        return;
      this.KeySizeInBits = message.KeySizeInBits;
    }

    public bool IsFinal
    {
      get => this._isFinal;
      set => this._isFinal = value;
    }

    public SecurityKeyIdentifierClause RequestedAttachedReference
    {
      get => this._requestedAttachedReference;
      set => this._requestedAttachedReference = value;
    }

    public DisplayToken RequestedDisplayToken
    {
      get => this._requestedDisplayToken;
      set => this._requestedDisplayToken = value;
    }

    public RequestedSecurityToken RequestedSecurityToken
    {
      get => this._requestedSecurityToken;
      set => this._requestedSecurityToken = value;
    }

    public RequestedProofToken RequestedProofToken
    {
      get => this._requestedProofToken;
      set => this._requestedProofToken = value;
    }

    public SecurityKeyIdentifierClause RequestedUnattachedReference
    {
      get => this._requestedUnattachedReference;
      set => this._requestedUnattachedReference = value;
    }

    public bool RequestedTokenCancelled
    {
      get => this._requestedTokenCancelled;
      set => this._requestedTokenCancelled = value;
    }

    public Status Status
    {
      get => this._status;
      set => this._status = value;
    }
  }
}
