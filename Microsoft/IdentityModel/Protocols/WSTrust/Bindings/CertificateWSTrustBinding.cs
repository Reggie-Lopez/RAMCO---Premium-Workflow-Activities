// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.Bindings.CertificateWSTrustBinding
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
  public class CertificateWSTrustBinding : WSTrustBindingBase
  {
    public CertificateWSTrustBinding()
      : this(SecurityMode.Message)
    {
    }

    public CertificateWSTrustBinding(SecurityMode securityMode)
      : base(securityMode)
    {
    }

    protected override SecurityBindingElement CreateSecurityBindingElement()
    {
      if (SecurityMode.Message == this.SecurityMode)
        return SecurityBindingElement.CreateMutualCertificateBindingElement();
      return SecurityMode.TransportWithMessageCredential == this.SecurityMode ? (SecurityBindingElement) SecurityBindingElement.CreateCertificateOverTransportBindingElement() : (SecurityBindingElement) null;
    }

    protected override void ApplyTransportSecurity(HttpTransportBindingElement transport)
    {
      transport.AuthenticationScheme = AuthenticationSchemes.Anonymous;
      if (!(transport is HttpsTransportBindingElement transportBindingElement))
        return;
      transportBindingElement.RequireClientCertificate = true;
    }
  }
}
