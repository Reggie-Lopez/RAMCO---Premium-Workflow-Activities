// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.WSTrust.IWSTrustFeb2005SyncContract
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
  [ServiceContract(Name = "IWSTrustFeb2005Sync", Namespace = "http://schemas.microsoft.com/ws/2008/06/identity/securitytokenservice")]
  [ComVisible(true)]
  public interface IWSTrustFeb2005SyncContract
  {
    [OperationContract(Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Cancel", Name = "TrustFeb2005Cancel", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel")]
    Message ProcessTrustFeb2005Cancel(Message message);

    [OperationContract(Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue", Name = "TrustFeb2005Issue", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue")]
    Message ProcessTrustFeb2005Issue(Message message);

    [OperationContract(Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Renew", Name = "TrustFeb2005Renew", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew")]
    Message ProcessTrustFeb2005Renew(Message message);

    [OperationContract(Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Validate", Name = "TrustFeb2005Validate", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate")]
    Message ProcessTrustFeb2005Validate(Message message);

    [OperationContract(Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", Name = "TrustFeb2005CancelResponse", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel")]
    Message ProcessTrustFeb2005CancelResponse(Message message);

    [OperationContract(Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", Name = "TrustFeb2005IssueResponse", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue")]
    Message ProcessTrustFeb2005IssueResponse(Message message);

    [OperationContract(Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", Name = "TrustFeb2005RenewResponse", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew")]
    Message ProcessTrustFeb2005RenewResponse(Message message);

    [OperationContract(Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", Name = "TrustFeb2005ValidateResponse", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate")]
    Message ProcessTrustFeb2005ValidateResponse(Message message);
  }
}
