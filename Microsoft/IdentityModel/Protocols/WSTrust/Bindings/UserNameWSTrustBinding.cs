// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.Bindings.UserNameWSTrustBinding
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Net;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Protocols.WSTrust.Bindings
{
  [ComVisible(true)]
  public class UserNameWSTrustBinding : WSTrustBindingBase
  {
    private HttpClientCredentialType _clientCredentialType;

    public UserNameWSTrustBinding()
      : this(SecurityMode.Message, HttpClientCredentialType.None)
    {
    }

    public UserNameWSTrustBinding(SecurityMode securityMode)
      : base(securityMode)
    {
      if (SecurityMode.Message != securityMode)
        return;
      this._clientCredentialType = HttpClientCredentialType.None;
    }

    public UserNameWSTrustBinding(SecurityMode mode, HttpClientCredentialType clientCredentialType)
      : base(mode)
    {
      if (!UserNameWSTrustBinding.IsHttpClientCredentialTypeDefined(clientCredentialType))
        throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (clientCredentialType));
      this._clientCredentialType = SecurityMode.Transport != mode || HttpClientCredentialType.Digest == clientCredentialType || HttpClientCredentialType.Basic == clientCredentialType ? clientCredentialType : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3225", (object) clientCredentialType));
    }

    public HttpClientCredentialType ClientCredentialType
    {
      get => this._clientCredentialType;
      set
      {
        if (!UserNameWSTrustBinding.IsHttpClientCredentialTypeDefined(value))
          throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange(nameof (value));
        this._clientCredentialType = SecurityMode.Transport != this.SecurityMode || HttpClientCredentialType.Digest == value || HttpClientCredentialType.Basic == value ? value : throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(Microsoft.IdentityModel.SR.GetString("ID3225", (object) value));
      }
    }

    private static bool IsHttpClientCredentialTypeDefined(HttpClientCredentialType value) => value == HttpClientCredentialType.None || value == HttpClientCredentialType.Basic || (value == HttpClientCredentialType.Digest || value == HttpClientCredentialType.Ntlm) || value == HttpClientCredentialType.Windows || value == HttpClientCredentialType.Certificate;

    protected override SecurityBindingElement CreateSecurityBindingElement()
    {
      if (SecurityMode.Message == this.SecurityMode)
        return (SecurityBindingElement) SecurityBindingElement.CreateUserNameForCertificateBindingElement();
      return SecurityMode.TransportWithMessageCredential == this.SecurityMode ? (SecurityBindingElement) SecurityBindingElement.CreateUserNameOverTransportBindingElement() : (SecurityBindingElement) null;
    }

    protected override void ApplyTransportSecurity(HttpTransportBindingElement transport)
    {
      if (this._clientCredentialType == HttpClientCredentialType.Basic)
        transport.AuthenticationScheme = AuthenticationSchemes.Basic;
      else
        transport.AuthenticationScheme = AuthenticationSchemes.Digest;
    }
  }
}
