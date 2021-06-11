// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.WrappedSessionSecurityTokenAuthenticator
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.ServiceModel.Security.Tokens;

namespace Microsoft.IdentityModel.Tokens
{
  internal class WrappedSessionSecurityTokenAuthenticator : 
    SecurityTokenAuthenticator,
    IIssuanceSecurityTokenAuthenticator,
    ICommunicationObject
  {
    private SessionSecurityTokenHandler _sessionTokenHandler;
    private IIssuanceSecurityTokenAuthenticator _issuanceSecurityTokenAuthenticator;
    private ICommunicationObject _communicationObject;
    private SctClaimsHandler _sctClaimsHandler;
    private Microsoft.IdentityModel.ExceptionMapper _exceptionMapper;

    public WrappedSessionSecurityTokenAuthenticator(
      SessionSecurityTokenHandler sessionTokenHandler,
      SecurityTokenAuthenticator wcfSessionAuthenticator,
      SctClaimsHandler sctClaimsHandler,
      Microsoft.IdentityModel.ExceptionMapper exceptionMapper)
    {
      if (sessionTokenHandler == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (sessionTokenHandler));
      if (wcfSessionAuthenticator == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (wcfSessionAuthenticator));
      if (sctClaimsHandler == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (sctClaimsHandler));
      if (exceptionMapper == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (exceptionMapper));
      this._issuanceSecurityTokenAuthenticator = wcfSessionAuthenticator as IIssuanceSecurityTokenAuthenticator;
      if (this._issuanceSecurityTokenAuthenticator == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4244"));
      this._communicationObject = wcfSessionAuthenticator as ICommunicationObject;
      if (this._communicationObject == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID4245"));
      this._sessionTokenHandler = sessionTokenHandler;
      this._sctClaimsHandler = sctClaimsHandler;
      this._exceptionMapper = exceptionMapper;
    }

    protected override ReadOnlyCollection<IAuthorizationPolicy> ValidateTokenCore(
      SecurityToken token)
    {
      SessionSecurityToken token1 = new SessionSecurityToken(token as SecurityContextSecurityToken, SessionSecurityTokenHandler.DefaultSecureConversationVersion);
      ClaimsIdentityCollection identityCollection = (ClaimsIdentityCollection) null;
      try
      {
        identityCollection = this._sessionTokenHandler.ValidateToken(token1, this._sctClaimsHandler.EndpointId);
      }
      catch (Exception ex)
      {
        if (!this._exceptionMapper.HandleSecurityTokenProcessingException(ex))
          throw;
      }
      return new List<IAuthorizationPolicy>((IEnumerable<IAuthorizationPolicy>) new AuthorizationPolicy[1]
      {
        new AuthorizationPolicy(identityCollection)
      }).AsReadOnly();
    }

    protected override bool CanValidateTokenCore(SecurityToken token) => token is SecurityContextSecurityToken;

    public IssuedSecurityTokenHandler IssuedSecurityTokenHandler
    {
      get => this._issuanceSecurityTokenAuthenticator.IssuedSecurityTokenHandler;
      set => this._issuanceSecurityTokenAuthenticator.IssuedSecurityTokenHandler = value;
    }

    public RenewedSecurityTokenHandler RenewedSecurityTokenHandler
    {
      get => this._issuanceSecurityTokenAuthenticator.RenewedSecurityTokenHandler;
      set => this._issuanceSecurityTokenAuthenticator.RenewedSecurityTokenHandler = value;
    }

    public void Abort() => this._communicationObject.Abort();

    public IAsyncResult BeginClose(
      TimeSpan timeout,
      AsyncCallback callback,
      object state)
    {
      return this._communicationObject.BeginClose(timeout, callback, state);
    }

    public IAsyncResult BeginClose(AsyncCallback callback, object state) => this._communicationObject.BeginClose(callback, state);

    public IAsyncResult BeginOpen(
      TimeSpan timeout,
      AsyncCallback callback,
      object state)
    {
      return this._communicationObject.BeginOpen(timeout, callback, state);
    }

    public IAsyncResult BeginOpen(AsyncCallback callback, object state) => this._communicationObject.BeginOpen(callback, state);

    public void Close(TimeSpan timeout) => this._communicationObject.Close(timeout);

    public void Close() => this._communicationObject.Close();

    public event EventHandler Closed
    {
      add => this._communicationObject.Closed += value;
      remove => this._communicationObject.Closed -= value;
    }

    public event EventHandler Closing
    {
      add => this._communicationObject.Closing += value;
      remove => this._communicationObject.Closing -= value;
    }

    public void EndClose(IAsyncResult result) => this._communicationObject.EndClose(result);

    public void EndOpen(IAsyncResult result) => this._communicationObject.EndOpen(result);

    public event EventHandler Faulted
    {
      add => this._communicationObject.Faulted += value;
      remove => this._communicationObject.Faulted -= value;
    }

    public void Open(TimeSpan timeout) => this._communicationObject.Open(timeout);

    public void Open() => this._communicationObject.Open();

    public event EventHandler Opened
    {
      add => this._communicationObject.Opened += value;
      remove => this._communicationObject.Opened -= value;
    }

    public event EventHandler Opening
    {
      add => this._communicationObject.Opening += value;
      remove => this._communicationObject.Opening -= value;
    }

    public CommunicationState State => this._communicationObject.State;
  }
}
