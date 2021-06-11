// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.IWSTrustFeb2005AsyncContract
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ServiceContract(Name = "IWSTrustFeb2005Async", Namespace = "http://schemas.microsoft.com/ws/2008/06/identity/securitytokenservice")]
  [ComVisible(true)]
  public interface IWSTrustFeb2005AsyncContract
  {
    [OperationContract(Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Cancel", AsyncPattern = true, Name = "TrustFeb2005CancelAsync", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel")]
    IAsyncResult BeginTrustFeb2005Cancel(
      Message request,
      AsyncCallback callback,
      object state);

    Message EndTrustFeb2005Cancel(IAsyncResult ar);

    [OperationContract(Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue", AsyncPattern = true, Name = "TrustFeb2005IssueAsync", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue")]
    IAsyncResult BeginTrustFeb2005Issue(
      Message request,
      AsyncCallback callback,
      object state);

    Message EndTrustFeb2005Issue(IAsyncResult ar);

    [OperationContract(Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Renew", AsyncPattern = true, Name = "TrustFeb2005RenewAsync", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew")]
    IAsyncResult BeginTrustFeb2005Renew(
      Message request,
      AsyncCallback callback,
      object state);

    Message EndTrustFeb2005Renew(IAsyncResult ar);

    [OperationContract(Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Validate", AsyncPattern = true, Name = "TrustFeb2005ValidateAsync", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate")]
    IAsyncResult BeginTrustFeb2005Validate(
      Message request,
      AsyncCallback callback,
      object state);

    Message EndTrustFeb2005Validate(IAsyncResult ar);

    [OperationContract(Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", AsyncPattern = true, Name = "TrustFeb2005CancelResponseAsync", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel")]
    IAsyncResult BeginTrustFeb2005CancelResponse(
      Message request,
      AsyncCallback callback,
      object state);

    Message EndTrustFeb2005CancelResponse(IAsyncResult ar);

    [OperationContract(Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", AsyncPattern = true, Name = "TrustFeb2005IssueResponseAsync", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue")]
    IAsyncResult BeginTrustFeb2005IssueResponse(
      Message request,
      AsyncCallback callback,
      object state);

    Message EndTrustFeb2005IssueResponse(IAsyncResult ar);

    [OperationContract(Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", AsyncPattern = true, Name = "TrustFeb2005RenewResponseAsync", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew")]
    IAsyncResult BeginTrustFeb2005RenewResponse(
      Message request,
      AsyncCallback callback,
      object state);

    Message EndTrustFeb2005RenewResponse(IAsyncResult ar);

    [OperationContract(Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", AsyncPattern = true, Name = "TrustFeb2005ValidateResponseAsync", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate")]
    IAsyncResult BeginTrustFeb2005ValidateResponse(
      Message request,
      AsyncCallback callback,
      object state);

    Message EndTrustFeb2005ValidateResponse(IAsyncResult ar);
  }
}
