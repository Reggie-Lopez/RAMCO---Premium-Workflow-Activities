// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrustServiceContract
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Security;
using System.Threading;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Schema;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, Name = "SecurityTokenService", Namespace = "http://schemas.microsoft.com/ws/2008/06/identity/securitytokenservice")]
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  [ComVisible(true)]
  public class WSTrustServiceContract : 
    IWSTrustFeb2005SyncContract,
    IWSTrust13SyncContract,
    IWSTrustFeb2005AsyncContract,
    IWSTrust13AsyncContract,
    IWsdlExportExtension,
    IContractBehavior
  {
    private const string soap11Namespace = "http://schemas.xmlsoap.org/soap/envelope/";
    private const string soap12Namespace = "http://www.w3.org/2003/05/soap-envelope";
    private SecurityTokenServiceConfiguration _securityTokenServiceConfiguration;

    private event EventHandler<WSTrustRequestProcessingErrorEventArgs> _requestFailed;

    public WSTrustServiceContract(
      SecurityTokenServiceConfiguration securityTokenServiceConfiguration)
    {
      this._securityTokenServiceConfiguration = securityTokenServiceConfiguration != null ? securityTokenServiceConfiguration : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityTokenServiceConfiguration));
    }

    public event EventHandler<WSTrustRequestProcessingErrorEventArgs> RequestFailed
    {
      add => this._requestFailed += value;
      remove => this._requestFailed -= value;
    }

    protected virtual SecurityTokenResolver GetSecurityHeaderTokenResolver(
      RequestContext requestContext)
    {
      if (requestContext == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestContext));
      List<SecurityToken> securityTokenList = new List<SecurityToken>();
      if (requestContext.RequestMessage != null && requestContext.RequestMessage.Properties != null && requestContext.RequestMessage.Properties.Security != null)
      {
        SecurityMessageProperty security = requestContext.RequestMessage.Properties.Security;
        if (security.ProtectionToken != null)
          securityTokenList.Add(security.ProtectionToken.SecurityToken);
        if (security.HasIncomingSupportingTokens)
        {
          foreach (SupportingTokenSpecification incomingSupportingToken in security.IncomingSupportingTokens)
          {
            if (incomingSupportingToken != null && (incomingSupportingToken.SecurityTokenAttachmentMode == SecurityTokenAttachmentMode.Endorsing || incomingSupportingToken.SecurityTokenAttachmentMode == SecurityTokenAttachmentMode.SignedEndorsing))
              securityTokenList.Add(incomingSupportingToken.SecurityToken);
          }
        }
        if (security.InitiatorToken != null)
          securityTokenList.Add(security.InitiatorToken.SecurityToken);
      }
      return securityTokenList.Count > 0 ? SecurityTokenResolver.CreateDefaultSecurityTokenResolver(securityTokenList.AsReadOnly(), true) : Microsoft.IdentityModel.Tokens.EmptySecurityTokenResolver.Instance;
    }

    protected virtual SecurityTokenResolver GetRstSecurityTokenResolver()
    {
      SecurityTokenResolver aggregateTokenResolver = this._securityTokenServiceConfiguration.CreateAggregateTokenResolver();
      if (this._securityTokenServiceConfiguration != null && aggregateTokenResolver != null && !object.ReferenceEquals((object) aggregateTokenResolver, (object) Microsoft.IdentityModel.Tokens.EmptySecurityTokenResolver.Instance))
        return aggregateTokenResolver;
      if (OperationContext.Current != null && OperationContext.Current.Host != null && OperationContext.Current.Host.Description != null)
      {
        ServiceCredentials serviceCredentials = OperationContext.Current.Host.Description.Behaviors.Find<ServiceCredentials>();
        if (serviceCredentials != null && serviceCredentials.ServiceCertificate != null && serviceCredentials.ServiceCertificate.Certificate != null)
          return SecurityTokenResolver.CreateDefaultSecurityTokenResolver(new List<SecurityToken>(1)
          {
            (SecurityToken) new X509SecurityToken(serviceCredentials.ServiceCertificate.Certificate)
          }.AsReadOnly(), false);
      }
      return Microsoft.IdentityModel.Tokens.EmptySecurityTokenResolver.Instance;
    }

    protected virtual WSTrustSerializationContext CreateSerializationContext() => new WSTrustSerializationContext(this._securityTokenServiceConfiguration.SecurityTokenHandlerCollectionManager, this.GetRstSecurityTokenResolver(), this.GetSecurityHeaderTokenResolver(OperationContext.Current.RequestContext));

    protected virtual IAsyncResult BeginDispatchRequest(
      DispatchContext dispatchContext,
      AsyncCallback asyncCallback,
      object asyncState)
    {
      return (IAsyncResult) new WSTrustServiceContract.DispatchRequestAsyncResult(dispatchContext, asyncCallback, asyncState);
    }

    protected virtual DispatchContext EndDispatchRequest(IAsyncResult ar) => WSTrustServiceContract.DispatchRequestAsyncResult.End(ar);

    protected virtual void DispatchRequest(DispatchContext dispatchContext)
    {
      RequestSecurityToken requestMessage = dispatchContext.RequestMessage as RequestSecurityToken;
      Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService securityTokenService = dispatchContext.SecurityTokenService;
      IClaimsPrincipal principal = dispatchContext.Principal;
      if (requestMessage == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID3022")));
      switch (requestMessage.RequestType)
      {
        case "http://schemas.microsoft.com/idfx/requesttype/cancel":
          dispatchContext.ResponseMessage = securityTokenService.Cancel(principal, requestMessage);
          break;
        case "http://schemas.microsoft.com/idfx/requesttype/issue":
          dispatchContext.ResponseMessage = securityTokenService.Issue(principal, requestMessage);
          break;
        case "http://schemas.microsoft.com/idfx/requesttype/renew":
          dispatchContext.ResponseMessage = securityTokenService.Renew(principal, requestMessage);
          break;
        case "http://schemas.microsoft.com/idfx/requesttype/validate":
          dispatchContext.ResponseMessage = securityTokenService.Validate(principal, requestMessage);
          break;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID3112", (object) requestMessage.RequestType)));
      }
    }

    protected virtual System.ServiceModel.Channels.Message ProcessCore(
      System.ServiceModel.Channels.Message requestMessage,
      WSTrustRequestSerializer requestSerializer,
      WSTrustResponseSerializer responseSerializer,
      string requestAction,
      string responseAction,
      string trustNamespace)
    {
      if (requestMessage == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestMessage));
      if (requestSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestSerializer));
      if (responseSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (responseSerializer));
      if (string.IsNullOrEmpty(requestAction))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestAction));
      if (string.IsNullOrEmpty(responseAction))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (responseAction));
      if (string.IsNullOrEmpty(trustNamespace))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustNamespace));
      System.ServiceModel.Channels.Message message = (System.ServiceModel.Channels.Message) null;
      try
      {
        WSTrustSerializationContext serializationContext = this.CreateSerializationContext();
        DispatchContext dispatchContext = this.CreateDispatchContext(requestMessage, requestAction, responseAction, trustNamespace, requestSerializer, responseSerializer, serializationContext);
        this.ValidateDispatchContext(dispatchContext);
        this.DispatchRequest(dispatchContext);
        message = System.ServiceModel.Channels.Message.CreateMessage(OperationContext.Current.RequestContext.RequestMessage.Version, dispatchContext.ResponseAction, (BodyWriter) new WSTrustResponseBodyWriter(dispatchContext.ResponseMessage, responseSerializer, serializationContext));
      }
      catch (Exception ex)
      {
        if (!this.HandleException(ex, trustNamespace, requestAction, requestMessage.Version.Envelope))
          throw;
      }
      return message;
    }

    protected virtual DispatchContext CreateDispatchContext(
      System.ServiceModel.Channels.Message requestMessage,
      string requestAction,
      string responseAction,
      string trustNamespace,
      WSTrustRequestSerializer requestSerializer,
      WSTrustResponseSerializer responseSerializer,
      WSTrustSerializationContext serializationContext)
    {
      DispatchContext dispatchContext = new DispatchContext()
      {
        Principal = Thread.CurrentPrincipal as IClaimsPrincipal,
        RequestAction = requestAction,
        ResponseAction = responseAction,
        TrustNamespace = trustNamespace
      };
      XmlReader readerAtBodyContents = (XmlReader) requestMessage.GetReaderAtBodyContents();
      if (requestSerializer.CanRead(readerAtBodyContents))
      {
        dispatchContext.RequestMessage = (WSTrustMessage) requestSerializer.ReadXml(readerAtBodyContents, serializationContext);
      }
      else
      {
        if (!responseSerializer.CanRead(readerAtBodyContents))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID3114")));
        dispatchContext.RequestMessage = (WSTrustMessage) responseSerializer.ReadXml(readerAtBodyContents, serializationContext);
      }
      dispatchContext.SecurityTokenService = this.CreateSTS();
      return dispatchContext;
    }

    protected virtual void ValidateDispatchContext(DispatchContext dispatchContext)
    {
      if (dispatchContext.RequestMessage is RequestSecurityToken && !WSTrustServiceContract.IsValidRSTAction(dispatchContext))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID3113", (object) "RequestSecurityToken", (object) dispatchContext.RequestAction)));
      if (dispatchContext.RequestMessage is RequestSecurityTokenResponse && !WSTrustServiceContract.IsValidRSTRAction(dispatchContext))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID3113", (object) "RequestSecurityTokenResponse", (object) dispatchContext.RequestAction)));
    }

    private static bool IsValidRSTAction(DispatchContext dispatchContext)
    {
      bool flag = false;
      string requestAction = dispatchContext.RequestAction;
      if (dispatchContext.TrustNamespace == "http://docs.oasis-open.org/ws-sx/ws-trust/200512")
      {
        switch (requestAction)
        {
          case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Cancel":
          case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue":
          case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Renew":
          case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Validate":
            flag = true;
            break;
        }
      }
      if (dispatchContext.TrustNamespace == "http://schemas.xmlsoap.org/ws/2005/02/trust")
      {
        switch (requestAction)
        {
          case "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Cancel":
          case "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue":
          case "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Renew":
          case "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Validate":
            flag = true;
            break;
        }
      }
      return flag;
    }

    private static bool IsValidRSTRAction(DispatchContext dispatchContext)
    {
      bool flag = false;
      string requestAction = dispatchContext.RequestAction;
      if (dispatchContext.TrustNamespace == "http://docs.oasis-open.org/ws-sx/ws-trust/200512")
      {
        switch (requestAction)
        {
          case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal":
          case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Cancel":
          case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal":
          case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Issue":
          case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal":
          case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Renew":
          case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal":
          case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Validate":
            flag = true;
            break;
        }
      }
      if (dispatchContext.TrustNamespace == "http://schemas.xmlsoap.org/ws/2005/02/trust")
      {
        switch (requestAction)
        {
          case "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel":
          case "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue":
          case "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew":
          case "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate":
            flag = true;
            break;
        }
      }
      return flag;
    }

    private Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService CreateSTS() => this._securityTokenServiceConfiguration.CreateSecurityTokenService() ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID3002")));

    protected virtual IAsyncResult BeginProcessCore(
      System.ServiceModel.Channels.Message requestMessage,
      WSTrustRequestSerializer requestSerializer,
      WSTrustResponseSerializer responseSerializer,
      string requestAction,
      string responseAction,
      string trustNamespace,
      AsyncCallback callback,
      object state)
    {
      if (requestMessage == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
      if (requestSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestSerializer));
      if (responseSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (responseSerializer));
      if (string.IsNullOrEmpty(requestAction))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestAction));
      if (string.IsNullOrEmpty(responseAction))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (responseAction));
      if (string.IsNullOrEmpty(trustNamespace))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustNamespace));
      IAsyncResult asyncResult = (IAsyncResult) null;
      try
      {
        WSTrustSerializationContext serializationContext = this.CreateSerializationContext();
        DispatchContext dispatchContext = this.CreateDispatchContext(requestMessage, requestAction, responseAction, trustNamespace, requestSerializer, responseSerializer, serializationContext);
        this.ValidateDispatchContext(dispatchContext);
        asyncResult = (IAsyncResult) new WSTrustServiceContract.ProcessCoreAsyncResult(this, dispatchContext, OperationContext.Current.RequestContext.RequestMessage.Version, responseSerializer, serializationContext, callback, state);
      }
      catch (Exception ex)
      {
        if (!this.HandleException(ex, trustNamespace, requestAction, requestMessage.Version.Envelope))
          throw;
      }
      return asyncResult;
    }

    protected virtual System.ServiceModel.Channels.Message EndProcessCore(
      IAsyncResult ar,
      string requestAction,
      string responseAction,
      string trustNamespace)
    {
      if (ar == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (ar));
      if (!(ar is WSTrustServiceContract.ProcessCoreAsyncResult processCoreAsyncResult))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new ArgumentException(Microsoft.IdentityModel.SR.GetString("ID2004", (object) typeof (WSTrustServiceContract.ProcessCoreAsyncResult), (object) ar.GetType()), nameof (ar)));
      System.ServiceModel.Channels.Message message = (System.ServiceModel.Channels.Message) null;
      try
      {
        message = WSTrustServiceContract.ProcessCoreAsyncResult.End(ar);
      }
      catch (Exception ex)
      {
        if (!this.HandleException(ex, trustNamespace, requestAction, processCoreAsyncResult.MessageVersion.Envelope))
          throw;
      }
      return message;
    }

    protected virtual bool HandleException(
      Exception ex,
      string trustNamespace,
      string action,
      EnvelopeVersion requestEnvelopeVersion)
    {
      if (DiagnosticUtil.IsFatal(ex))
        return false;
      if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
        DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, "RequestFailed: TrustNamespace={0}, Action={1}, Exception={2}", (object) trustNamespace, (object) action, (object) ex);
      if (this._requestFailed != null)
        this._requestFailed((object) this, new WSTrustRequestProcessingErrorEventArgs(action, ex));
      bool flag = false;
      ServiceDebugBehavior serviceDebugBehavior = OperationContext.Current.Host.Description.Behaviors.Find<ServiceDebugBehavior>();
      if (serviceDebugBehavior != null)
        flag = serviceDebugBehavior.IncludeExceptionDetailInFaults;
      if (string.IsNullOrEmpty(trustNamespace) || string.IsNullOrEmpty(action) || (flag || ex is FaultException))
        return false;
      FaultException faultException = this._securityTokenServiceConfiguration.ExceptionMapper.FromException(ex, requestEnvelopeVersion == EnvelopeVersion.Soap11 ? "http://schemas.xmlsoap.org/soap/envelope/" : "http://www.w3.org/2003/05/soap-envelope", trustNamespace);
      if (faultException != null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) faultException);
      return false;
    }

    public System.ServiceModel.Channels.Message ProcessTrust13Cancel(System.ServiceModel.Channels.Message message) => this.ProcessCore(message, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Cancel", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");

    public System.ServiceModel.Channels.Message ProcessTrust13Issue(System.ServiceModel.Channels.Message message) => this.ProcessCore(message, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");

    public System.ServiceModel.Channels.Message ProcessTrust13Renew(System.ServiceModel.Channels.Message message) => this.ProcessCore(message, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Renew", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");

    public System.ServiceModel.Channels.Message ProcessTrust13Validate(System.ServiceModel.Channels.Message message) => this.ProcessCore(message, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Validate", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");

    public System.ServiceModel.Channels.Message ProcessTrust13CancelResponse(System.ServiceModel.Channels.Message message) => this.ProcessCore(message, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Cancel", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");

    public System.ServiceModel.Channels.Message ProcessTrust13IssueResponse(System.ServiceModel.Channels.Message message) => this.ProcessCore(message, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Issue", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");

    public System.ServiceModel.Channels.Message ProcessTrust13RenewResponse(System.ServiceModel.Channels.Message message) => this.ProcessCore(message, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Renew", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");

    public System.ServiceModel.Channels.Message ProcessTrust13ValidateResponse(
      System.ServiceModel.Channels.Message message)
    {
      return this.ProcessCore(message, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Validate", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
    }

    public System.ServiceModel.Channels.Message ProcessTrustFeb2005Cancel(System.ServiceModel.Channels.Message message) => this.ProcessCore(message, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust");

    public System.ServiceModel.Channels.Message ProcessTrustFeb2005Issue(System.ServiceModel.Channels.Message message) => this.ProcessCore(message, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust");

    public System.ServiceModel.Channels.Message ProcessTrustFeb2005Renew(System.ServiceModel.Channels.Message message) => this.ProcessCore(message, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust");

    public System.ServiceModel.Channels.Message ProcessTrustFeb2005Validate(System.ServiceModel.Channels.Message message) => this.ProcessCore(message, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust");

    public System.ServiceModel.Channels.Message ProcessTrustFeb2005CancelResponse(
      System.ServiceModel.Channels.Message message)
    {
      return this.ProcessCore(message, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust");
    }

    public System.ServiceModel.Channels.Message ProcessTrustFeb2005IssueResponse(
      System.ServiceModel.Channels.Message message)
    {
      return this.ProcessCore(message, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust");
    }

    public System.ServiceModel.Channels.Message ProcessTrustFeb2005RenewResponse(
      System.ServiceModel.Channels.Message message)
    {
      return this.ProcessCore(message, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust");
    }

    public System.ServiceModel.Channels.Message ProcessTrustFeb2005ValidateResponse(
      System.ServiceModel.Channels.Message message)
    {
      return this.ProcessCore(message, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust");
    }

    public SecurityTokenServiceConfiguration SecurityTokenServiceConfiguration => this._securityTokenServiceConfiguration;

    public IAsyncResult BeginTrust13Cancel(
      System.ServiceModel.Channels.Message request,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessCore(request, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Cancel", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", callback, state);
    }

    public System.ServiceModel.Channels.Message EndTrust13Cancel(IAsyncResult ar) => this.EndProcessCore(ar, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Cancel", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");

    public IAsyncResult BeginTrust13Issue(
      System.ServiceModel.Channels.Message request,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessCore(request, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", callback, state);
    }

    public System.ServiceModel.Channels.Message EndTrust13Issue(IAsyncResult ar) => this.EndProcessCore(ar, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");

    public IAsyncResult BeginTrust13Renew(
      System.ServiceModel.Channels.Message request,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessCore(request, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Renew", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", callback, state);
    }

    public System.ServiceModel.Channels.Message EndTrust13Renew(IAsyncResult ar) => this.EndProcessCore(ar, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Renew", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");

    public IAsyncResult BeginTrust13Validate(
      System.ServiceModel.Channels.Message request,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessCore(request, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Validate", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", callback, state);
    }

    public System.ServiceModel.Channels.Message EndTrust13Validate(IAsyncResult ar) => this.EndProcessCore(ar, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Validate", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");

    public IAsyncResult BeginTrust13CancelResponse(
      System.ServiceModel.Channels.Message request,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessCore(request, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Cancel", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", callback, state);
    }

    public System.ServiceModel.Channels.Message EndTrust13CancelResponse(IAsyncResult ar) => this.EndProcessCore(ar, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Cancel", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");

    public IAsyncResult BeginTrust13IssueResponse(
      System.ServiceModel.Channels.Message request,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessCore(request, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Issue", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", callback, state);
    }

    public System.ServiceModel.Channels.Message EndTrust13IssueResponse(IAsyncResult ar) => this.EndProcessCore(ar, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Issue", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");

    public IAsyncResult BeginTrust13RenewResponse(
      System.ServiceModel.Channels.Message request,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessCore(request, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Renew", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", callback, state);
    }

    public System.ServiceModel.Channels.Message EndTrust13RenewResponse(IAsyncResult ar) => this.EndProcessCore(ar, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Renew", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");

    public IAsyncResult BeginTrust13ValidateResponse(
      System.ServiceModel.Channels.Message request,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessCore(request, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrust13RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Validate", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", callback, state);
    }

    public System.ServiceModel.Channels.Message EndTrust13ValidateResponse(IAsyncResult ar) => this.EndProcessCore(ar, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Validate", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");

    public IAsyncResult BeginTrustFeb2005Cancel(
      System.ServiceModel.Channels.Message request,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessCore(request, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust", callback, state);
    }

    public System.ServiceModel.Channels.Message EndTrustFeb2005Cancel(IAsyncResult ar) => this.EndProcessCore(ar, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust");

    public IAsyncResult BeginTrustFeb2005Issue(
      System.ServiceModel.Channels.Message request,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessCore(request, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust", callback, state);
    }

    public System.ServiceModel.Channels.Message EndTrustFeb2005Issue(IAsyncResult ar) => this.EndProcessCore(ar, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust");

    public IAsyncResult BeginTrustFeb2005Renew(
      System.ServiceModel.Channels.Message request,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessCore(request, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust", callback, state);
    }

    public System.ServiceModel.Channels.Message EndTrustFeb2005Renew(IAsyncResult ar) => this.EndProcessCore(ar, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust");

    public IAsyncResult BeginTrustFeb2005Validate(
      System.ServiceModel.Channels.Message request,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessCore(request, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust", callback, state);
    }

    public System.ServiceModel.Channels.Message EndTrustFeb2005Validate(IAsyncResult ar) => this.EndProcessCore(ar, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust");

    public IAsyncResult BeginTrustFeb2005CancelResponse(
      System.ServiceModel.Channels.Message request,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessCore(request, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust", callback, state);
    }

    public System.ServiceModel.Channels.Message EndTrustFeb2005CancelResponse(IAsyncResult ar) => this.EndProcessCore(ar, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust");

    public IAsyncResult BeginTrustFeb2005IssueResponse(
      System.ServiceModel.Channels.Message request,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessCore(request, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust", callback, state);
    }

    public System.ServiceModel.Channels.Message EndTrustFeb2005IssueResponse(IAsyncResult ar) => this.EndProcessCore(ar, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust");

    public IAsyncResult BeginTrustFeb2005RenewResponse(
      System.ServiceModel.Channels.Message request,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessCore(request, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust", callback, state);
    }

    public System.ServiceModel.Channels.Message EndTrustFeb2005RenewResponse(IAsyncResult ar) => this.EndProcessCore(ar, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust");

    public IAsyncResult BeginTrustFeb2005ValidateResponse(
      System.ServiceModel.Channels.Message request,
      AsyncCallback callback,
      object state)
    {
      return this.BeginProcessCore(request, (WSTrustRequestSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, (WSTrustResponseSerializer) this._securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust", callback, state);
    }

    public System.ServiceModel.Channels.Message EndTrustFeb2005ValidateResponse(
      IAsyncResult ar)
    {
      return this.EndProcessCore(ar, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust");
    }

    public void AddBindingParameters(
      ContractDescription contractDescription,
      ServiceEndpoint endpoint,
      BindingParameterCollection bindingParameters)
    {
    }

    public void ApplyClientBehavior(
      ContractDescription contractDescription,
      ServiceEndpoint endpoint,
      ClientRuntime clientRuntime)
    {
    }

    public void ApplyDispatchBehavior(
      ContractDescription contractDescription,
      ServiceEndpoint endpoint,
      DispatchRuntime dispatchRuntime)
    {
    }

    public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
    {
    }

    public virtual void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
    {
    }

    public virtual void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
    {
      if (exporter == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (exporter));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      if (context.WsdlPort == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3146"));
      if (context.WsdlPort.Service == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3147"));
      if (context.WsdlPort.Service.ServiceDescription == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3148"));
      System.Web.Services.Description.ServiceDescription serviceDescription = context.WsdlPort.Service.ServiceDescription;
      foreach (PortType portType in (CollectionBase) serviceDescription.PortTypes)
      {
        if (StringComparer.Ordinal.Equals(portType.Name, "IWSTrustFeb2005Sync"))
        {
          this.IncludeNamespace(context, "t", "http://schemas.xmlsoap.org/ws/2005/02/trust");
          this.ImportSchema(exporter, context, "http://schemas.xmlsoap.org/ws/2005/02/trust");
          this.FixMessageElement(serviceDescription, portType, context, "TrustFeb2005Cancel", new XmlQualifiedName("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust"), new XmlQualifiedName("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust"));
          this.FixMessageElement(serviceDescription, portType, context, "TrustFeb2005Issue", new XmlQualifiedName("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust"), new XmlQualifiedName("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust"));
          this.FixMessageElement(serviceDescription, portType, context, "TrustFeb2005Renew", new XmlQualifiedName("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust"), new XmlQualifiedName("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust"));
          this.FixMessageElement(serviceDescription, portType, context, "TrustFeb2005Validate", new XmlQualifiedName("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust"), new XmlQualifiedName("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust"));
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(portType.Name, "IWSTrust13Sync"))
        {
          this.IncludeNamespace(context, "trust", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
          this.ImportSchema(exporter, context, "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
          this.FixMessageElement(serviceDescription, portType, context, "Trust13Cancel", new XmlQualifiedName("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"), new XmlQualifiedName("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"));
          this.FixMessageElement(serviceDescription, portType, context, "Trust13Issue", new XmlQualifiedName("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"), new XmlQualifiedName("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"));
          this.FixMessageElement(serviceDescription, portType, context, "Trust13Renew", new XmlQualifiedName("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"), new XmlQualifiedName("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"));
          this.FixMessageElement(serviceDescription, portType, context, "Trust13Validate", new XmlQualifiedName("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"), new XmlQualifiedName("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"));
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(portType.Name, "IWSTrustFeb2005Async"))
        {
          this.IncludeNamespace(context, "t", "http://schemas.xmlsoap.org/ws/2005/02/trust");
          this.ImportSchema(exporter, context, "http://schemas.xmlsoap.org/ws/2005/02/trust");
          this.FixMessageElement(serviceDescription, portType, context, "TrustFeb2005CancelAsync", new XmlQualifiedName("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust"), new XmlQualifiedName("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust"));
          this.FixMessageElement(serviceDescription, portType, context, "TrustFeb2005IssueAsync", new XmlQualifiedName("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust"), new XmlQualifiedName("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust"));
          this.FixMessageElement(serviceDescription, portType, context, "TrustFeb2005RenewAsync", new XmlQualifiedName("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust"), new XmlQualifiedName("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust"));
          this.FixMessageElement(serviceDescription, portType, context, "TrustFeb2005ValidateAsync", new XmlQualifiedName("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust"), new XmlQualifiedName("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust"));
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals(portType.Name, "IWSTrust13Async"))
        {
          this.IncludeNamespace(context, "trust", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
          this.ImportSchema(exporter, context, "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
          this.FixMessageElement(serviceDescription, portType, context, "Trust13CancelAsync", new XmlQualifiedName("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"), new XmlQualifiedName("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"));
          this.FixMessageElement(serviceDescription, portType, context, "Trust13IssueAsync", new XmlQualifiedName("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"), new XmlQualifiedName("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"));
          this.FixMessageElement(serviceDescription, portType, context, "Trust13RenewAsync", new XmlQualifiedName("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"), new XmlQualifiedName("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"));
          this.FixMessageElement(serviceDescription, portType, context, "Trust13ValidateAsync", new XmlQualifiedName("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"), new XmlQualifiedName("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"));
        }
      }
    }

    protected virtual void IncludeNamespace(
      WsdlEndpointConversionContext context,
      string prefix,
      string ns)
    {
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      if (string.IsNullOrEmpty(prefix))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (prefix));
      if (string.IsNullOrEmpty(ns))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (ns));
      bool flag = false;
      foreach (XmlQualifiedName xmlQualifiedName in context.WsdlBinding.ServiceDescription.Namespaces.ToArray())
      {
        if (StringComparer.Ordinal.Equals(xmlQualifiedName.Namespace, ns))
        {
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      context.WsdlBinding.ServiceDescription.Namespaces.Add(prefix, ns);
    }

    protected virtual void ImportSchema(
      WsdlExporter exporter,
      WsdlEndpointConversionContext context,
      string ns)
    {
      if (exporter == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (exporter));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      if (string.IsNullOrEmpty(ns))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (ns));
      foreach (System.Xml.Schema.XmlSchema schema in (CollectionBase) context.WsdlPort.Service.ServiceDescription.Types.Schemas)
      {
        foreach (XmlSchemaObject include in schema.Includes)
        {
          if (include is XmlSchemaImport xmlSchemaImport && StringComparer.Ordinal.Equals(xmlSchemaImport.Namespace, ns))
            return;
        }
      }
      System.Xml.Schema.XmlSchema xmlSchema = WSTrustServiceContract.GetXmlSchema(exporter, ns);
      if (xmlSchema == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3004", (object) ns));
      System.Xml.Schema.XmlSchema schema1;
      if (context.WsdlPort.Service.ServiceDescription.Types.Schemas.Count == 0)
      {
        schema1 = new System.Xml.Schema.XmlSchema();
        context.WsdlPort.Service.ServiceDescription.Types.Schemas.Add(schema1);
      }
      else
        schema1 = context.WsdlPort.Service.ServiceDescription.Types.Schemas[0];
      XmlSchemaImport xmlSchemaImport1 = new XmlSchemaImport();
      xmlSchemaImport1.Namespace = ns;
      exporter.GeneratedXmlSchemas.Add(xmlSchema);
      schema1.Includes.Add((XmlSchemaObject) xmlSchemaImport1);
    }

    private static System.Xml.Schema.XmlSchema GetXmlSchema(
      WsdlExporter exporter,
      string ns)
    {
      if (exporter == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (exporter));
      if (string.IsNullOrEmpty(ns))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (ns));
      ICollection collection = exporter.GeneratedXmlSchemas.Schemas(ns);
      if (collection != null && collection.Count > 0)
      {
        IEnumerator enumerator = collection.GetEnumerator();
        try
        {
          if (enumerator.MoveNext())
            return (System.Xml.Schema.XmlSchema) enumerator.Current;
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      }
      string s;
      switch (ns)
      {
        case "http://schemas.xmlsoap.org/ws/2005/02/trust":
          s = "<?xml version='1.0' encoding='utf-8'?>\r\n<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'\r\n           xmlns:wst='http://schemas.xmlsoap.org/ws/2005/02/trust'\r\n           targetNamespace='http://schemas.xmlsoap.org/ws/2005/02/trust'\r\n           elementFormDefault='qualified' >\r\n\r\n<xs:element name='RequestSecurityToken' type='wst:RequestSecurityTokenType' />\r\n  <xs:complexType name='RequestSecurityTokenType' >\r\n    <xs:choice minOccurs='0' maxOccurs='unbounded' >\r\n        <xs:any namespace='##any' processContents='lax' minOccurs='0' maxOccurs='unbounded' />\r\n    </xs:choice>\r\n    <xs:attribute name='Context' type='xs:anyURI' use='optional' />\r\n    <xs:anyAttribute namespace='##other' processContents='lax' />\r\n  </xs:complexType>\r\n\r\n<xs:element name='RequestSecurityTokenResponse' type='wst:RequestSecurityTokenResponseType' />\r\n  <xs:complexType name='RequestSecurityTokenResponseType' >\r\n    <xs:choice minOccurs='0' maxOccurs='unbounded' >\r\n        <xs:any namespace='##any' processContents='lax' minOccurs='0' maxOccurs='unbounded' />\r\n    </xs:choice>\r\n    <xs:attribute name='Context' type='xs:anyURI' use='optional' />\r\n    <xs:anyAttribute namespace='##other' processContents='lax' />\r\n  </xs:complexType>\r\n\r\n        </xs:schema>";
          break;
        case "http://docs.oasis-open.org/ws-sx/ws-trust/200512":
          s = "<?xml version='1.0' encoding='utf-8'?>\r\n<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'\r\n           xmlns:trust='http://docs.oasis-open.org/ws-sx/ws-trust/200512'\r\n           targetNamespace='http://docs.oasis-open.org/ws-sx/ws-trust/200512'\r\n           elementFormDefault='qualified' >\r\n\r\n<xs:element name='RequestSecurityToken' type='trust:RequestSecurityTokenType' />\r\n  <xs:complexType name='RequestSecurityTokenType' >\r\n    <xs:choice minOccurs='0' maxOccurs='unbounded' >\r\n        <xs:any namespace='##any' processContents='lax' minOccurs='0' maxOccurs='unbounded' />\r\n    </xs:choice>\r\n    <xs:attribute name='Context' type='xs:anyURI' use='optional' />\r\n    <xs:anyAttribute namespace='##other' processContents='lax' />\r\n  </xs:complexType>\r\n\r\n<xs:element name='RequestSecurityTokenResponse' type='trust:RequestSecurityTokenResponseType' />\r\n  <xs:complexType name='RequestSecurityTokenResponseType' >\r\n    <xs:choice minOccurs='0' maxOccurs='unbounded' >\r\n        <xs:any namespace='##any' processContents='lax' minOccurs='0' maxOccurs='unbounded' />\r\n    </xs:choice>\r\n    <xs:attribute name='Context' type='xs:anyURI' use='optional' />\r\n    <xs:anyAttribute namespace='##other' processContents='lax' />\r\n  </xs:complexType>\r\n\r\n  <xs:element name='RequestSecurityTokenResponseCollection' type='trust:RequestSecurityTokenResponseCollectionType' />\r\n  <xs:complexType name='RequestSecurityTokenResponseCollectionType' >\r\n    <xs:sequence>\r\n      <xs:element ref='trust:RequestSecurityTokenResponse' minOccurs='1' maxOccurs='unbounded' />\r\n    </xs:sequence>\r\n    <xs:anyAttribute namespace='##other' processContents='lax' />\r\n  </xs:complexType>\r\n\r\n        </xs:schema>";
          break;
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID5004", (object) ns));
      }
      return System.Xml.Schema.XmlSchema.Read((TextReader) new StringReader(s), (ValidationEventHandler) null);
    }

    protected virtual void FixMessageElement(
      System.Web.Services.Description.ServiceDescription serviceDescription,
      PortType portType,
      WsdlEndpointConversionContext context,
      string operationName,
      XmlQualifiedName inputMessageElement,
      XmlQualifiedName outputMessageElement)
    {
      if (serviceDescription == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (serviceDescription));
      if (portType == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (portType));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      if (string.IsNullOrEmpty(operationName))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString(nameof (operationName));
      if (inputMessageElement == (XmlQualifiedName) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (inputMessageElement));
      if (outputMessageElement == (XmlQualifiedName) null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (outputMessageElement));
      System.Web.Services.Description.Operation operation1 = (System.Web.Services.Description.Operation) null;
      System.Web.Services.Description.Message message1 = (System.Web.Services.Description.Message) null;
      System.Web.Services.Description.Message message2 = (System.Web.Services.Description.Message) null;
      foreach (System.Web.Services.Description.Operation operation2 in (CollectionBase) portType.Operations)
      {
        if (StringComparer.Ordinal.Equals(operation2.Name, operationName))
        {
          operation1 = operation2;
          foreach (System.Web.Services.Description.Message message3 in (CollectionBase) serviceDescription.Messages)
          {
            if (StringComparer.Ordinal.Equals(message3.Name, operation2.Messages.Input.Message.Name))
            {
              if (message3.Parts.Count != 1)
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3144", (object) portType.Name, (object) operation2.Name, (object) message3.Name, (object) message3.Parts.Count));
              message1 = message3;
            }
            else if (StringComparer.Ordinal.Equals(message3.Name, operation2.Messages.Output.Message.Name))
            {
              if (message3.Parts.Count != 1)
                throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3144", (object) portType.Name, (object) operation2.Name, (object) message3.Name, (object) message3.Parts.Count));
              message2 = message3;
            }
            if (message1 != null)
            {
              if (message2 != null)
                break;
            }
          }
        }
        if (operation1 != null)
          break;
      }
      if (operation1 == null)
        return;
      if (message1 == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3149", (object) portType.Name, (object) portType.Namespaces, (object) operationName));
      if (message2 == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3150", (object) portType.Name, (object) portType.Namespaces, (object) operationName));
      message1.Parts[0].Element = inputMessageElement;
      message2.Parts[0].Element = outputMessageElement;
      message1.Parts[0].Type = (XmlQualifiedName) null;
      message2.Parts[0].Type = (XmlQualifiedName) null;
    }

    internal class ProcessCoreAsyncResult : AsyncResult
    {
      private WSTrustServiceContract _trustServiceContract;
      private DispatchContext _dispatchContext;
      private MessageVersion _messageVersion;
      private WSTrustResponseSerializer _responseSerializer;
      private WSTrustSerializationContext _serializationContext;

      public ProcessCoreAsyncResult(
        WSTrustServiceContract contract,
        DispatchContext dispatchContext,
        MessageVersion messageVersion,
        WSTrustResponseSerializer responseSerializer,
        WSTrustSerializationContext serializationContext,
        AsyncCallback asyncCallback,
        object asyncState)
        : base(asyncCallback, asyncState)
      {
        if (contract == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (contract));
        if (dispatchContext == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (dispatchContext));
        if (responseSerializer == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (responseSerializer));
        if (serializationContext == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (serializationContext));
        this._trustServiceContract = contract;
        this._dispatchContext = dispatchContext;
        this._messageVersion = messageVersion;
        this._responseSerializer = responseSerializer;
        this._serializationContext = serializationContext;
        contract.BeginDispatchRequest(dispatchContext, new AsyncCallback(this.OnDispatchRequestCompleted), (object) null);
      }

      public WSTrustServiceContract TrustServiceContract => this._trustServiceContract;

      public DispatchContext DispatchContext => this._dispatchContext;

      public MessageVersion MessageVersion => this._messageVersion;

      public WSTrustResponseSerializer ResponseSerializer => this._responseSerializer;

      public WSTrustSerializationContext SerializationContext => this._serializationContext;

      public static System.ServiceModel.Channels.Message End(IAsyncResult ar)
      {
        AsyncResult.End(ar);
        if (!(ar is WSTrustServiceContract.ProcessCoreAsyncResult processCoreAsyncResult))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID2004", (object) typeof (WSTrustServiceContract.ProcessCoreAsyncResult), (object) ar.GetType()));
        return System.ServiceModel.Channels.Message.CreateMessage(OperationContext.Current.RequestContext.RequestMessage.Version, processCoreAsyncResult.DispatchContext.ResponseAction, (BodyWriter) new WSTrustResponseBodyWriter(processCoreAsyncResult.DispatchContext.ResponseMessage, processCoreAsyncResult.ResponseSerializer, processCoreAsyncResult.SerializationContext));
      }

      private void OnDispatchRequestCompleted(IAsyncResult ar)
      {
        try
        {
          this._dispatchContext = this._trustServiceContract.EndDispatchRequest(ar);
          this.Complete(false);
        }
        catch (Exception ex)
        {
          if (DiagnosticUtil.ExceptionUtil.IsFatal(ex))
            throw;
          else
            this.Complete(false, ex);
        }
      }
    }

    internal class DispatchRequestAsyncResult : AsyncResult
    {
      private DispatchContext _dispatchContext;

      public DispatchContext DispatchContext => this._dispatchContext;

      public DispatchRequestAsyncResult(
        DispatchContext dispatchContext,
        AsyncCallback asyncCallback,
        object asyncState)
        : base(asyncCallback, asyncState)
      {
        this._dispatchContext = dispatchContext;
        IClaimsPrincipal principal = dispatchContext.Principal;
        RequestSecurityToken requestMessage = dispatchContext.RequestMessage as RequestSecurityToken;
        Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService securityTokenService = dispatchContext.SecurityTokenService;
        if (requestMessage == null)
        {
          this.Complete(true, DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidRequestException(Microsoft.IdentityModel.SR.GetString("ID3023"))));
        }
        else
        {
          switch (requestMessage.RequestType)
          {
            case "http://schemas.microsoft.com/idfx/requesttype/cancel":
              securityTokenService.BeginCancel(principal, requestMessage, new AsyncCallback(this.OnCancelComplete), (object) null);
              break;
            case "http://schemas.microsoft.com/idfx/requesttype/issue":
              securityTokenService.BeginIssue(principal, requestMessage, new AsyncCallback(this.OnIssueComplete), (object) null);
              break;
            case "http://schemas.microsoft.com/idfx/requesttype/renew":
              securityTokenService.BeginRenew(principal, requestMessage, new AsyncCallback(this.OnRenewComplete), (object) null);
              break;
            case "http://schemas.microsoft.com/idfx/requesttype/validate":
              securityTokenService.BeginValidate(principal, requestMessage, new AsyncCallback(this.OnValidateComplete), (object) null);
              break;
            default:
              this.Complete(true, DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID3112", (object) requestMessage.RequestType))));
              break;
          }
        }
      }

      public static DispatchContext End(IAsyncResult ar)
      {
        AsyncResult.End(ar);
        return ar is WSTrustServiceContract.DispatchRequestAsyncResult requestAsyncResult ? requestAsyncResult.DispatchContext : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID2004", (object) typeof (WSTrustServiceContract.DispatchRequestAsyncResult), (object) ar.GetType()));
      }

      private void OnCancelComplete(IAsyncResult ar)
      {
        try
        {
          this._dispatchContext.ResponseMessage = this._dispatchContext.SecurityTokenService.EndCancel(ar);
          this.Complete(false);
        }
        catch (Exception ex)
        {
          this.Complete(false, ex);
        }
      }

      private void OnIssueComplete(IAsyncResult ar)
      {
        try
        {
          this._dispatchContext.ResponseMessage = this._dispatchContext.SecurityTokenService.EndIssue(ar);
          this.Complete(false);
        }
        catch (Exception ex)
        {
          this.Complete(false, ex);
        }
      }

      private void OnRenewComplete(IAsyncResult ar)
      {
        try
        {
          this._dispatchContext.ResponseMessage = this._dispatchContext.SecurityTokenService.EndRenew(ar);
          this.Complete(false);
        }
        catch (Exception ex)
        {
          this.Complete(false, ex);
        }
      }

      private void OnValidateComplete(IAsyncResult ar)
      {
        try
        {
          this._dispatchContext.ResponseMessage = this._dispatchContext.SecurityTokenService.EndValidate(ar);
          this.Complete(false);
        }
        catch (Exception ex)
        {
          this.Complete(false, ex);
        }
      }
    }
  }
}
