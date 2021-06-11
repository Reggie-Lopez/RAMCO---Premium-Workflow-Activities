// Decompiled with JetBrains decompiler
// Type: Microsoft.IdentityModel.Protocols.RequestFaultException
// Assembly: Premium Workflow Activities, Version=1.0.7559.16014, Culture=neutral, PublicKeyToken=cff3f52c5f80d6c2
// MVID: 17367862-F6A9-4287-953B-58B91807BBFD
// Assembly location: C:\Users\rlopez\OneDrive - National Association of Realtors\Dev\RAMCO\Premium Workflow Activities_1.0.7559.16014.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols
{
  [ComVisible(true)]
  [Serializable]
  public abstract class RequestFaultException : FaultException
  {
    protected RequestFaultException(FaultReason reason, FaultCode code)
      : base(reason, code)
    {
    }

    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
  }
}
