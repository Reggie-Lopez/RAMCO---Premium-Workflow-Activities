// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.DispatchContext
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class DispatchContext
  {
    private IClaimsPrincipal _principal;
    private string _requestAction;
    private WSTrustMessage _requestMessage;
    private string _responseAction;
    private RequestSecurityTokenResponse _responseMessage;
    private Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService _securityTokenService;
    private string _trustNamespace;

    public IClaimsPrincipal Principal
    {
      get => this._principal;
      set => this._principal = value;
    }

    public string RequestAction
    {
      get => this._requestAction;
      set => this._requestAction = value;
    }

    public WSTrustMessage RequestMessage
    {
      get => this._requestMessage;
      set => this._requestMessage = value;
    }

    public string ResponseAction
    {
      get => this._responseAction;
      set => this._responseAction = value;
    }

    public RequestSecurityTokenResponse ResponseMessage
    {
      get => this._responseMessage;
      set => this._responseMessage = value;
    }

    public Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService SecurityTokenService
    {
      get => this._securityTokenService;
      set => this._securityTokenService = value;
    }

    public string TrustNamespace
    {
      get => this._trustNamespace;
      set => this._trustNamespace = value;
    }
  }
}
