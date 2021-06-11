// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.IWSTrust13SyncContract
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ServiceContract(Name = "IWSTrust13Sync", Namespace = "http://schemas.microsoft.com/ws/2008/06/identity/securitytokenservice")]
  [ComVisible(true)]
  public interface IWSTrust13SyncContract
  {
    [OperationContract(Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Cancel", Name = "Trust13Cancel", ReplyAction = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal")]
    Message ProcessTrust13Cancel(Message message);

    [OperationContract(Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue", Name = "Trust13Issue", ReplyAction = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal")]
    Message ProcessTrust13Issue(Message message);

    [OperationContract(Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Renew", Name = "Trust13Renew", ReplyAction = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal")]
    Message ProcessTrust13Renew(Message message);

    [OperationContract(Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Validate", Name = "Trust13Validate", ReplyAction = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal")]
    Message ProcessTrust13Validate(Message message);

    [OperationContract(Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Cancel", Name = "Trust13CancelResponse", ReplyAction = "*")]
    Message ProcessTrust13CancelResponse(Message message);

    [OperationContract(Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Issue", Name = "Trust13IssueResponse", ReplyAction = "*")]
    Message ProcessTrust13IssueResponse(Message message);

    [OperationContract(Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Renew", Name = "Trust13RenewResponse", ReplyAction = "*")]
    Message ProcessTrust13RenewResponse(Message message);

    [OperationContract(Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Validate", Name = "Trust13ValidateResponse", ReplyAction = "*")]
    Message ProcessTrust13ValidateResponse(Message message);
  }
}
