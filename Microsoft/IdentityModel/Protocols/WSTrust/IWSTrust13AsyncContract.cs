// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.IWSTrust13AsyncContract
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ServiceContract(Name = "IWSTrust13Async", Namespace = "http://schemas.microsoft.com/ws/2008/06/identity/securitytokenservice")]
  [ComVisible(true)]
  public interface IWSTrust13AsyncContract
  {
    [OperationContract(Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Cancel", AsyncPattern = true, Name = "Trust13CancelAsync", ReplyAction = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal")]
    IAsyncResult BeginTrust13Cancel(
      Message request,
      AsyncCallback callback,
      object state);

    Message EndTrust13Cancel(IAsyncResult ar);

    [OperationContract(Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue", AsyncPattern = true, Name = "Trust13IssueAsync", ReplyAction = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal")]
    IAsyncResult BeginTrust13Issue(
      Message request,
      AsyncCallback callback,
      object state);

    Message EndTrust13Issue(IAsyncResult ar);

    [OperationContract(Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Renew", AsyncPattern = true, Name = "Trust13RenewAsync", ReplyAction = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal")]
    IAsyncResult BeginTrust13Renew(
      Message request,
      AsyncCallback callback,
      object state);

    Message EndTrust13Renew(IAsyncResult ar);

    [OperationContract(Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Validate", AsyncPattern = true, Name = "Trust13ValidateAsync", ReplyAction = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal")]
    IAsyncResult BeginTrust13Validate(
      Message request,
      AsyncCallback callback,
      object state);

    Message EndTrust13Validate(IAsyncResult ar);

    [OperationContract(Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Cancel", AsyncPattern = true, Name = "Trust13CancelResponseAsync", ReplyAction = "*")]
    IAsyncResult BeginTrust13CancelResponse(
      Message request,
      AsyncCallback callback,
      object state);

    Message EndTrust13CancelResponse(IAsyncResult ar);

    [OperationContract(Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Issue", AsyncPattern = true, Name = "Trust13IssueResponseAsync", ReplyAction = "*")]
    IAsyncResult BeginTrust13IssueResponse(
      Message request,
      AsyncCallback callback,
      object state);

    Message EndTrust13IssueResponse(IAsyncResult ar);

    [OperationContract(Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Renew", AsyncPattern = true, Name = "Trust13RenewResponseAsync", ReplyAction = "*")]
    IAsyncResult BeginTrust13RenewResponse(
      Message request,
      AsyncCallback callback,
      object state);

    Message EndTrust13RenewResponse(IAsyncResult ar);

    [OperationContract(Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Validate", AsyncPattern = true, Name = "Trust13ValidateResponseAsync", ReplyAction = "*")]
    IAsyncResult BeginTrust13ValidateResponse(
      Message request,
      AsyncCallback callback,
      object state);

    Message EndTrust13ValidateResponse(IAsyncResult ar);
  }
}
