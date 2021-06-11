// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.Bindings.WSTrustBindingBase
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSTrust.Bindings
{
  [ComVisible(true)]
  public abstract class WSTrustBindingBase : Binding
  {
    private SecurityMode _securityMode = SecurityMode.Message;
    private TrustVersion _trustVersion = TrustVersion.WSTrust13;
    private bool _enableRsaProofKeys;

    protected WSTrustBindingBase(SecurityMode securityMode)
      : this(securityMode, TrustVersion.WSTrust13)
    {
    }

    protected WSTrustBindingBase(SecurityMode securityMode, TrustVersion trustVersion)
    {
      if (trustVersion == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (trustVersion));
      this.ValidateTrustVersion(trustVersion);
      WSTrustBindingBase.ValidateSecurityMode(securityMode);
      this._securityMode = securityMode;
      this._trustVersion = trustVersion;
    }

    public bool EnableRsaProofKeys
    {
      get => this._enableRsaProofKeys;
      set => this._enableRsaProofKeys = value;
    }

    public SecurityMode SecurityMode
    {
      get => this._securityMode;
      set
      {
        WSTrustBindingBase.ValidateSecurityMode(value);
        this._securityMode = value;
      }
    }

    public TrustVersion TrustVersion
    {
      get => this._trustVersion;
      set
      {
        if (value == null)
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (value));
        this.ValidateTrustVersion(value);
        this._trustVersion = value;
      }
    }

    public override string Scheme
    {
      get
      {
        TransportBindingElement transportBindingElement = this.CreateBindingElements().Find<TransportBindingElement>();
        return transportBindingElement == null ? string.Empty : transportBindingElement.Scheme;
      }
    }

    public override BindingElementCollection CreateBindingElements()
    {
      BindingElementCollection elementCollection = new BindingElementCollection();
      elementCollection.Clear();
      if (SecurityMode.Message == this._securityMode || SecurityMode.TransportWithMessageCredential == this._securityMode)
        elementCollection.Add((BindingElement) this.ApplyMessageSecurity(this.CreateSecurityBindingElement()));
      elementCollection.Add((BindingElement) this.CreateEncodingBindingElement());
      elementCollection.Add((BindingElement) this.CreateTransportBindingElement());
      return elementCollection.Clone();
    }

    protected abstract SecurityBindingElement CreateSecurityBindingElement();

    protected virtual MessageEncodingBindingElement CreateEncodingBindingElement() => (MessageEncodingBindingElement) new TextMessageEncodingBindingElement()
    {
      ReaderQuotas = {
        MaxArrayLength = 2097152,
        MaxStringContentLength = 2097152
      }
    };

    protected static void ValidateSecurityMode(SecurityMode securityMode)
    {
      if (securityMode != SecurityMode.None && securityMode != SecurityMode.Message && (securityMode != SecurityMode.Transport && securityMode != SecurityMode.TransportWithMessageCredential))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (securityMode));
      if (securityMode == SecurityMode.None)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3224"));
    }

    protected void ValidateTrustVersion(TrustVersion trustVersion)
    {
      if (trustVersion != TrustVersion.WSTrust13 && trustVersion != TrustVersion.WSTrustFeb2005)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (trustVersion));
    }

    protected virtual HttpTransportBindingElement CreateTransportBindingElement()
    {
      HttpTransportBindingElement transport = SecurityMode.Message != this._securityMode ? (HttpTransportBindingElement) new HttpsTransportBindingElement() : new HttpTransportBindingElement();
      transport.MaxReceivedMessageSize = 2097152L;
      if (SecurityMode.Transport == this._securityMode)
        this.ApplyTransportSecurity(transport);
      return transport;
    }

    protected abstract void ApplyTransportSecurity(HttpTransportBindingElement transport);

    protected virtual SecurityBindingElement ApplyMessageSecurity(
      SecurityBindingElement securityBindingElement)
    {
      if (securityBindingElement == null)
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull(nameof (securityBindingElement));
      securityBindingElement.MessageSecurityVersion = TrustVersion.WSTrustFeb2005 != this._trustVersion ? MessageSecurityVersion.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10 : MessageSecurityVersion.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10;
      if (this._enableRsaProofKeys)
      {
        RsaSecurityTokenParameters securityTokenParameters = new RsaSecurityTokenParameters();
        securityTokenParameters.InclusionMode = SecurityTokenInclusionMode.Never;
        securityTokenParameters.RequireDerivedKeys = false;
        securityBindingElement.OptionalEndpointSupportingTokenParameters.Endorsing.Add((SecurityTokenParameters) securityTokenParameters);
      }
      return securityBindingElement;
    }
  }
}
