// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Tokens.FailedAuthenticationFaultException
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Tokens
{
  [ComVisible(true)]
  public class FailedAuthenticationFaultException : FaultException
  {
    public FailedAuthenticationFaultException()
      : this(new FaultReason(Microsoft.IdentityModel.SR.GetString("ID3242")))
    {
    }

    public FailedAuthenticationFaultException(FaultReason reason)
      : base(reason, new FaultCode("Sender", new FaultCode("FailedAuthentication", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")))
    {
    }
  }
}
