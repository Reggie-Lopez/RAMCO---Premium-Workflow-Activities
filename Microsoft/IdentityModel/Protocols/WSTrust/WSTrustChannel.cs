// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrustChannel
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using Microsoft.IdentityModel.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(true)]
  public class WSTrustChannel : 
    IWSTrustChannelContract,
    IWSTrustContract,
    IChannel,
    ICommunicationObject
  {
    private const int DefaultKeySizeInBits = 1024;
    private const int FaultMaxBufferSize = 20480;
    private WSTrustChannelFactory _factory;
    private IChannel _innerChannel;
    private IWSTrustChannelContract _innerContract;
    private MessageVersion _messageVersion;
    private TrustVersion _trustVersion;
    private WSTrustSerializationContext _context;
    private WSTrustRequestSerializer _wsTrustRequestSerializer;
    private WSTrustResponseSerializer _wsTrustResponseSerializer;

    public IChannel Channel
    {
      get => this._innerChannel;
      protected set => this._innerChannel = value;
    }

    public WSTrustChannelFactory ChannelFactory
    {
      get => this._factory;
      protected set => this._factory = value;
    }

    public IWSTrustChannelContract Contract
    {
      get => this._innerContract;
      protected set => this._innerContract = value;
    }

    public TrustVersion TrustVersion
    {
      get => this._trustVersion;
      protected set
      {
        if (value != null && value != TrustVersion.WSTrust13)
        {
          TrustVersion wsTrustFeb2005 = TrustVersion.WSTrustFeb2005;
        }
        this._trustVersion = value;
      }
    }

    public WSTrustSerializationContext WSTrustSerializationContext
    {
      get => this._context;
      protected set => this._context = value;
    }

    public WSTrustRequestSerializer WSTrustRequestSerializer
    {
      get => this._wsTrustRequestSerializer;
      protected set => this._wsTrustRequestSerializer = value;
    }

    public WSTrustResponseSerializer WSTrustResponseSerializer
    {
      get => this._wsTrustResponseSerializer;
      protected set => this._wsTrustResponseSerializer = value;
    }

    public WSTrustChannel(
      WSTrustChannelFactory factory,
      IWSTrustChannelContract inner,
      TrustVersion trustVersion,
      WSTrustSerializationContext context,
      WSTrustRequestSerializer requestSerializer,
      WSTrustResponseSerializer responseSerializer)
    {
      if (factory == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (inner));
      if (inner == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (inner));
      if (context == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (context));
      if (requestSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (requestSerializer));
      if (responseSerializer == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (responseSerializer));
      if (trustVersion == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustVersion));
      this._innerChannel = inner as IChannel;
      if (this._innerChannel == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3286"));
      this._innerContract = inner;
      this._factory = factory;
      this._context = context;
      this._wsTrustRequestSerializer = requestSerializer;
      this._wsTrustResponseSerializer = responseSerializer;
      this._trustVersion = trustVersion;
      this._messageVersion = MessageVersion.Default;
      if (this._factory.Endpoint == null || this._factory.Endpoint.Binding == null || this._factory.Endpoint.Binding.MessageVersion == null)
        return;
      this._messageVersion = this._factory.Endpoint.Binding.MessageVersion;
    }

    protected virtual Message CreateRequest(RequestSecurityToken request, string requestType) => Message.CreateMessage(this._messageVersion, WSTrustChannel.GetRequestAction(requestType, this.TrustVersion), (BodyWriter) new WSTrustRequestBodyWriter(request, this.WSTrustRequestSerializer, this.WSTrustSerializationContext));

    protected virtual RequestSecurityTokenResponse ReadResponse(
      Message response)
    {
      if (response == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (response));
      if (response.IsFault)
      {
        MessageFault fault = MessageFault.CreateFault(response, 20480);
        string action = (string) null;
        if (response.Headers != null)
          action = response.Headers.Action;
        throw FaultException.CreateFault(fault, action);
      }
      return this.WSTrustResponseSerializer.ReadXml((XmlReader) response.GetReaderAtBodyContents(), this.WSTrustSerializationContext);
    }

    protected static string GetRequestAction(string requestType, TrustVersion trustVersion)
    {
      if (trustVersion != TrustVersion.WSTrust13 && trustVersion != TrustVersion.WSTrustFeb2005)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID3137", (object) trustVersion.ToString())));
      switch (requestType)
      {
        case "http://schemas.microsoft.com/idfx/requesttype/cancel":
          return trustVersion != TrustVersion.WSTrustFeb2005 ? "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Cancel" : "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Cancel";
        case "http://schemas.microsoft.com/idfx/requesttype/issue":
          return trustVersion != TrustVersion.WSTrustFeb2005 ? "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue" : "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue";
        case "http://schemas.microsoft.com/idfx/requesttype/renew":
          return trustVersion != TrustVersion.WSTrustFeb2005 ? "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Renew" : "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Renew";
        case "http://schemas.microsoft.com/idfx/requesttype/validate":
          return trustVersion != TrustVersion.WSTrustFeb2005 ? "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Validate" : "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Validate";
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID3141", (object) requestType.ToString())));
      }
    }

    public virtual SecurityToken GetTokenFromResponse(
      RequestSecurityToken request,
      RequestSecurityTokenResponse response)
    {
      if (response == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (response));
      if (!response.IsFinal)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotImplementedException(Microsoft.IdentityModel.SR.GetString("ID3270")));
      if (response.RequestedSecurityToken == null)
        return (SecurityToken) null;
      SecurityToken securityToken = response.RequestedSecurityToken.SecurityToken;
      if (securityToken != null)
        return securityToken;
      if (response.RequestedSecurityToken.SecurityTokenXml == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID3138")));
      SecurityToken proofKey = WSTrustChannel.GetProofKey(request, response);
      DateTime? nullable1 = new DateTime?();
      DateTime? nullable2 = new DateTime?();
      if (response.Lifetime != null)
      {
        nullable1 = response.Lifetime.Created;
        nullable2 = response.Lifetime.Expires;
        if (!nullable1.HasValue)
          nullable1 = new DateTime?(DateTime.UtcNow);
        if (!nullable2.HasValue)
          nullable2 = new DateTime?(DateTime.UtcNow.AddHours(10.0));
      }
      else
      {
        nullable1 = new DateTime?(DateTime.UtcNow);
        nullable2 = new DateTime?(DateTime.UtcNow.AddHours(10.0));
      }
      return (SecurityToken) new GenericXmlSecurityToken(response.RequestedSecurityToken.SecurityTokenXml, proofKey, nullable1.Value, nullable2.Value, response.RequestedAttachedReference, response.RequestedUnattachedReference, new ReadOnlyCollection<IAuthorizationPolicy>((IList<IAuthorizationPolicy>) new List<IAuthorizationPolicy>()));
    }

    internal static SecurityToken GetUseKeySecurityToken(
      UseKey useKey,
      string requestKeyType)
    {
      return useKey != null && useKey.Token != null ? useKey.Token : throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID3190", (object) requestKeyType)));
    }

    internal static WSTrustChannel.ProofKeyType GetKeyType(string keyType)
    {
      if (keyType == "http://docs.oasis-open.org/ws-sx/ws-trust/200512/SymmetricKey" || keyType == "http://schemas.xmlsoap.org/ws/2005/02/trust/SymmetricKey" || (keyType == "http://schemas.microsoft.com/idfx/keytype/symmetric" || string.IsNullOrEmpty(keyType)))
        return WSTrustChannel.ProofKeyType.Symmetric;
      if (keyType == "http://docs.oasis-open.org/ws-sx/ws-trust/200512/PublicKey" || keyType == "http://schemas.xmlsoap.org/ws/2005/02/trust/PublicKey" || keyType == "http://schemas.microsoft.com/idfx/keytype/asymmetric")
        return WSTrustChannel.ProofKeyType.Asymmetric;
      return keyType == "http://docs.oasis-open.org/ws-sx/ws-trust/200512/Bearer" || keyType == "http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey" || keyType == "http://schemas.microsoft.com/idfx/keytype/bearer" ? WSTrustChannel.ProofKeyType.Bearer : WSTrustChannel.ProofKeyType.Unknown;
    }

    internal static bool IsPsha1(string algorithm) => algorithm == "http://docs.oasis-open.org/ws-sx/ws-trust/200512/CK/PSHA1" || algorithm == "http://schemas.xmlsoap.org/ws/2005/02/trust/CK/PSHA1" || algorithm == "http://schemas.microsoft.com/idfx/computedkeyalgorithm/psha1";

    internal static SecurityToken ComputeProofKey(
      RequestSecurityToken request,
      RequestSecurityTokenResponse response)
    {
      if (response.Entropy == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID3193")));
      if (request.Entropy == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID3194")));
      int keySizeInBits = request.KeySizeInBits ?? 1024;
      if (response.KeySizeInBits.HasValue)
        keySizeInBits = response.KeySizeInBits.Value;
      return (SecurityToken) new BinarySecretSecurityToken(KeyGenerator.ComputeCombinedKey(request.Entropy.GetKeyBytes(), response.Entropy.GetKeyBytes(), keySizeInBits));
    }

    internal static SecurityToken GetProofKey(
      RequestSecurityToken request,
      RequestSecurityTokenResponse response)
    {
      if (response.RequestedProofToken != null)
      {
        if (response.RequestedProofToken.ProtectedKey != null)
          return (SecurityToken) new BinarySecretSecurityToken(response.RequestedProofToken.ProtectedKey.GetKeyBytes());
        if (WSTrustChannel.IsPsha1(response.RequestedProofToken.ComputedKeyAlgorithm))
          return WSTrustChannel.ComputeProofKey(request, response);
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID3192", (object) response.RequestedProofToken.ComputedKeyAlgorithm)));
      }
      switch (WSTrustChannel.GetKeyType(request.KeyType))
      {
        case WSTrustChannel.ProofKeyType.Bearer:
          return (SecurityToken) null;
        case WSTrustChannel.ProofKeyType.Symmetric:
          if (response.Entropy != null)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID3191")));
          return request.Entropy != null ? (SecurityToken) new BinarySecretSecurityToken(request.Entropy.GetKeyBytes()) : (SecurityToken) null;
        case WSTrustChannel.ProofKeyType.Asymmetric:
          return WSTrustChannel.GetUseKeySecurityToken(request.UseKey, request.KeyType);
        default:
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID3139", (object) request.KeyType)));
      }
    }

    public T GetProperty<T>() where T : class => this.Channel.GetProperty<T>();

    public void Abort() => this.Channel.Abort();

    public IAsyncResult BeginClose(
      TimeSpan timeout,
      AsyncCallback callback,
      object state)
    {
      return this.Channel.BeginClose(timeout, callback, state);
    }

    public IAsyncResult BeginClose(AsyncCallback callback, object state) => this.Channel.BeginClose(callback, state);

    public IAsyncResult BeginOpen(
      TimeSpan timeout,
      AsyncCallback callback,
      object state)
    {
      return this.Channel.BeginOpen(timeout, callback, state);
    }

    public IAsyncResult BeginOpen(AsyncCallback callback, object state) => this.Channel.BeginOpen(callback, state);

    public void Close(TimeSpan timeout) => this.Channel.Close(timeout);

    public void Close() => this.Channel.Close();

    public event EventHandler Closed
    {
      add => this.Channel.Closed += value;
      remove => this.Channel.Closed -= value;
    }

    public event EventHandler Closing
    {
      add => this.Channel.Closing += value;
      remove => this.Channel.Closing -= value;
    }

    public void EndClose(IAsyncResult result) => this.Channel.EndClose(result);

    public void EndOpen(IAsyncResult result) => this.Channel.EndOpen(result);

    public event EventHandler Faulted
    {
      add => this.Channel.Faulted += value;
      remove => this.Channel.Faulted -= value;
    }

    public void Open(TimeSpan timeout) => this.Channel.Open(timeout);

    public void Open() => this.Channel.Open();

    public event EventHandler Opened
    {
      add => this.Channel.Opened += value;
      remove => this.Channel.Opened -= value;
    }

    public event EventHandler Opening
    {
      add => this.Channel.Opening += value;
      remove => this.Channel.Opening -= value;
    }

    public CommunicationState State => this.Channel.State;

    public virtual RequestSecurityTokenResponse Cancel(
      RequestSecurityToken rst)
    {
      return this.ReadResponse(this.Contract.Cancel(this.CreateRequest(rst, "http://schemas.microsoft.com/idfx/requesttype/cancel")));
    }

    public virtual SecurityToken Issue(RequestSecurityToken rst)
    {
      RequestSecurityTokenResponse rstr = (RequestSecurityTokenResponse) null;
      return this.Issue(rst, out rstr);
    }

    public virtual SecurityToken Issue(
      RequestSecurityToken rst,
      out RequestSecurityTokenResponse rstr)
    {
      Message response = this.Contract.Issue(this.CreateRequest(rst, "http://schemas.microsoft.com/idfx/requesttype/issue"));
      rstr = this.ReadResponse(response);
      return this.GetTokenFromResponse(rst, rstr);
    }

    public virtual RequestSecurityTokenResponse Renew(
      RequestSecurityToken rst)
    {
      return this.ReadResponse(this.Contract.Renew(this.CreateRequest(rst, "http://schemas.microsoft.com/idfx/requesttype/renew")));
    }

    public virtual RequestSecurityTokenResponse Validate(
      RequestSecurityToken rst)
    {
      return this.ReadResponse(this.Contract.Validate(this.CreateRequest(rst, "http://schemas.microsoft.com/idfx/requesttype/validate")));
    }

    private IAsyncResult BeginOperation(
      WSTrustChannel.WSTrustChannelAsyncResult.Operations operation,
      string requestType,
      RequestSecurityToken rst,
      AsyncCallback callback,
      object state)
    {
      Message request = rst != null ? this.CreateRequest(rst, requestType) : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (rst));
      WSTrustSerializationContext serializationContext = this.WSTrustSerializationContext;
      return (IAsyncResult) new WSTrustChannel.WSTrustChannelAsyncResult((IWSTrustContract) this, operation, rst, serializationContext, request, callback, state);
    }

    private RequestSecurityTokenResponse EndOperation(
      IAsyncResult result,
      out WSTrustChannel.WSTrustChannelAsyncResult tcar)
    {
      tcar = result != null ? result as WSTrustChannel.WSTrustChannelAsyncResult : throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (result));
      if (tcar == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID2004", (object) typeof (WSTrustChannel.WSTrustChannelAsyncResult), (object) result.GetType()));
      return this.ReadResponse(WSTrustChannel.WSTrustChannelAsyncResult.End(result));
    }

    public IAsyncResult BeginCancel(
      RequestSecurityToken rst,
      AsyncCallback callback,
      object state)
    {
      return this.BeginOperation(WSTrustChannel.WSTrustChannelAsyncResult.Operations.Cancel, "http://schemas.microsoft.com/idfx/requesttype/cancel", rst, callback, state);
    }

    public void EndCancel(IAsyncResult result, out RequestSecurityTokenResponse rstr) => rstr = this.EndOperation(result, out WSTrustChannel.WSTrustChannelAsyncResult _);

    public IAsyncResult BeginIssue(
      RequestSecurityToken rst,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginOperation(WSTrustChannel.WSTrustChannelAsyncResult.Operations.Issue, "http://schemas.microsoft.com/idfx/requesttype/issue", rst, callback, asyncState);
    }

    public SecurityToken EndIssue(
      IAsyncResult result,
      out RequestSecurityTokenResponse rstr)
    {
      WSTrustChannel.WSTrustChannelAsyncResult tcar;
      rstr = this.EndOperation(result, out tcar);
      return this.GetTokenFromResponse(tcar.RequestSecurityToken, rstr);
    }

    public IAsyncResult BeginRenew(
      RequestSecurityToken rst,
      AsyncCallback callback,
      object state)
    {
      return this.BeginOperation(WSTrustChannel.WSTrustChannelAsyncResult.Operations.Renew, "http://schemas.microsoft.com/idfx/requesttype/renew", rst, callback, state);
    }

    public void EndRenew(IAsyncResult result, out RequestSecurityTokenResponse rstr) => rstr = this.EndOperation(result, out WSTrustChannel.WSTrustChannelAsyncResult _);

    public IAsyncResult BeginValidate(
      RequestSecurityToken rst,
      AsyncCallback callback,
      object state)
    {
      return this.BeginOperation(WSTrustChannel.WSTrustChannelAsyncResult.Operations.Validate, "http://schemas.microsoft.com/idfx/requesttype/validate", rst, callback, state);
    }

    public void EndValidate(IAsyncResult result, out RequestSecurityTokenResponse rstr) => rstr = this.EndOperation(result, out WSTrustChannel.WSTrustChannelAsyncResult _);

    public Message Cancel(Message message) => this.Contract.Cancel(message);

    public IAsyncResult BeginCancel(
      Message message,
      AsyncCallback callback,
      object asyncState)
    {
      return this.Contract.BeginCancel(message, callback, asyncState);
    }

    public Message EndCancel(IAsyncResult asyncResult) => this.Contract.EndCancel(asyncResult);

    public Message Issue(Message message) => this.Contract.Issue(message);

    public IAsyncResult BeginIssue(
      Message message,
      AsyncCallback callback,
      object asyncState)
    {
      return this.Contract.BeginIssue(message, callback, asyncState);
    }

    public Message EndIssue(IAsyncResult asyncResult) => this.Contract.EndIssue(asyncResult);

    public Message Renew(Message message) => this.Contract.Renew(message);

    public IAsyncResult BeginRenew(
      Message message,
      AsyncCallback callback,
      object asyncState)
    {
      return this.Contract.BeginRenew(message, callback, asyncState);
    }

    public Message EndRenew(IAsyncResult asyncResult) => this.Contract.EndRenew(asyncResult);

    public Message Validate(Message message) => this.Contract.Validate(message);

    public IAsyncResult BeginValidate(
      Message message,
      AsyncCallback callback,
      object asyncState)
    {
      return this.Contract.BeginValidate(message, callback, asyncState);
    }

    public Message EndValidate(IAsyncResult asyncResult) => this.Contract.EndValidate(asyncResult);

    internal class WSTrustChannelAsyncResult : AsyncResult
    {
      private IWSTrustContract _client;
      private RequestSecurityToken _rst;
      private WSTrustSerializationContext _serializationContext;
      private Message _response;
      private WSTrustChannel.WSTrustChannelAsyncResult.Operations _operation;

      public WSTrustChannelAsyncResult(
        IWSTrustContract client,
        WSTrustChannel.WSTrustChannelAsyncResult.Operations operation,
        RequestSecurityToken rst,
        WSTrustSerializationContext serializationContext,
        Message request,
        AsyncCallback callback,
        object state)
        : base(callback, state)
      {
        this._client = client;
        this._rst = rst;
        this._serializationContext = serializationContext;
        this._operation = operation;
        switch (this._operation)
        {
          case WSTrustChannel.WSTrustChannelAsyncResult.Operations.Cancel:
            client.BeginCancel(request, new AsyncCallback(this.OnOperationCompleted), (object) null);
            break;
          case WSTrustChannel.WSTrustChannelAsyncResult.Operations.Issue:
            client.BeginIssue(request, new AsyncCallback(this.OnOperationCompleted), (object) null);
            break;
          case WSTrustChannel.WSTrustChannelAsyncResult.Operations.Renew:
            client.BeginRenew(request, new AsyncCallback(this.OnOperationCompleted), (object) null);
            break;
          case WSTrustChannel.WSTrustChannelAsyncResult.Operations.Validate:
            client.BeginValidate(request, new AsyncCallback(this.OnOperationCompleted), (object) null);
            break;
          default:
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3285", (object) Enum.GetName(typeof (WSTrustChannel.WSTrustChannelAsyncResult.Operations), (object) this._operation)));
        }
      }

      public IWSTrustContract Client
      {
        get => this._client;
        set => this._client = value;
      }

      public RequestSecurityToken RequestSecurityToken
      {
        get => this._rst;
        set => this._rst = value;
      }

      public Message Response
      {
        get => this._response;
        set => this._response = value;
      }

      public WSTrustSerializationContext SerializationContext
      {
        get => this._serializationContext;
        set => this._serializationContext = value;
      }

      public static Message End(IAsyncResult iar)
      {
        AsyncResult.End(iar);
        return iar is WSTrustChannel.WSTrustChannelAsyncResult channelAsyncResult ? channelAsyncResult.Response : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID2004", (object) typeof (WSTrustChannel.WSTrustChannelAsyncResult), (object) iar.GetType()));
      }

      private void OnOperationCompleted(IAsyncResult iar)
      {
        try
        {
          this.Response = this.EndOperation(iar);
          this.Complete(iar.CompletedSynchronously);
        }
        catch (Exception ex)
        {
          if (DiagnosticUtil.ExceptionUtil.IsFatal(ex))
            throw;
          else
            this.Complete(false, ex);
        }
      }

      private Message EndOperation(IAsyncResult iar)
      {
        switch (this._operation)
        {
          case WSTrustChannel.WSTrustChannelAsyncResult.Operations.Cancel:
            return this.Client.EndCancel(iar);
          case WSTrustChannel.WSTrustChannelAsyncResult.Operations.Issue:
            return this.Client.EndIssue(iar);
          case WSTrustChannel.WSTrustChannelAsyncResult.Operations.Renew:
            return this.Client.EndRenew(iar);
          case WSTrustChannel.WSTrustChannelAsyncResult.Operations.Validate:
            return this.Client.EndValidate(iar);
          default:
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3285", (object) this._operation));
        }
      }

      public enum Operations
      {
        Cancel,
        Issue,
        Renew,
        Validate,
      }
    }

    internal enum ProofKeyType
    {
      Unknown,
      Bearer,
      Symmetric,
      Asymmetric,
    }
  }
}
