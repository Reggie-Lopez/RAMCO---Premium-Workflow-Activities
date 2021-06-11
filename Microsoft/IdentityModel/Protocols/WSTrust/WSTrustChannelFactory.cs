// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.WSTrustChannelFactory
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ComVisible(false)]
  public class WSTrustChannelFactory : ChannelFactory<IWSTrustChannelContract>
  {
    private object _factoryLock = new object();
    private bool _locked;
    private WSTrustChannelFactory.WSTrustChannelLockedProperties _lockedProperties;
    private TrustVersion _trustVersion;
    private SecurityTokenResolver _securityTokenResolver;
    private SecurityTokenResolver _useKeyTokenResolver;
    private Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager _securityTokenHandlerCollectionManager = Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager.CreateDefaultSecurityTokenHandlerCollectionManager();
    private WSTrustRequestSerializer _wsTrustRequestSerializer;
    private WSTrustResponseSerializer _wsTrustResponseSerializer;

    public WSTrustChannelFactory()
    {
    }

    public WSTrustChannelFactory(string endpointConfigurationName)
      : base(endpointConfigurationName)
    {
    }

    public WSTrustChannelFactory(Binding binding)
      : base(binding)
    {
    }

    public WSTrustChannelFactory(ServiceEndpoint endpoint)
      : base(endpoint)
    {
    }

    public WSTrustChannelFactory(string endpointConfigurationName, EndpointAddress remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public WSTrustChannelFactory(Binding binding, EndpointAddress remoteAddress)
      : base(binding, remoteAddress)
    {
    }

    public WSTrustChannelFactory(Binding binding, string remoteAddress)
      : base(binding, remoteAddress)
    {
    }

    public TrustVersion TrustVersion
    {
      get => this._trustVersion;
      set
      {
        lock (this._factoryLock)
        {
          if (this._locked)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3287"));
          this._trustVersion = value;
        }
      }
    }

    public Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager SecurityTokenHandlerCollectionManager
    {
      get => this._securityTokenHandlerCollectionManager;
      set
      {
        if (value == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
        lock (this._factoryLock)
        {
          if (this._locked)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3287"));
          this._securityTokenHandlerCollectionManager = value;
        }
      }
    }

    public SecurityTokenResolver SecurityTokenResolver
    {
      get => this._securityTokenResolver;
      set
      {
        lock (this._factoryLock)
        {
          if (this._locked)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3287"));
          this._securityTokenResolver = value;
        }
      }
    }

    public SecurityTokenResolver UseKeyTokenResolver
    {
      get => this._useKeyTokenResolver;
      set
      {
        lock (this._factoryLock)
        {
          if (this._locked)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3287"));
          this._useKeyTokenResolver = value;
        }
      }
    }

    public WSTrustRequestSerializer WSTrustRequestSerializer
    {
      get => this._wsTrustRequestSerializer;
      set
      {
        lock (this._factoryLock)
        {
          if (this._locked)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3287"));
          this._wsTrustRequestSerializer = value;
        }
      }
    }

    public WSTrustResponseSerializer WSTrustResponseSerializer
    {
      get => this._wsTrustResponseSerializer;
      set
      {
        lock (this._factoryLock)
        {
          if (this._locked)
            throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3287"));
          this._wsTrustResponseSerializer = value;
        }
      }
    }

    public override IWSTrustChannelContract CreateChannel(
      EndpointAddress address,
      Uri via)
    {
      IWSTrustChannelContract channel = base.CreateChannel(address, via);
      WSTrustChannelFactory.WSTrustChannelLockedProperties lockedProperties = this.GetLockedProperties();
      return (IWSTrustChannelContract) this.CreateTrustChannel(channel, lockedProperties.TrustVersion, lockedProperties.Context, lockedProperties.RequestSerializer, lockedProperties.ResponseSerializer);
    }

    protected virtual WSTrustChannel CreateTrustChannel(
      IWSTrustChannelContract innerChannel,
      TrustVersion trustVersion,
      WSTrustSerializationContext context,
      WSTrustRequestSerializer requestSerializer,
      WSTrustResponseSerializer responseSerializer)
    {
      return new WSTrustChannel(this, innerChannel, trustVersion, context, requestSerializer, responseSerializer);
    }

    private WSTrustChannelFactory.WSTrustChannelLockedProperties GetLockedProperties()
    {
      lock (this._factoryLock)
      {
        if (this._lockedProperties == null)
        {
          WSTrustChannelFactory.WSTrustChannelLockedProperties lockedProperties = new WSTrustChannelFactory.WSTrustChannelLockedProperties()
          {
            TrustVersion = this.GetTrustVersion(),
            Context = this.CreateSerializationContext()
          };
          lockedProperties.RequestSerializer = this.GetRequestSerializer(lockedProperties.TrustVersion);
          lockedProperties.ResponseSerializer = this.GetResponseSerializer(lockedProperties.TrustVersion);
          this._lockedProperties = lockedProperties;
          this._locked = true;
        }
        return this._lockedProperties;
      }
    }

    private WSTrustRequestSerializer GetRequestSerializer(
      TrustVersion trustVersion)
    {
      if (this._wsTrustRequestSerializer != null)
        return this._wsTrustRequestSerializer;
      if (trustVersion == TrustVersion.WSTrust13)
        return (WSTrustRequestSerializer) new WSTrust13RequestSerializer();
      if (trustVersion == TrustVersion.WSTrustFeb2005)
        return (WSTrustRequestSerializer) new WSTrustFeb2005RequestSerializer();
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID3137", (object) trustVersion.ToString())));
    }

    private WSTrustResponseSerializer GetResponseSerializer(
      TrustVersion trustVersion)
    {
      if (this._wsTrustResponseSerializer != null)
        return this._wsTrustResponseSerializer;
      if (trustVersion == TrustVersion.WSTrust13)
        return (WSTrustResponseSerializer) new WSTrust13ResponseSerializer();
      if (trustVersion == TrustVersion.WSTrustFeb2005)
        return (WSTrustResponseSerializer) new WSTrustFeb2005ResponseSerializer();
      throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new NotSupportedException(Microsoft.IdentityModel.SR.GetString("ID3137", (object) trustVersion.ToString())));
    }

    private TrustVersion GetTrustVersion()
    {
      TrustVersion trustVersion = this._trustVersion;
      if (trustVersion == null)
        trustVersion = (this.Endpoint.Binding.CreateBindingElements().Find<SecurityBindingElement>() ?? throw DiagnosticUtil.ExceptionUtil.ThrowHelperError((Exception) new InvalidOperationException(Microsoft.IdentityModel.SR.GetString("ID3269")))).MessageSecurityVersion.TrustVersion;
      return trustVersion;
    }

    protected virtual WSTrustSerializationContext CreateSerializationContext()
    {
      SecurityTokenResolver securityTokenResolver = this._securityTokenResolver;
      if (securityTokenResolver == null)
      {
        ClientCredentials credentials = this.Credentials;
        if (credentials.ClientCertificate != null && credentials.ClientCertificate.Certificate != null)
          securityTokenResolver = SecurityTokenResolver.CreateDefaultSecurityTokenResolver(new List<SecurityToken>()
          {
            (SecurityToken) new X509SecurityToken(credentials.ClientCertificate.Certificate)
          }.AsReadOnly(), false);
      }
      if (securityTokenResolver == null)
        securityTokenResolver = Microsoft.IdentityModel.Tokens.EmptySecurityTokenResolver.Instance;
      SecurityTokenResolver useKeyTokenResolver = this._useKeyTokenResolver ?? Microsoft.IdentityModel.Tokens.EmptySecurityTokenResolver.Instance;
      return new WSTrustSerializationContext(this._securityTokenHandlerCollectionManager, securityTokenResolver, useKeyTokenResolver);
    }

    private class WSTrustChannelLockedProperties
    {
      public TrustVersion TrustVersion;
      public WSTrustSerializationContext Context;
      public WSTrustRequestSerializer RequestSerializer;
      public WSTrustResponseSerializer ResponseSerializer;
    }
  }
}
