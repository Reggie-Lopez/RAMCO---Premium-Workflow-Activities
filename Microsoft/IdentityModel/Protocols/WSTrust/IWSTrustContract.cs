// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.IWSTrustContract
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ServiceContract]
  [ComVisible(true)]
  public interface IWSTrustContract
  {
    [OperationContract(Action = "*", Name = "Cancel", ReplyAction = "*")]
    Message Cancel(Message message);

    [OperationContract(Action = "*", AsyncPattern = true, Name = "Cancel", ReplyAction = "*")]
    IAsyncResult BeginCancel(
      Message message,
      AsyncCallback callback,
      object asyncState);

    Message EndCancel(IAsyncResult asyncResult);

    [OperationContract(Action = "*", Name = "Issue", ReplyAction = "*")]
    Message Issue(Message message);

    [OperationContract(Action = "*", AsyncPattern = true, Name = "Issue", ReplyAction = "*")]
    IAsyncResult BeginIssue(Message message, AsyncCallback callback, object asyncState);

    Message EndIssue(IAsyncResult asyncResult);

    [OperationContract(Action = "*", Name = "Renew", ReplyAction = "*")]
    Message Renew(Message message);

    [OperationContract(Action = "*", AsyncPattern = true, Name = "Renew", ReplyAction = "*")]
    IAsyncResult BeginRenew(Message message, AsyncCallback callback, object asyncState);

    Message EndRenew(IAsyncResult asyncResult);

    [OperationContract(Action = "*", Name = "Validate", ReplyAction = "*")]
    Message Validate(Message message);

    [OperationContract(Action = "*", AsyncPattern = true, Name = "Validate", ReplyAction = "*")]
    IAsyncResult BeginValidate(
      Message message,
      AsyncCallback callback,
      object asyncState);

    Message EndValidate(IAsyncResult asyncResult);
  }
}
