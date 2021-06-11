// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.IWSTrustChannelContract
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ServiceContract]
  [ComVisible(false)]
  public interface IWSTrustChannelContract : IWSTrustContract
  {
    RequestSecurityTokenResponse Cancel(RequestSecurityToken request);

    IAsyncResult BeginCancel(
      RequestSecurityToken request,
      AsyncCallback callback,
      object state);

    void EndCancel(IAsyncResult result, out RequestSecurityTokenResponse response);

    SecurityToken Issue(RequestSecurityToken request);

    SecurityToken Issue(
      RequestSecurityToken request,
      out RequestSecurityTokenResponse response);

    IAsyncResult BeginIssue(
      RequestSecurityToken request,
      AsyncCallback callback,
      object asyncState);

    SecurityToken EndIssue(
      IAsyncResult result,
      out RequestSecurityTokenResponse response);

    RequestSecurityTokenResponse Renew(RequestSecurityToken request);

    IAsyncResult BeginRenew(
      RequestSecurityToken request,
      AsyncCallback callback,
      object state);

    void EndRenew(IAsyncResult result, out RequestSecurityTokenResponse response);

    RequestSecurityTokenResponse Validate(RequestSecurityToken request);

    IAsyncResult BeginValidate(
      RequestSecurityToken request,
      AsyncCallback callback,
      object state);

    void EndValidate(IAsyncResult result, out RequestSecurityTokenResponse response);
  }
}
